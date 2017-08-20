using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    string LiveConStr = ConfigurationManager.ConnectionStrings["LiveConStrAddress"].ConnectionString;
    string LiveConStrIP = ConfigurationManager.ConnectionStrings["LiveConStrIP"].ConnectionString;
    string LocalConStr = ConfigurationManager.ConnectionStrings["LocalConStr"].ConnectionString;
    string CommsListNames = "";

    SqlConnection con = null;
    SqlCommand com;

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        con = new SqlConnection(LocalConStr);
        com = new SqlCommand("", con);
    }

    [WebMethod]
    public string Register(string fname, string lname, string uname, string pass, string email)
    {

        string output;
        com.CommandText =
       " SELECT [ID] " +
       " FROM [dbo].[UsersTB] " +
       " WHERE(UserName = @uname) ";
        com.Parameters.Add(new SqlParameter("@uname", uname));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader.Read())
        {
            output = "User Name already registred, please try again.";
            return output;
        }
        com.Connection.Close();
        com.CommandText =
           " SELECT [ID] " +
           " FROM [dbo].[UsersTB] " +
           " WHERE(Email = @email) ";
        com.Parameters.Add(new SqlParameter("@email", email));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader.Read())
        {
            output = "Email already registred, please try again.";
            return output;
        }
        com.Connection.Close();
        com.CommandText =
            " INSERT INTO [dbo].[UsersTB] ([FirstName] ,[LastName] ,[UserName], [Password], [Email]) " +
            " VALUES (@fname,@lname,@uname,@pass,@email) ";
        com.Parameters.Add(new SqlParameter("@fname", fname));
        com.Parameters.Add(new SqlParameter("@lname", lname));
        com.Parameters.Add(new SqlParameter("@pass", pass));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Final success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }

    [WebMethod]
    public string Login(string uname, string pass)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string output;
        com.CommandText =
            " SELECT * " +
            " FROM dbo.UsersTB " +
            " WHERE(UserName = '" + uname + "') AND(Password = @passPar) ";
        com.Parameters.Add(new SqlParameter("@passPar", pass));

        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }

        SqlDataReader reader = com.ExecuteReader();
        if (reader.Read())
        {
            User usr = new User()
            {
                Name = reader["FirstName"].ToString(),
                Family = reader["LastName"].ToString(),
                eMail = reader["Email"].ToString(),
                uName = reader["UserName"].ToString(),
                uID = reader["ID"].ToString(),
            };

            output = serializer.Serialize(usr);
            //output= "hello " + reader["Name"].ToString() + ", " + reader["Family"].ToString();
        }
        else
        {
            output = "no user found!";
        }
        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string NewGroup(string gName, string gSum, string uname, string isPub)
    {
        string newID;
        string output;
        com.CommandText =
       " SELECT [ID] " +
       " FROM [dbo].[GroupNames] " +
       " WHERE(GroupName = @gName) ";
        com.Parameters.Add(new SqlParameter("@gName", gName));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader.Read())
        {
            output = "Group Name already registred, please try again.";
            return output;
        }
        com.Connection.Close();
        com.CommandText =
            " INSERT INTO [dbo].[GroupNames] ([Creator] ,[GroupName] ,[Summery], [isPub], [MembersCount]) " +
            " VALUES (@uName,@gName,@gSum,@isPub, 1) ";
        com.Parameters.Add(new SqlParameter("@uName", uname));
        com.Parameters.Add(new SqlParameter("@gSum", gSum));
        com.Parameters.Add(new SqlParameter("@isPub", isPub));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        reader.Read();
        com.Connection.Close();
        com.CommandText = " SELECT MAX(ID) " +
        " FROM [dbo].[GroupNames] ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        reader.Read();
        newID = reader[0].ToString();
        com.Connection.Close();
        com.CommandText =
          " INSERT INTO [dbo].[GroupMembers] ([GroupID] ,[UserName] ,[Owner], [Manager]) " +
          " VALUES ("+newID+",@uName    ,1,1) ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader.Read())
        {
            output="Yes";
        }
        else output="Not";
        com.Connection.Close();
        return output;
    }

    [WebMethod]
    public string GroupListMostMembers()
    {
        string output = null;
        com.CommandText =
       " SELECT * " +
       " FROM [dbo].[GroupNames] " +
       " ORDER BY [MembersCount] DESC ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        while (reader.Read())
        {
            output += "<div class='container'><div class='col-md-4'><h3>" + reader["GroupName"].ToString() + "</h3></div><p class='well black-white-sw'>";
            output += "<span class='hLights'>Creator:</span> " + reader["Creator"].ToString() + "<br/>";
            //output += "Group ID : " + reader["ID"].ToString() + "<br/>";
            output += "<span class='hLights'>Summery:</span> " + reader["Summery"].ToString() + "<br/>";
            output += "<span class='hLights'>Members:</span> " + reader["MembersCount"].ToString() + "<br/>";
            if (reader["isPub"].ToString() == "True")
            {
                output += "<span id='pub'>Public</span><br/>";
                output += "<span value='pub'><button type='button' class='btn btn-success' value='" + reader["ID"].ToString() + "' class='pub'>Enter</button></span>";

            }
            else
            {
                output += "<span id='prv'>Private</span><br/>";
                output += "<span value='prv'><button type='button' class='btn btn-success' value='" + reader["ID"].ToString() + "' class='prv'>Enter</button></span>";
            }

            output += "</p></div>";
        }
        com.Connection.Close();
        return output;
    }

    [WebMethod]
    public string GroupsList()
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        string output = null;
        com.CommandText =
       " SELECT * " +
       " FROM [dbo].[GroupNames] " +
       " ORDER BY [ID] DESC ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        while (reader.Read())
        {
            output += "<div class='container'><div class='col-md-4'><h3>" + reader["GroupName"].ToString() + "</h3></div><p class='well black-white-sw'>";
            output += "<span class='hLights'>Creator:</span> " + reader["Creator"].ToString() + "<br/>";
            //output += "Group ID : " + reader["ID"].ToString() + "<br/>";
            output += "<span class='hLights'>Summery:</span> " + reader["Summery"].ToString() + "<br/>";
            output += "<span class='hLights'>Members:</span> " + reader["MembersCount"].ToString() + "<br/>";
            if (reader["isPub"].ToString() == "True")
            {
                output += "<span id='pub'>Public</span><br/>";
                output += "<span value='pub'><button type='button' class='btn btn-success' value='" + reader["ID"].ToString() + "' class='pub'>Enter</button></span>";
                
            }
            else
            {
                output += "<span id='prv'>Private</span><br/>";
                output += "<span value='prv'><button type='button' class='btn btn-success' value='" + reader["ID"].ToString() + "' class='prv'>Enter</button></span>";
            }
           
            output += "</p></div>";
        }
        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string MyGroups(string uName)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        int count = 0;
        string output = null;
        com.CommandText =
       " SELECT * " +
       " FROM dbo.GroupNames " +
       " FULL JOIN dbo.GroupMembers ON dbo.GroupNames.ID = dbo.GroupMembers.GroupID " +
       " WHERE dbo.GroupMembers.UserName='" + uName + "'";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();

        while (reader.Read())
        {
            count++;
            output += "<div class='container'><div class='col-md-4'><h3>" + reader["GroupName"].ToString() + "</h3></div><p class='well black-white-sw'>";
            output += "<span class='hLights'>Creator:</span> " + reader["Creator"].ToString() + "<br/>";
            //output += "Group ID : " + reader["ID"].ToString() + "<br/>";
            output += "<span class='hLights'>Summery:</span> " + reader["Summery"].ToString() + "<br/>";
            output += "<span class='hLights'>Members:</span> " + reader["MembersCount"].ToString() + "<br/>";
            if (reader["isPub"].ToString() == "True")
            {
                output += "<span id='pub'>Public</span><br/>";
                output += "<span value='pub'><button type='button' class='btn btn-success' value='" + reader["ID"].ToString() + "' class='pub'>Enter</button></span>";

            }
            else
            {
                output += "<span id='prv'>Private</span><br/>";
                output += "<span value='prv'><button type='button' class='btn btn-success' value='" + reader["ID"].ToString() + "' class='prv'>Enter</button></span>";
            }
            output += "</p></div>";
        }
        if(count==0)
        {
            output = "<h2>You are not member in any group</h2>";
        }
        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string ShowGroup(string uName, string gID, string isPub) // ShowGroup
    {
        int isMem = 0;
        bool isAdmin = false;
        string output = null;
        com.CommandText =
       " SELECT * " +
       " FROM [dbo].[GroupNames] " +
       " WHERE(ID = '" + gID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        SqlDataReader reader2;
        if (reader.Read())
        {
            SqlConnection con2 = new SqlConnection(LocalConStr);
            SqlCommand com2 = new SqlCommand("", con2);
            com2.CommandText =
            " SELECT Owner, Manager " +
            " FROM [dbo].[GroupMembers] " +
            " WHERE(GroupID = '" + gID + "' AND UserName = '" + uName + "') ";
            try
            {
                com2.Connection.Open();
            }
            catch (Exception ex)
            {
                return ex.Message + ", " + com2.CommandText;
            }
            output += "<h3>Welcome to " + reader["GroupName"].ToString() + "</h3>";
            com.Connection.Close();
            reader2 = com2.ExecuteReader();
            if (reader2.Read())
            {
                if (reader2[0].ToString() == "True" || reader2[1].ToString() == "True")
                {
                    isAdmin = true;
                    output += "<button type='button' class='btn btn-warning' id='adminBTN'>Admin Pannel</button>";
                }
                output += "<button id='leaveBTN' type='button' class='btn btn-danger'>Leave Us</button>";
                output += "<button id='postTopic' type='button' class='btn btn-primary'>Post</button><br/>";
                isMem = 1;
            }
            else
            {
                com.CommandText =
               " SELECT [GroupID] " +
               " FROM dbo.PendingUsers " +
               " WHERE (UserName='" + uName + "' AND GroupID='" + gID + "')";
                try
                {
                    com.Connection.Open();
                }
                catch (Exception ex)
                {
                    return ex.Message + ", " + com.CommandText;
                }
                reader = com.ExecuteReader();
                if(reader.Read())
                {
                    output += "<button type='button' class='btn btn-warning'>Requested</button>" + "<br/>";
                }
                else
                    output += "<button id='regBTN' type='button' class='btn btn-success'>Join Us</button>" + "<br/>";
                isMem = 0;
             

            }
            output += "<br/>";
            com2.Connection.Close();
        }
        com.Connection.Close();
        int count = 0;
        if (isPub == "pub" || isMem == 1)
        {
            com.CommandText =
             " SELECT * " +
            " FROM [dbo].[Topics] " +
            " WHERE(GroupID = '" + gID + "') " +
            " ORDER BY [Date] DESC ";
            try
            {
                com.Connection.Open();
            }
            catch (Exception ex)
            {
                return ex.Message + ", " + com.CommandText;
            }
            reader = com.ExecuteReader();
            while (reader.Read())
            {
                count++;
                output += "<div class='container'><div class='col-md-4'><h4>" + reader["Topic"].ToString() + "</h4></div><p class='well black-white-sw'>";
                output += "<span class='hLights'>Post by:</span> " + reader["UserName"].ToString() + "<br/>";
                output += "<span class='hLights'>Views:</span> " + reader["Views"].ToString() + "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp";
                output += "<span class='hLights'>Comments:</span> " + reader["Comments"].ToString() + "<br/>";
                output += "<span class='hLights'>Posted at:</span> " + reader["Date"].ToString() + "<br/>";
                output += "<button id='enterBtn' type='button' class='btn btn-default' value='" + reader["TopicID"].ToString() + "'>Enter</button>";
                if (isAdmin)
                    output += "<button id='delTopicBtn' type='button' class='btn btn-danger' value='" + reader["TopicID"].ToString() + "'>Delete</button>";
                output += "</p></div>";
            }
            if (count == 0)
            {
                output += "<h2>No topics yet.</h2>";
            }
        }

        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string UserReg(string gID, string isPub,string uName) // ShowGroup
    {
        string output = null;
        if (isPub == "pub")
        {
            com.CommandText =
             " INSERT INTO [dbo].[GroupMembers] ([GroupID] ,[UserName] ,[Owner] ,[Manager]) " +
             " VALUES (@gID,@uName,0,0) ";
            com.Parameters.Add(new SqlParameter("@gID", gID));
            com.Parameters.Add(new SqlParameter("@uName", uName));
               try
            {
                com.Connection.Open();
            }
            catch (Exception ex)
            {
                return ex.Message + ", " + com.CommandText;
            }
            SqlDataReader reader = com.ExecuteReader();
            if (reader.Read())
            {
                output = "Error.";
            }
            else output = "Done.";
            com.Connection.Close();
            com.CommandText =
             " UPDATE [dbo].[GroupNames] " +
             " SET MembersCount = MembersCount + 1 " +
             " WHERE(ID = '" + gID + "') ";
            try
            {
                com.Connection.Open();
            }
            catch (Exception ex)
            {
                return ex.Message + ", " + com.CommandText;
            }
            reader = com.ExecuteReader();
            if (reader != null)
            {
                output = "Success!";
            }
            else
            {
                output = "Failed!";
            }
            com.Connection.Close();
        }
        else
        {
            com.CommandText =
            " SELECT [GroupID] " +
             " FROM dbo.PendingUsers " +
             " WHERE (UserName='" + uName + "' AND GroupID='" + gID + "')";
            try
            {
                com.Connection.Open();
            }
            catch (Exception ex)
            {
                return ex.Message + ", " + com.CommandText;
            }
            SqlDataReader reader = com.ExecuteReader();
            if (reader.Read())
            {
                output = "You allready requested to join that group!";
                com.Connection.Close();
            }
            else
            {
                com.Connection.Close();
                com.CommandText =
                  " INSERT INTO [dbo].[PendingUsers] ([UserName] ,[GroupID]) " +
                  " VALUES ('" + uName + "','" + gID + "') ";
                try
                {
                    com.Connection.Open();
                }
                catch (Exception ex)
                {
                    return ex.Message + ", " + com.CommandText;
                }
                reader = com.ExecuteReader();
                if (reader.Read())
                {
                    output += "Error.";
                }
                else
                {
                    output += "Request sent.";
                }
                com.Connection.Close();
            }
        }
        return output;
    }


    [WebMethod]
    public string ProUser(string uName,string gID)
    {
        string output = null;
        com.CommandText =
          " UPDATE [dbo].[GroupMembers] " +
          " SET Manager = 'True' " +
          " WHERE(GroupID = '" + gID + "' AND UserName = '" + uName + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }

    [WebMethod]
    public string delTopic(string tID)
    {
        string output = null;
        com.CommandText =
         " DELETE FROM [dbo].[Topics] " +
         " WHERE (TopicID = " + tID + ") ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        com.CommandText =
        " DELETE FROM [dbo].[Comments] " +
        " WHERE (TopicID = " + tID + ") ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }

    [WebMethod]
    public string DeProUser(string uName, string gID)
    {
        string output = null;
        com.CommandText =
          " UPDATE [dbo].[GroupMembers] " +
          " SET Manager = 'False' " +
          " WHERE(GroupID = '" + gID + "' AND UserName = '" + uName + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }


    [WebMethod]
    public string UserDelFromGroup(string uName, string gID) // ShowGroup
    {
        string output = null;
        com.CommandText =
         " DELETE FROM [dbo].[GroupMembers] " +
         " WHERE (UserName = @uName AND GroupID = @gID) ";
        com.Parameters.Add(new SqlParameter("@gID", gID));
        com.Parameters.Add(new SqlParameter("@uName", uName));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        com.CommandText =
            " UPDATE [dbo].[GroupNames] " +
            " SET MembersCount = MembersCount - 1 " +
            " WHERE(ID = '" + gID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }

    [WebMethod]
    public string NewTopic(string topic, string contact, string userName, string groupID)
    {
        string output;
        com.CommandText =
            " INSERT INTO [dbo].[Topics] ([Topic] ,[UserName] ,[GroupID], [Views], [Comments], [Date]) " +
            " VALUES (@topic,@uName,@gID, 0, 0, '" + DateTime.Now.ToString() + "') ";
        com.Parameters.Add(new SqlParameter("@topic", topic));
        com.Parameters.Add(new SqlParameter("@uName", userName));
        com.Parameters.Add(new SqlParameter("@gID", groupID));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        com.Connection.Close();
        com.CommandText =
       " SELECT MAX(TopicID) " +
       " FROM [dbo].[Topics] ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        reader.Read();
        output = reader[0].ToString();
        com.Connection.Close();
        com.CommandText =
           " INSERT INTO [dbo].[Comments] ([TopicID] ,[UserName] ,[Contact], [Date]) " +
           " VALUES ('" + output + "',@uName,@contact,'" + DateTime.Now.ToString() + "') ";
        com.Parameters.Add(new SqlParameter("@contact", contact));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string ShowTopic(string tID)
    {
        com.CommandText =
       " SELECT [Topic] " +
       " FROM [dbo].[Topics] " +
       " WHERE(TopicID = '" + tID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        reader.Read();
        string output = "<span id='addSpot'></span>&nbsp&nbsp<button id='addCom' type='button' class='btn btn-primary'>Add Comment</button><br/><div class='container'><h4>" + reader[0].ToString() + "</h4>";
        com.Connection.Close();
        com.CommandText =
        " SELECT * " +
        " FROM [dbo].[Comments] " +
        " WHERE(TopicID = '" + tID + "') " +
        " ORDER BY Date DESC";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        while (reader.Read())
        {
            output += "<p class='well black-white-sw'><span class='hLights'>Post by:</span> " + reader["UserName"].ToString() + "<br/>";
            output += "<span class='hLights'>Post Date:</span> " + reader["Date"].ToString() + "<br/>";
            output += reader["Contact"].ToString() + "</p><hr/>";
        }
        output += "</div>";
        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string addCom(string tID, string uName, string cont)
    {
        string output = null;
        com.CommandText =
            " INSERT INTO [dbo].[Comments] ([TopicID] ,[UserName] ,[Contact], [Date]) " +
            " VALUES ('" + tID + "','" + uName + "','" + cont + "','" + DateTime.Now.ToString() + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        com.CommandText =
       " UPDATE [dbo].[Topics] " +
       " SET Comments = Comments + 1 " +
       " WHERE(TopicID = '" + tID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }


    [WebMethod]
    public string GroupName(string gID)
    {
        string output = null;
        com.CommandText =
     " SELECT [GroupName] " +
     " FROM [dbo].[GroupNames] " +
     " WHERE(ID = '" + gID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        reader.Read();
        output = reader[0].ToString();
        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string GetUser(string uName)
    {
        string output = null;
        com.CommandText =
         " SELECT * " +
         " FROM [dbo].[UsersTB] " +
         " WHERE(UserName = '" + uName + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        reader.Read();
        output = "<div class='well black-white-sw'><table><tr><td> <input type='text' id='fName' value='" + reader[0].ToString() + "'></td></tr>";
        output += "<tr><td> <input type='text' id='lName' value='" + reader[1].ToString() + "'></td></tr>";
        output += "<tr><td> <input type='text' id='uName' value='" + reader[2].ToString() + "'></td></tr>";
        output += "<tr><td> <input type='password' id='pwd' value='" + reader[3].ToString() + "'></td></tr>";
        output += "<tr><td><input type='email' id='email' value='" + reader[4].ToString() + "'></td></tr>";
        output += "<tr><td colspan='2'><button id='updateBtn' class='btn btn-success' type='button'>Update</button></td></tr></table></div>";
        com.Connection.Close();
        return output;
    }

    [WebMethod]
    public string EditUser(string uName, string fName, string lName, string pwrd, string eMail)
    {
        string output = null;
        com.CommandText =
           " UPDATE [dbo].[UsersTB] SET [FirstName] = '" + fName + "' ,[LastName] = '" + lName + "' ,[UserName] = '" + uName + "', [Password] = '" + pwrd + "', [Email]= '" + eMail + "' " +
           " WHERE (UserName = '" + uName + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string EditGroup(string gID)
    {
        string output = null;
        com.CommandText =
         " SELECT GroupName, Summery, isPub " +
         " FROM [dbo].[GroupNames] " +
         " WHERE(ID = '" + gID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        reader.Read();
        output = "<center><table><tr><td><span class='hLights'>Group Name:</span></td><td> <input type='text' id='gName' value='" + reader[0].ToString() + "'></td></tr>";
        output += "<tr><td><span class='hLights'>Summery:</span></td><td> <input type='text' id='Sum' value='" + reader[1].ToString() + "'></td></tr>";
        if(reader[2].ToString()=="True")
        {
            output += "<tr><td><input id='isPub' name='samename' type='radio' value='pub' checked/>Public</td><td> <input type='radio' name='samename' value='prv' />Private</td></tr>";
        }
        else
        {
            output += "<tr><td><input id='isPub' name='samename' type='radio' value='pub'/>Public</td><td> <input type='radio' name='samename' value='prv' checked/>Private</td></tr>";
        }
        output += "<tr><td colspan='2'><button id='updateGroup' class='btn btn-success' type='button'>Update</button></td></tr></table></center>";
        com.Connection.Close();
        return output;
    }
    [WebMethod]
    public string UpdateGroup(string gID,string gName,string gSum,string gPub)
    {
        string output = null;
        com.CommandText =
            " UPDATE [dbo].[GroupNames] " +
            " SET GroupName=@gName, Summery=@gSum, isPub=@isPub " +
            " WHERE(ID = '" + gID + "') ";
        com.Parameters.Add(new SqlParameter("@gName", gName));
        com.Parameters.Add(new SqlParameter("@gSum", gSum));
        com.Parameters.Add(new SqlParameter("@isPub", gPub));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }

    [WebMethod]
    public string penUsers(string gID)
    {
        int count = 0;
        string output = "<center><table>";
        com.CommandText =
       " SELECT [UserName] " +
       " FROM [dbo].[PendingUsers] " +
       " WHERE(GroupID = '" + gID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        while (reader.Read())
        {
            count++;
            output += "<tr><td>User Name: " + reader[0].ToString() + "&nbsp&nbsp</td><td><button value='" + reader[0].ToString() + "' id='confirmUser' type='button' class='btn btn-success'>Accept</button></td><td><button type='button' id='refuseBtn' value='" + reader[0].ToString() + "' class='btn btn-danger'>Refuse</button></td>";
        }
        output += "</table></center>";
        com.Connection.Close();
        if (count == 0)
            return "No pending users.";
        return output;
    }

    [WebMethod]
    public string RefuseUser(string gID,string uName)
    {
        string output = null;
        com.CommandText =
       " DELETE FROM [dbo].[PendingUsers] " +
       " WHERE(GroupID = '" + gID + "' AND UserName = '" + uName + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        reader.Read();
        com.Connection.Close();
        return output;
    }


    [WebMethod]
    public string ConfirmUser(string gID, string uName)
    {
        string output = null;
        com.CommandText =
       " DELETE FROM [dbo].[PendingUsers] " +
       " WHERE(GroupID = '" + gID + "' AND UserName = '" + uName + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        reader.Read();
        com.Connection.Close();
        com.CommandText =
            " INSERT INTO [dbo].[GroupMembers] ([GroupID] ,[UserName] ,[Owner] ,[Manager]) " +
            " VALUES (@gID,@uName,0,0) ";
        com.Parameters.Add(new SqlParameter("@gID", gID));
        com.Parameters.Add(new SqlParameter("@uName", uName));
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader.Read())
        {
            output = "Error.";
        }
        else output = "Done.";
        com.Connection.Close();
        com.CommandText =
           " UPDATE [dbo].[GroupNames] " +
           " SET MembersCount = MembersCount + 1 " +
           " WHERE(ID = '" + gID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        reader = com.ExecuteReader();
        if (reader != null)
        {
            output = "Success!";
        }
        else
        {
            output = "Failed!";
        }
        com.Connection.Close();
        return output;
    }
    
    [WebMethod]
    public string EditUsers(string gID)
    {
        string output = "<h3>Group Users:</h3>";
        com.CommandText =
       " SELECT [UserName] ,[Manager], [Owner] " +
       " FROM [dbo].[GroupMembers] " +
       " WHERE(GroupID = '" + gID + "') ";
        try
        {
            com.Connection.Open();
        }
        catch (Exception ex)
        {
            return ex.Message + ", " + com.CommandText;
        }
        SqlDataReader reader = com.ExecuteReader();
        output = "<center><table>";
        while (reader.Read())
        {
            if (reader[2].ToString()=="False")
            output += "<tr><td>" + reader[0].ToString() + "&nbsp&nbsp</td><td><button value='" + reader[0].ToString() +
            "' id='deleteUsr' type='button' class='btn btn-danger'>Delete User</button></td><td>";
            if (reader[1].ToString() == "False" && reader[2].ToString() == "False")
            {
                output += "<button value='" + reader[0].ToString() + "' type='button' class='btn btn-success' id='proUser'>Promote User</button><br/>";
            }
            else if (reader[2].ToString() == "False")
                output += "<button value='" + reader[0].ToString() + "' type='button' class='btn btn-warning' id='deProUser'>Remove Admin</button><br/>";
            output += "</td></tr>";
        }
        output += "</table></center>";
        com.Connection.Close();
        return output;
    }


    class User
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string uName { get; set; }
        public string eMail { get; set; }
        public string uID { get; set; }
    }
}