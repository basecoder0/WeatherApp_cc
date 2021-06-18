$(document).ready(function () {
    var stopAutoHide;
    function showWindow() {
        $('#main').show();
        stopAutoHide = setTimeout(hideWindow, 5000);
    }

    function hideWindow() {
        $('#main').hide();
    }
    setTimeout(showWindow, 2000);

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
                if (model != null) {
                    var row = "";
                    var name = model.name;
                    var state = model.weather[0].state;
                    var temp = model.main.temp;
                    var description = model.weather[0].description;
                    var id = model.user_id;
                    var lon = model.coord.lon;
                    var lat = model.coord.lat;
                }
                row += "<tr><td>" + name + "</td><td>" + state + "</td><td>" + temp + "\xB0F" + "</td><td>" + description + "</td><td> <span class='table-remove'> <button id='dltBtn' class='btn btn-danger btn-rounded btn-sm my-0y' type='button' data-id=" + id + "'_'" + lon + "'_'" + lat + ">Delete</button></span></td></tr>"
                $('#weatherBody').append(row)
            }
        })
    })
});