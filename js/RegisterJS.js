$(document).ready(init);

function init() {
    var local = true;
    WebServiceURL = "http://ruppinmobile.ac.il.preview26.livedns.co.il/site02/WebService.asmx";
    if (local) {
        WebServiceURL = "WebService.asmx";
    }
    $(document).on('click', "#rpBtnRegister", rpREGbtn);
    $(document).on('click', "#cancel", function ()
    {
        window.history.back();
    })

}



function rpREGbtn() {
    var inputFName = $('#rpFirstName').val();
    var inputLName = $('#rpLastName').val();
    var inputUName = $('#rpUserName').val();
    var inputPass1 = $('#rpPass1').val();
    var inputPass2 = $('#rpPass2').val();
    var inputEmail = $('#rpEmail').val();
    if (inputPass1 != inputPass2) {
        $("#lpDvRes").html("<h2><font color='red'>Password not match.</font></h2>");
        return;
    }
    var user =
    {
        fname: inputFName,
        lname: inputLName,
        uname: inputUName,
        pass: inputPass1,
        email: inputEmail
    };
    //alert(user.uname);
    $.ajax({
        url: WebServiceURL + "/Register",
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
            //debugger;
            //alert(data);
            //alert(data.d);
            //debugger;
            var res = data.d;
            //alert(res);
            //if (res != 'no user found!') {
            //  alert("blablabla");
            //}
            //alert('in success');
            //code
            
            if (res == "User Name already registred, please try again." || res == "Email already registred, please try again.")
                $("#lpDvRes").html("<h2><font color='red'>" + data.d + "</font></h2>");
            else
                window.location.href = "index.html";
        }
    });

    //alert('END');
}