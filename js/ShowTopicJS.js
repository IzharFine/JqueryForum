$(document).ready(init);

function init() {
    var isOpen = 0;
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }
    $(document).on("click", "#addCom", function () {
        if (isOpen === 0)
        {
            $("#addSpot").append("<textarea type='text' id='inText'/><br/>");
            isOpen = 1;
        }
        else
        {
            if (document.getElementById("inText").value != "")
            {
                var comm =
           {
               tID: sessionStorage.getItem('selectedTopic'),
               uName: sessionStorage.getItem('uname'),
               cont: document.getElementById("inText").value
           };
                $.ajax({
                    url: WebServiceURL + "/addCom",
                    dataType: "json",
                    type: "POST", //use only POST!
                    data: JSON.stringify(comm),
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
            document.getElementById("addSpot").innerHTML = "";
            isOpen = 0;
        }
    });
    var usr =
           {
               tID: sessionStorage.getItem('selectedTopic'),
           };
    $.ajax({
        url: WebServiceURL + "/ShowTopic",
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

function userReg() {
    //var gID = sessionStorage.getItem('selectedGroup');
    //var uID = sessionStorage.getItem('uid');
    var usr =
       {
           uID: sessionStorage.getItem('uid'),
           gID: sessionStorage.getItem('selectedGroup'),
       };
    $.ajax({
        url: WebServiceURL + "/UserReg",
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
    var usr =
      {
          uID: sessionStorage.getItem('uid'),
          gID: sessionStorage.getItem('selectedGroup'),
      };
    $.ajax({
        url: WebServiceURL + "/UserDel",
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