$(document).ready(function () {
    var ar = window.location.pathname.split("/");
    var statement = ar[ar.length - 1];
    statement = (ar == ",") ? "Home" : statement; //Ternary operator whoop
    $("ul.navbar-nav li a:contains('" + statement + "')").css("color", "#ff0000").parent(".navbar-li").addClass("current");
});


