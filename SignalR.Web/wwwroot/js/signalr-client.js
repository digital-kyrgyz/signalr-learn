$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder().withUrl("/exampleTypeSafeHub").configureLogging(signalR.LogLevel.Information).build();

    const broadcastMessageToAllClientHubMethodCall = "BroadcastMessageToAllClient";
    const receiveMessageForAllClientMethodCall = "ReceiveMessageForAllClient";

    const broadcastMessageToCallerClient = "BroadcastMessageToCallerClient";
    const receiveMessageForCallerClient = "ReceiveMessageForCallerClient";

    const broadcastMessageToOthersClient = "BroadcastMessageToOthersClient";
    const receiveMessageOthersCallerClient = "ReceiveMessageOthersCallerClient";

    const broadcastMessageToIndividualClient = "BroadcastMessageToIndividualClient";
    const receiveMessageForIndividualClient = "ReceiveMessageForIndividualClient";

    const broadcastMessageToGroupClient = "BroadcastMessageToGroupClient";
    const receiveMessageForGroupClient = "ReceiveMessageForGroupClient";

    const broadcastTypedMessageToAllClient = "BroadcastTypedMessageToAllClient";
    const receiveTypesMessageForAllClient = "ReceiveTypesMessageForAllClient";
    
    const receiveConnectedClientCountAllClient = "ReceiveConnectedClientCountAllClient";

    function start() {
        connection.start().then(() => {
            $("#connection-id").html("connection id: " + connection.connectionId);
            console.log("Connection opened with hub");
        });
    }

    try {
        start();
    } catch {
        setTimeout(() => start(), 5000);
    }
    //
    let connectedClientCount = $("#connected-client-count");

    connection.on(receiveConnectedClientCountAllClient, (count) => {
        connectedClientCount.text(count);
        console.log("Connected clients: ", count);
    });
    //

    //Start subscribe
    connection.on(receiveMessageForAllClientMethodCall, (message) => {
        console.log("Message: ", message);
    });
    connection.on(receiveTypesMessageForAllClient, (product) => {
        console.log("Message: ", product);
    });
    connection.on(receiveMessageForCallerClient, (message) => {
        console.log("(Caller) Message: ", message);
    });
    connection.on(receiveMessageOthersCallerClient, (message) => {
        console.log("(Others) Message: ", message);
    });
    connection.on(receiveMessageForIndividualClient, (message) => {
        console.log("(Individual) Message: ", message);
    });
    connection.on(receiveMessageForGroupClient, (message) => {
        console.log("(Group) Message: ", message);
    });
    //End subscribe

    $("#btn-send-message-all-client").click(function () {
        const message = "hello world";
        connection.invoke(broadcastMessageToAllClientHubMethodCall, message).catch(err => console.error("Error: " + err));
    });

    $("#btn-send-message-caller-client").click(function () {
        const message = "Caller client";
        connection.invoke(broadcastMessageToCallerClient, message).catch(err => console.error("Error: " + err));
    });

    $("#btn-send-message-others-client").click(function () {
        const message = "others client";
        connection.invoke(broadcastMessageToOthersClient, message).catch(err => console.error("Error: " + err));
    });

    $("#btn-send-message-individual-client").click(function () {
        const connectionId = $("#connectionId").val();
        const message = "individual client";
        connection.invoke(broadcastMessageToIndividualClient, connectionId, message).catch(err => console.error("Error: " + err));
    });


    //Group codes
    const groupA = "GroupA";
    const groupB = "GroupB";

    let currentGroupList = [];

    function refreshGoupList() {
        let groupList = $("#group-list").empty();
        currentGroupList.forEach(x => {
            groupList.append(`<p>${x}</p>`)
        });
    }

    $("#btn-group-a-add").click(function () {
        if (currentGroupList.includes(groupA)) return;
        connection.invoke("AddGroup", groupA).then(() => {
            currentGroupList.push(groupA);
            refreshGoupList();
        });
    });

    $("#btn-group-b-add").click(function () {
        if (currentGroupList.includes(groupB)) return;
        connection.invoke("AddGroup", groupB).then(() => {
            currentGroupList.push(groupB);
            refreshGoupList();
        })
    });

    $("#btn-group-a-remove").click(function () {
        if (!currentGroupList.includes(groupA)) return;
        connection.invoke("RemoveGroup", groupA).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupA);
            refreshGoupList();
        })
    });

    $("#btn-group-b-remove").click(function () {
        if (!currentGroupList.includes(groupB)) return;
        connection.invoke("RemoveGroup", groupB).then(() => {
            currentGroupList = currentGroupList.filter(x => x !== groupB);
            refreshGoupList();
        })
    });
    
    $("#btn-group-a-send-message").click(function (){
        const message = "Group A message";
        connection.invoke(broadcastMessageToGroupClient, groupA, message).catch(err => console.log("Error: ", err));
    });

    $("#btn-group-b-send-message").click(function (){
        const message = "Group B message";
        connection.invoke(broadcastMessageToGroupClient, groupB, message).catch(err => console.log("Error: ", err));
    });
    
    $("#btn-send-typed-message-all-client").click(function () {
        const product = { id: 1, name: "pencil 1", price: 200 };
        connection.invoke(broadcastTypedMessageToAllClient, product).catch(err => console.log("Error: ", err));
    });
}); 