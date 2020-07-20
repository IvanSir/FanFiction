// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $("#changeTheme").click(function () {

        if ($(this).hasClass("fa-moon")) {
            $(this).removeClass("fa-moon").addClass("fa-sun");
            $("#themeStyle").prop("href", "/lib/bootstrap/dist/css/bootstrap.css");
        } else {
            $(this).removeClass("fa-sun").addClass("fa-moon");
            $("#themeStyle").prop("href", "/lib/bootstrap/dist/css/bootstrap-DarkTheme.css");
        }

        var theme;
        if ($(this).hasClass("fa-moon"))
            theme = "light";
        else theme = "dark";

        $.post("/Home/SetTheme",
            {
                theme: theme
            });
    });
});
