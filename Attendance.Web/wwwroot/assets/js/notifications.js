const notify = (message) => {
    var messageText = message.title;
    if (window.Notification) {
        if (Notification.permission === 'granted') {
            const notify = new Notification(messageText);
            notify.onclick = function () { window.open(`${message.url}`); };
        } else {
            Notification.requestPermission().then(function (p) {
                if (p === 'granted') {
                    const notify = new Notification(messageText)
                    notify.onclick = function () {
                        window.open(`${message.url}`);
                    };
                } else {
                    console.log('User has blocked notifications.')
                }
            }).catch(function (err) {
                console.error(err)
            });
        }
    }

    (new Audio('/sounds/tone1.wav')).play();
    //$("#notification-toast").stop().slideDown('slow');
}
$(document).ready(function () {
    GetNotificationPartial()
    connection.on("ReceiveNotification", (message) => {
        notify(message);
        GetNotificationPartial();
    });

});





function GetNotificationPartial() {
    $.ajax({
        url: '/Notifications/GetPartial',
        success: function (data) {
            console.log(data)
            $('#notification-list').html(data);
            $('.readNoti').on("click", function (e) {
                console.log($('.readNoti'))
                $(this).attr("data-id")
                $.ajax({
                    url: '/Notifications/readNotification',
                    method: 'POST',
                    data: {
                        Id: $(this).attr("data-id")
                    },
                    success: function (response) {
                        GetNotificationPartial();
                    }
                });
            });
        },
    });

    $.ajax({
        url: '/Notifications/GetNotificationCount',
        success: function (data) {
            console.log(data)
            $('.notificationCount').html(data);
        },
    });
}



//$(` <a href="#" class="iq-sub-card">
//<div class="d-flex align-items-center">
//    <img class="p-1 avatar-40 rounded-pill bg-soft-primary" src="@item.Icon" alt="">
//    <div class="ms-3 w-100">
//        <h6 class="mb-0 ">@item.Text</h6>
//        <div class="d-flex justify-content-between align-items-center">
//            <p class="mb-0">95 MB</p>
//            <small class="float-end font-size-12">
//                @item.DateAdded.GetPrettyDate()
//            </small>
//        </div>
//    </div>
//</div></a>`).insertAfter("");
// $('#notification_no_data').hide();
//$(`<li class="nav-item border-bottom">` +
//    `<a href="${message.url}" class="nav-link">` +
//    `<div class="align-items-center me-3 notify-icon bg-${message.color} brround box-shadow-primary">` +
//    `<i class=" ${message.icon}"></i>` +
//    `</div><div class="notify-h mt-1"><h5> ${message.title} </h5>` +
//    `<span>${message.date}</span> ` +
//    `</div></a></li>`

//).insertAfter('.notification-content');



//$(".recent-close").click(function () {
//    $(".recent-purchase").stop().slideUp('slow');
//});
