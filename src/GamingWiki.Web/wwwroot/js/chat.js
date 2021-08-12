var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/Chat")
        .build();

connection.on("NewMessage",
    function (message) {
        var chatInfo = `
<div class="d-flex justify-content-end mb-4">
    <div class="msg_cotainer_send">
       ${message.content}
       <span class="msg_time">${message.sentOn.toString()}</span>
    </div>
</div>`;
        $("#messagesList").append(chatInfo);
    });

$("#sendButton").click(function () {
    var message = $("#messageInput").val();
    var discussionId = $("#discussionId").val();
    connection.invoke("Send", message, discussionId );
    $("#messageInput").val('');
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}