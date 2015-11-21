$(document).ready(function () {
    // IClock
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

        // Validation
        var submitButton = $(value).find(".js_TimeSetSubmit:first");

        $(submitButton).on("click", function (event) {
            var regexp = /^[0-9]{1,2}$/;
            var hoursField = $(value).find(".js_HoursSetField:first");
            var hours = hoursField[0].value;
            var minutesField = $(value).find(".js_MinutsSetField:first");
            var minutes = minutesField[0].value;

            if (hours.match(regexp) == null) {
                $.jGrowl("This field must contain digits");
                $(hoursField).stop(true).animate({ 'backgroundColor': 'red' }, 2000, 'easeOutQuint').
				animate({ 'backgroundColor': 'white' }, 2000, 'easeInQuint');
                event.preventDefault();
                return;
            }
            hours = parseInt(hours);

            if (minutes.match(regexp) == null) {
                $.jGrowl("This field must contain digits");
                $(minutesField).stop(true).animate({ 'backgroundColor': 'red' }, 2000, 'easeOutQuint').
				animate({ 'backgroundColor': 'white' }, 2000, 'easeInQuint');
                event.preventDefault();
                return;
            }
            minutes = parseInt(minutes);

            if (hours < 0 || hours > 23) {
                $.jGrowl("this field value is incorrect");
                $(hoursField).stop(true).animate({ 'backgroundColor': 'red' }, 2000, 'easeOutQuint').
				animate({ 'backgroundColor': 'white' }, 2000, 'easeInQuint');
                event.preventDefault();
                return;
            }
            if (minutes < 0 || minutes > 59) {
                $.jGrowl("this field value is incorrect");
                $(minutesField).stop(true).animate({ 'backgroundColor': 'red' }, 2000, 'easeOutQuint').
				animate({ 'backgroundColor': 'white' }, 2000, 'easeInQuint');
                event.preventDefault();
                return;
            }
        });
    });

    // ITimer
    //
    $(".js_TimerDiv").each(function (index, value) {
    // js_DeviceName
    //$(".device").each(function (index, value) {
    
        var sendButton = $(value).find(".js_TimerButton");
        $(sendButton).on("click", function (event) {
            // alert("Я вызван!");
            $.ajax({
                type: 'GET',
                url: "Index.aspx/TimerHandler",
                success: function() {
                    alert("Запрос выполнен успешно!");
                },
                error: function() {
                    alert("Ошибка при выполнении запроса!");
                }
            });
        });
    });
});