$(document).ready(init);

function init() {
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }

    $(document).on('change', '#selOp',sortBySel);
    $.ajax({
        url: WebServiceURL + "/GroupsList",
        dataType: "json",
        type: "POST", //use only POST!
        contentType: "application/json; charset=utf-8",
        error: function (jqXHR, exception) {
            alert(formatErrorMessage(jqXHR, exception));
        },
        success: function (data) {
            $("#output").append(data.d);
        }
    });
    $(document).on('click','button',function()
    {
        sessionStorage.setItem("prvpub", $(this).parent().attr('value'));
        sessionStorage.setItem('selectedGroup', this.value);
        window.location.href = "ShowGroup.html";
    })
}


function sortBySel()
{
    if ($('#selOp').val() == "Newest")
    {
        $("#output").html("");
        $.ajax({
            url: WebServiceURL + "/GroupsList",
            dataType: "json",
            type: "POST", //use only POST!
            contentType: "application/json; charset=utf-8",
            error: function (jqXHR, exception) {
                alert(formatErrorMessage(jqXHR, exception));
            },
            success: function (data) {
                $("#output").append(data.d);
            }
        });
    }
    else if ($('#selOp').val() == "MostUsers")
    {
        $("#output").html("");
        $.ajax({
            url: WebServiceURL + "/GroupListMostMembers",
            dataType: "json",
            type: "POST", //use only POST!
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
    }
    //var usr =
    // {
    //     tID: this.value
    // };
    //$.ajax({
    //    url: WebServiceURL + "/SortByGroups",
    //    dataType: "json",
    //    type: "POST", //use only POST!
    //    data: JSON.stringify(usr),
    //    contentType: "application/json; charset=utf-8",
    //    //error: function (jqXHR, exception) {
    //    //    alert(formatErrorMessage(jqXHR, exception));
    //    //},
    //    // async: false,
    //    success: function (grp) {
    //        //if (res != 'no user found!') {
    //        //  alert("blablabla");
    //        //}
    //        debugger;
    //        alert("??!WTFFFFF");
    //        //code
    //    }
    //});
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



/*Toggle Menu*/
