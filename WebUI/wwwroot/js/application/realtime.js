var url = window.location.origin + '/realtime';
var method = 'ReceiveMessage';

const connection = new signalR.HubConnectionBuilder()
    .withUrl(url)
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start().then(function (){
    console.log('listening')
}).catch(function(err) {
    console.error(err.toString());
});

var sendMessage = function (type, message) {
    connection.invoke(method, type, message).then(function() {
        $("#message").val('');
    }).catch(function(err) {
        console.error(err.toString());
    });
}

var listenMessage = function (callback) {
    connection.on(method, (type, message) => {
        callback(type, message);
    });
}