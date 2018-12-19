// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
$(document).ready(function() {
    $(".box .inner").on("click", function () {
        var box = $(this);
        var colorClass = "testColor1";
        
        if (box.hasClass(colorClass)) {
            box.removeClass(colorClass);
        } else {
            box.addClass("testColor1");
        }

        
    });
}); 