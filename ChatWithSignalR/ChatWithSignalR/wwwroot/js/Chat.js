$(function () {

    var $SendMessageTextArea = $("#SendMessageTextArea");
    var $UserListSelectBox = $("#UserList");
    var $SendMessageBtn = $("#SendMessageBtn");
    var $MessageBox = $("#MessageBox");

    var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

    signalRConnection.on("ChatChannel", function (message) {
        $MessageBox.append("<strong>Gelen Mesaj :</strong>" + message + "</br>");
    });

    $SendMessageBtn.click(function () {

        var userId = $UserListSelectBox.find(":selected").val();
        var message = $SendMessageTextArea.val();
        
        $MessageBox.append(message + "</br>");
        $SendMessageTextArea.val("");
        signalRConnection.invoke("SendMessage", message, userId);
    });

    signalRConnection.start().then(function () {

    }).catch(function (err) {
        return console.error(err.toString());
    });

})