$(document).ready(function () {
    ////Shows and Hides popup window in weather view
    var stopAutoHide;   

    function showWindow() {
        $('#main-popup').show();
        $('#inBox').hide();
        stopAutoHide = setTimeout(hideWindow, 5000);
    }

    function hideWindow() {
        $('#main-popup').hide();
        $('#inBox').show();
    }
    setTimeout(showWindow, 1000);

    $("#noBtn").click(function () {
        hideWindow();
        clearTimeout(stopAutoHide)
    })

    ////Get user's location from DB
    $("#yesBtn").click(function getLocation() {
        var model = $("#weatherForm").serialize()
        $.ajax({
            url: '/Home/GetUserSignUpLoc',
            type: 'GET',
            cache: false,
            data: model,
            success: function (model) {
                if (model != null && model.errorMessage != "NotFound") {
                    var row = "";
                    var name = model.name;
                    if (model.weather[0].state != null) {
                        var state = model.weather[0].state;
                    } else {
                        var state = 'NA';
                    }
                    var temp = model.main.temp;
                    var description = model.weather[0].description;
                    var id = model.user_id;
                    var lon = model.coord.lon;
                    var lat = model.coord.lat;
                } else {
                    alert("Please enter a valid City and State name");
                    return null;
                }
                row += "<tr><td>" + name + "</td><td>" + state + "</td><td>" + temp + "\xB0F" + "</td><td>" + description + "</td><td> <span class='table-remove'> <button id='dltBtn' class='btn btn-danger btn-rounded btn-sm my-0y' type='button' data-id=" + id + "'_'" + lon + "'_'" + lat + ">Delete</button></span></td></tr>"
                $('#weatherBody').append(row);
                $('#weatherBody').load("Weather.cshtml");
            }
        })
    })
});