window.customJs = {
    showAlert: function (message) {
        alert("Mensagem via jQuery: " + message);
    },
    animateCounter: function () {
        $("#counterText").fadeOut(200).fadeIn(200);
    }
};