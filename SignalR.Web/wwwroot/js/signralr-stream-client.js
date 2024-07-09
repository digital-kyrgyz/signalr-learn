$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

    const broadcastStreamDataToAllClient = "BroadcastStreamDataToAllClient";
    const receiveMessageAsStreamForAllClient = "ReceiveMessageAsStreamForAllClient";

    const broadcastStreamProductToAllClient = "BroadcastStreamProductToAllClient";
    const receiveProductAsStreamForAllClient = "ReceiveProductAsStreamForAllClient";
    
    const broadcastFromHubToClients = "BroadcastFromHubToClients";
    
    //settings
    async function start() {
        try {
            await connection.start().then(() => {
                $("#connection-id").html("connection id: " + connection.connectionId);
                console.log("Connection opened with hub");
            });
        } catch (err) {
            console.log("Connection failed: ", err);
            setTimeout(() => start(), 3000);
        }

    }

    connection.onclose(async () => {
        await start();
    });

    //subscribe
    connection.on(receiveMessageAsStreamForAllClient, (name) => {
        $("#stream-box").append(`<p>${name}</p>`)
    });

    connection.on(receiveProductAsStreamForAllClient, (product) => {
        $("#stream-box").append(`<p>${product.id} - ${product.name} - ${product.price}</p>`)
    });

    //clicks
    $("#btn-stream").click(function () {
        const names = $("#stream").val();
        const namesAsChunk = names.split(";");
        const subject = new signalR.Subject();
        connection.send(broadcastStreamDataToAllClient, subject).catch(err => console.error(err));

        namesAsChunk.forEach(name => {
            subject.next(name);
        });

        subject.complete();
    });

    $("#btn-stream-product").click(function () {
        const productList = [
            {id: 1, name: "pen 1", price: 100},
            {id: 2, name: "pen 2", price: 200},
            {id: 3, name: "pen 3", price: 300},
        ];
        
        const subject = new signalR.Subject();
        connection.send(broadcastStreamProductToAllClient, subject).catch(err => console.error(err));

        productList.forEach(product => {
            subject.next(product);
        });

        subject.complete();
    });

    $("#btn-stream-from-hub-to-client").click(function (){
       connection.stream(broadcastFromHubToClients, 5).subscribe({
           next: (message) => $("#stream-box").append(`<p>${message}</p>`)
       });
    });

    start();
});