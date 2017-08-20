$(document).ready(init);

function init() {
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }
    $(document).on('click', '#cancel', function () {
        window.history.back();
    });
    $(document).on('click','#CreateGroupBTN',createNewGroup)
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


function createNewGroup()
{
    var isPub = 0;
        var GroupName = $('#grpName').val();
        var GroupSummery = $('#grpSum').val();
        if ($('#isPub').is(":checked")) {
            isPub = 1;
        }
        var group =
        {
            gName: GroupName,
            gSum: GroupSummery,
            uname: sessionStorage.getItem('uname'),
            isPub:isPub,
        };
        $.ajax({
            url: WebServiceURL + "/NewGroup",
            dataType: "json",
            type: "POST", //use only POST!
            //data: "{'name':'" + name + "' , " +
            //      " 'pass':'" + pass + "' }",
            //data: "{}",
            data: JSON.stringify(group),
            contentType: "application/json; charset=utf-8",
            error: function (jqXHR, exception) {
                alert(formatErrorMessage(jqXHR, exception));
            },
            // async: false,
            success: function (data) {
                //if (res != 'no user found!') {
                //  alert("blablabla");
                //}
                if (data.d == "Group Name already registred, please try again.") {
                    alert(data.d);
                }
                else {
                    //alert('Group created');
                    //location.reload();
                    window.history.back();
                }
                //code
            }
        });

}