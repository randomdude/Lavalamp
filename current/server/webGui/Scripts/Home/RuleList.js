﻿$(document).ready(function () {
    $('.post-link').click(function() {
        var form = $(this).closest("form");
        var input = $("<input>").attr("type", "hidden").attr("name", "selectedRule").val($(this).html());
        form.append($(input));
        form.submit();
        return false;
    });
    
});