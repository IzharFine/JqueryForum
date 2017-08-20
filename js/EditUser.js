//if (navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry|IEMobile)/)) {
//    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site14/WebService.asmx";
//    document.addEventListener("deviceready", init, false);//this is mobile app
//}
//else {
//    WebServiceURL = "WebService.asmx";

//    $(document).ready(init); //this is the browser
//}

$(document).ready(init);

function init() {
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }
    var inputuName = $('#lpUserName').val();
    var inputPass = $('#lpTxtPass').val();
    if ($('#lpUserName').val() == "" || $('#lpTxtPass').val() == "") {
        alert("U didnt fill at least one of the fields.");
        return;
    }
    var user =
    {
        uName:sessionStorage.getItem("uname")
    };
    $.ajax({
        url: WebServiceURL + "/GetUser",
        dataType: "json",
        type: "POST", //use only POST!
        //data: "{'name':'" + name + "' , " +
        //      " 'pass':'" + pass + "' }",
        //data: "{}",
        data: JSON.stringify(user),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            //alert(data);
            //alert(data.d);
            //debugger;
            $("#output").append(data.d);
            //code
        }
    });
    $(document).on('click', '#updateBtn', updateUsr);
}

//Functions declerations

//string uName, string fName, string lName, string pwrd, string eMail
function updateUsr()
{
    var user =
    {
        uName: sessionStorage.getItem("uname"),
        fName: $('#fName').val(),
        lName: $('#lName').val(),
        pwrd: $('#pwd').val(),
        eMail: $('#email').val(),
    };
    $.ajax({
        url: WebServiceURL + "/EditUser",
        dataType: "json",
        type: "POST", //use only POST!
        //data: "{'name':'" + name + "' , " +
        //      " 'pass':'" + pass + "' }",
        //data: "{}",
        data: JSON.stringify(user),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            //alert(data);
            //alert(data.d);
            //debugger;
            //code
        }
    });
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




//function loginBTNfunc() {
//    var inputuName = $('#lpUserName').val();
//    var inputPass = $('#lpTxtPass').val();
//    if ($('#lpUserName').val() == "" || $('#lpTxtPass').val() == "") {
//        alert("U didnt fill at least one of the fields.");
//        return;
//    }
//    var user =
//    {
//        uname: inputuName,
//        pass: inputPass
//    };
//    $.ajax({
//        url: WebServiceURL + "/Login",
//        dataType: "json",
//        type: "POST", //use only POST!
//        //data: "{'name':'" + name + "' , " +
//        //      " 'pass':'" + pass + "' }",
//        //data: "{}",
//        data: JSON.stringify(user),
//        contentType: "application/json; charset=utf-8",
//        error: function (jqXHR, exception) {
//            alert(formatErrorMessage(jqXHR, exception));
//        },
//        // async: false,
//        success: function (data) {
//            //alert(data);
//            //alert(data.d);
//            //debugger;
//            var res = data.d;
//            if (res != 'no user found!') {
//                debugger;
//                //var UserObj = JSON.parse(res);
//                //$('#lpDvRes').text(UserObj.ID + ", " + UserObj.Name + ", " + UserObj.Family + ", " + UserObj.Pass);
//                window.location.href = "GroupsList.html";
//                var json = JSON.parse(data.d);
//                sessionStorage.setItem('name', json.Name);
//                sessionStorage.setItem('uname', json.uName);
//                sessionStorage.setItem('uid', json.uID);
//                //$('#WelcomeName').text(+"<p>"+json.name+"</p>");
//            }
//            else {
//                alert("no user found");
//            }
//            //code
//        }
//    });

//}
