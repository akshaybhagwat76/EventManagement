function my_code() {
 

    var url_string = window.location.href;
    var url = new URL(url_string);
    var c = url.searchParams.get("email");

    const sEmails = c.split('@');
    var use = sEmails[0];
    var domain = sEmails[1];
    document.getElementById("ValidEmail").value = use +"@" + domain;//Now you get the js variable inside your form element
    console.log("params");

    console.log(c);
 
}



window.onload = my_code();