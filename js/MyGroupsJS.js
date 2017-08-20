$(document).ready(init);

function init() {
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }
    $(document).on('click', 'button', function () {
        sessionStorage.setItem('selectedGroup', this.value);
        sessionStorage.setItem("prvpub", this.className);
        window.location.href = "ShowGroup.html";
    });
    var usr =
           {
               uName: sessionStorage.getItem('uname'),
           };
    $.ajax({
        url: WebServiceURL + "/MyGroups",
        dataType: "json",
        type: "POST", //use only POST!
        data: JSON.stringify(usr),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        success: function (data) {
            $("#output").append(data.d);
        }
    });
    //$(document).on('click', 'button', function () {
    //    sessionStorage.setItem('selectedGroup', this.value);
    //    window.location.href = "ShowGroup.html";
    //})
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

