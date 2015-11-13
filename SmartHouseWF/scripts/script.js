$(document).ready(function () {
    $(".js_TimeDiv").each(function (index, value) {
        var clockElement = $(value).find(".js_timeField:first");
        var timestamp = $(value).find("input[type='hidden']:first").val();
        var disabled = false;
        if (timestamp == "disabled") {
            disabled = true;
        }
        else {
            timestamp = parseInt(timestamp);
        }
        $(clockElement).myClock({ "timestamp": timestamp, "disabled": disabled });
    });
});