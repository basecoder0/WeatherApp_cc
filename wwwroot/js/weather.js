$(document).ready(function () {

    $("#addBtn").click(function () {
        var model = $("#weatherForm").serialize()
        $.ajax({
            url: '/Home/GetWeather',
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
                $('#weatherBody').load("Weather.cshtml")
            }
        })
    })

    $("#weatherBody").on('click', '#dltBtn', function () {
        var id = $(this).closest('tr');
        $.ajax({
            url: '/Home/DeleteWeatherInfo',
            type: "POST",
            cache: false,
            data: { id: $(this).data("id") },
            success: function (msg) {
                $(id).remove();
            }
        })
    });
});