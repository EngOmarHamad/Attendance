function showTime() {
    var date = new Date();
    const monthNames = [
        "January",
        "February",
        "March",
        "April",
        "May",
        "June",
        "July",
        "August",
        "September",
        "October",
        "November",
        "December",
    ];
    var days = [
        "Sunday",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday",
    ];
    var h = date.getHours();
    var min = date.getMinutes();
    var s = date.getSeconds();
    var d = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
    var m = monthNames[date.getMonth()];
    var y = date.getFullYear();
    var session = "AM";
    if (h == 0) {
        h = 12;
    }
    if (h >= 12) {
        session = "PM";
    }
    if (h > 12) {
        h = h - 12;
    }
    m = m < 10 ? (m = "0" + m) : m;
    s = s < 10 ? (s = "0" + s) : s;
    var date =
        days[date.getDay()] + "    " + d.toString() + "-" + m + "-" + y;
    var time = h + ":" + min + ":" + s + " " + session;
    $("#current-date").html(date);
    $("#current-date-time").html(time);
    setTimeout(showTime, 1000);
}
showTime();