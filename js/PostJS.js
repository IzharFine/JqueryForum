$(document).ready(init);

function init() {
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }
    $(document).on('click', "#post", postBtn);
    $(document).on('click', "#cancel", function () {
        window.history.back();
    })

}



function postBtn() {
    var postTopic = $('#topic').val();
    var postContact = $('#contact').val();
    var post =
    {
        topic: postTopic,
        contact: postContact,
        userName: sessionStorage.getItem('uname'),
        groupID: sessionStorage.getItem('selectedGroup')
    };
    $.ajax({
        url: WebServiceURL + "/NewTopic",
        dataType: "json",
        type: "POST", //use only POST!
        //data: "{'name':'" + name + "' , " +
        //      " 'pass':'" + pass + "' }",
        //data: "{}",
        data: JSON.stringify(post),
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        // async: false,
        success: function (data) {
            window.history.back();
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