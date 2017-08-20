$(document).ready(init);

function init() {
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }
    $(document).on('click', '#regBTN', userReg);
    $(document).on('click', '#leaveBTN', userDel);
    $(document).on('click', '#enterBtn', function () {
            sessionStorage.setItem("isPub", this.className);
            sessionStorage.setItem('selectedTopic', this.value);
        window.location.href = "ShowTopic.html";
    });
    $(document).on('click', '#adminBTN', function () {
        window.location.href = "AdminPanel.html";
    });
    $(document).on('click', '#postTopic', function () {
        window.location.href = "Post.html";
    });
    $(document).on('click', '#delTopicBtn', delTopic);
    var usr =
           {
               uName: sessionStorage.getItem('uname'),
               gID: sessionStorage.getItem('selectedGroup'),
               isPub: sessionStorage.getItem('isPub')
           };
    $.ajax({
        url: WebServiceURL + "/ShowGroup",
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
        }
    });
}

function delTopic()
{
    var usr =
     {
         tID: this.value
     };
    $.ajax({
        url: WebServiceURL + "/delTopic",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(usr),
        contentType: "application/json; charset=utf-8",
        //error: function (jqXHR, exception) {
        //    alert(formatErrorMessage(jqXHR, exception));
        //},
        // async: false,
        success: function (data) {
            //if (res != 'no user found!') {
            //  alert("blablabla");
            //}
            alert("??!WTFFFFF");
            //code
        }
    });
    location.reload();
}

function userReg()
{
    var usr =
       {
           gID: sessionStorage.getItem('selectedGroup'),
           isPub: sessionStorage.getItem('prvpub'),
           uName: sessionStorage.getItem('uname')
       };
    $.ajax({
        url: WebServiceURL + "/UserReg",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(usr),
        contentType: "application/json; charset=utf-8",
        //error: function (jqXHR, exception) {
        //    alert(formatErrorMessage(jqXHR, exception));
        //},
        // async: false,
        success: function (data) {
            //if (res != 'no user found!') {
            //  alert("blablabla");
            //}
            $("#output").append(data.d);
            //code
        }
    });
    location.reload();
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

function userDel()
{
    var usr =
      {
          uName: sessionStorage.getItem('uname'),
          gID: sessionStorage.getItem('selectedGroup'),
      };
    $.ajax({
        url: WebServiceURL + "/UserDelFromGroup",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(usr),
        contentType: "application/json; charset=utf-8",
        //error: function (jqXHR, exception) {
        //    alert(formatErrorMessage(jqXHR, exception));
        //},
        // async: false,
        success: function (data) {
            //if (res != 'no user found!') {
            //  alert("blablabla");
            //}

            //code
        }
    });
    location.reload();
}