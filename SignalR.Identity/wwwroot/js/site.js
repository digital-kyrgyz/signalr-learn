let toastTimeout;

$(document).ready(function () {
    const connection = new window.signalR.HubConnectionBuilder().withUrl("/hub").build();

    async function start() {
        try {
            await connection.start().then(() => {
                $("#connection-id").html("connection id: " + connection.connectionId);
                console.log("Connection opened with hub");
            });
        } catch (err) {
            console.log("Connection failed: ", err);
            setTimeout(() => start(), 5000);
        }

    }

    connection.onclose(async () => {
        await start();
    });

    start();

    //subscribe
    connection.on("AlertCompleteFile", (downloadPath) => {
        clearTimeout(toastTimeout);
        $(".toast-body").html(`<p>Excel process was done. Above link you may download</p><a href="${downloadPath}">Download</a>`);
        $("#liveToast").show();
    })

});

