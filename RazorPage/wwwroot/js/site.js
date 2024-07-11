$(() => {
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

    connection.on("ReceiveMessageWithData", async function (data) {
        console.log("Message received with data: ", data);
        await LoadRooms(data);
    });
    connection.on("ReceiveMessage", async function (message) {
        console.log("Message received: ", message);
    });

    async function LoadRooms(data) {
        var tr = '';
        $.each(data.$values, (k, v) => {
            tr += `<tr>
                    <td>${v.roomNumber}</td>
                    <td>${v.roomType.roomTypeName}</td>
                    <td>${v.roomPricePerDay}</td>
                    <td>${v.roomStatus}</td>
                    <td>${v.roomDetailDescription}</td>
                    <td>${v.roomMaxCapacity}</td>
                    <td>
                        <form method="post" action="/Rooms?handler=AddToCart">
                            <input type="hidden" name="roomId" value="${v.roomId}">
                            <button type="submit" class="btn btn-primary">Add to Cart</button>
                        </form>
                    </td>
                </tr>`
        })
        $("#tableBody").html(tr);
    };
});