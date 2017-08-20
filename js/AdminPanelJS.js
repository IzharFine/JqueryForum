$(document).ready(init);

function init() {
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }
    var gID = sessionStorage.getItem('selectedGroup');
    var grp =
       {
           gID: gID,
       };
        $.ajax({
        url: WebServiceURL + "/GroupName",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(grp),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        success: function (data) {
            $("#welcomeGroup").append("Welcome to " + data.d + ", Admin Control Panel");
        }
        });
        $(document).on('click', '#EditGroup', EditGroup);
        $(document).on('click', '#updateGroup', updateGroup);
        $(document).on('click', '#PenUsers', penUsers);
        $(document).on('click', "#confirmUser", conUser);
        $(document).on('click', "#EditUsers", editUsers);
        $(document).on('click', '#deleteUsr', userDel);
        $(document).on('click', '#proUser', proUser);
        $(document).on('click', '#deProUser', deProUser);
        $(document).on('click', '#refuseBtn', refuseUser);
}

function refuseUser()
{
    var grp =
        {
            gID: sessionStorage.getItem('selectedGroup'),
            uName:this.value
        };
    $.ajax({
        url: WebServiceURL + "/RefuseUser",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(grp),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (user) {
            //code
        }
    });
    $("#output").html('');
}


function deProUser()
{
    var usr =
        {
            gID: sessionStorage.getItem('selectedGroup'),
            uName: this.value,
        };
    $.ajax({
        url: WebServiceURL + "/DeProUser",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(usr),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            //code
            
        }
    });
    location.reload();
}

function proUser()
{
    var usr =
        {
            gID: sessionStorage.getItem('selectedGroup'),
            uName: this.value,
        };
    $.ajax({
        url: WebServiceURL + "/ProUser",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(usr),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            //code
            location.reload();
        }
    });
}

function editUsers()
{
    var grp =
        {
            gID: sessionStorage.getItem('selectedGroup'),
        };
    $.ajax({
        url: WebServiceURL + "/EditUsers",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(grp),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            $("#output").append(data.d);
            //code
        }
    });
    $("#output").html('');
}

function conUser()
{
    var usr =
        {
            gID: sessionStorage.getItem('selectedGroup'),
            uName: this.value,
        };
    $.ajax({
        url: WebServiceURL + "/ConfirmUser",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(usr),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            //code
            $("#output").html('');
        }
    });
}

function penUsers()
{
    var grp =
        {
            gID: sessionStorage.getItem('selectedGroup'),
        };
    $.ajax({
        url: WebServiceURL + "/penUsers",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(grp),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            $("#output").append(data.d);
            //code
        }
    });
    $("#output").html('');
}

function updateGroup()
{
    var isPub = 0;
    if ($('#isPub').is(":checked")) {
        isPub = 1;
    }
    var gName = $("#gName").val();
    var gSum = $("#Sum").val();
    var gPub = isPub;
    var grp =
       {
           gID: sessionStorage.getItem('selectedGroup'),
           gName: gName,
           gSum: gSum,
           gPub:gPub
       };
    $.ajax({
        url: WebServiceURL + "/UpdateGroup",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(grp),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            
            //$("#output").append(data.d);
            //code
        }
    });
    $("#output").html('');
}

function EditGroup() {
    $("#output").html('');
    
    var grp =
       {
           gID: sessionStorage.getItem('selectedGroup'),
       };
    $.ajax({
        url: WebServiceURL + "/EditGroup",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(grp),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            
            $("#output").append(data.d);
            //code
        }
    });
    //location.reload();
}



function formatErrorMessage(jqXHR, exception) {
    if (jqXHR.status === 0) {
        return ('Not connected.\nPleaseverify your network connection.');
    } else if (jqXHR.status == 404) {
        return ('The requested page not found. [404]');
    } else if (jqXHR.status == 500) {
        return ('Internal Server Error [500].');
    } else if (exception === 'parsererror') {
        return ('Requested JSON parse failed.');
    } else if (exception === 'timeout') {
        return ('Time out error.');
    } else if (exception === 'abort') {
        return ('Ajax request aborted.');
    } else {
        return ('Uncaught Error.\n' + jqXHR.responseText);
    }
}

function userDel() {
    
    alert("?!?!");
    var usr =
      {
          uName: this.value,
          gID: sessionStorage.getItem('selectedGroup'),
      };
    $.ajax({
        url: WebServiceURL + "/UserDelFromGroup",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(usr),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            //if (res != 'no user found!') {
            //  alert("blablabla");
            //}
            
            $("#output").append(data.d);
            //code
            location.reload();
        }
    });
}