
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/NotificationHub")
    .withAutomaticReconnect([0, 0, 10000])
    .configureLogging(signalR.LogLevel.Information)
    .build();
async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};
connection.onclose(async () => {
    await start();
});
start();
connection.onreconnecting(error => {
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
});

connection.onreconnected(connectionId => {
    console.assert(connection.state === signalR.HubConnectionState.Connected);
});
