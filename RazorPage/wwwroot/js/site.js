    $(document).ready(() => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/bookingHub")
        .build();

    connection.start()
        .then(() => {
            console.log("SignalR connected.");
        })
        .catch(err => {
            console.error("SignalR connection error: ", err.toString());
        });

    connection.on("ReceiveMessage", async function (message) {
        console.log("Message received: ", message);
        // Reload room data or perform necessary updates here
        // Reload the room listing page
        location.reload();
    });
});