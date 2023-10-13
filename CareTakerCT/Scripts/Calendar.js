function generateCalendar(appointments) {
    console.log(appointments);

    var events = [];
    for (var i = 0; i < appointments.length; i++) {
        var e;
        if (moment(appointments[i].BookTime) < (Date.now())) {
            e = {
                id: appointments[i].Id,
                title: "Patient Appointment",
                description: appointments[i].Description,
                start: moment(appointments[i].BookTime).format("YYYY-MM-DDTHH:mm:ss"),
                end: moment(appointments[i].BookTime).add(1, "hours").format("YYYY-MM-DDTHH:mm:ss"), 
                isBooked: true,
                color: "#0000FF",
            };
        } else {
            e = {
                id: appointments[i].Id,
                title: "Patient Appointment",
                description: appointments[i].Description,
                start: moment(appointments[i].BookTime).format("YYYY-MM-DDTHH:mm:ss"),
                end: moment(appointments[i].BookTime).add(1, "hours").format("YYYY-MM-DDTHH:mm:ss"), 
                isBooked: true,
                color: "#666666", 
            };
        }
        events.push(e);
    }



    console.log(events);

    $("#calendar").fullCalendar({
        defaultView: "month",
        contentHeight: 600,
        businessHours: true,
        header: {
            left: "month, agendasWeek, today",
            center: "title",
            right: "prevYear, prev, next, nextYear"
        },
        events: events,
    });
}