$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();
    async function start() {
        try {
            connection.start().then(() => {
                $("#connection-id").html("connection id: " + connection.connectionId);
                console.log("Connection opened with hub");
            });
        } catch(err) {
            console.log("Connection failed: ", err);
            setTimeout(() => start(), 5000);
        }

    }
    
    connection.onclose(async () => {
        await start();
    });

    start();
});