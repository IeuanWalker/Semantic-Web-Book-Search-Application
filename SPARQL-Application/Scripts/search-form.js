$(document).ready(function(){
    $(".control").click(function () {
        $("body").addClass("mode-search");
        $(".input-search").focus();
    });

    $(".icon-close").click( function(){
        $("body").removeClass("mode-search");
    });
});