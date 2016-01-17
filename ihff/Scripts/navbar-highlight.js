$(document).ready(function () {
    var ar = window.location.pathname.split("/");
    var statement = ar[ar.length - 1];

    statement = (statement == "FoodFilm") ? "Food & Film Ticket" : statement;
    statement = (ar == ",") ? "Home" : statement; //Ternary operator whoop
    //$("ul.navbar-nav li a:contains('" + statement + "')").css("color", "#e4494f").parent(".navbar-li").addClass("current");
    $("ul.navbar-nav li a").filter(function () {
        return $(this).text() === statement;
    }).css("color", "#e4494f").parent(".navbar-li").addClass("current");

});


