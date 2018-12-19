// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
$(document).ready(function() {
    $(".board-box .board-inner").on("click", function () {
        var box = $(this);

        $("input[name='x']").val(box.attr("x"));
        $("input[name='y']").val(box.attr("y"));
    });
}); 