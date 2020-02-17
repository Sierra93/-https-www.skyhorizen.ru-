// Отправка заявки на бэк
$(window).ready(function () {

});
var onPressGetRequest = () => {
    console.log("onPressGetRequest");
    var sUserName = document.getElementById("idName").value;
    var sEmail = document.getElementById("idEmailOrNumber").value;
    var sMultiText = document.getElementById("idMultiInput").value;
    var btnSend = document.getElementById("idBtnApplyRequest");
    btnSend.disabled = true;
    // Отправляем данные на бэк
    return $.ajax({
        url: '/GetRequest/SaveRequest/',
        type: "POST",
        dataType: "json",
        data: {
            sUserName: sUserName,
            sEmailOrNumber: sEmail,
            sMultiTextRequest: sMultiText
        },
        success: (data) => {
            if (data === "OK") {
                btnSend.disabled = false;
                document.getElementById("idName").value = "";
                document.getElementById("idEmailOrNumber").value = "";
                document.getElementById("idMultiInput").value = "";
                swal("Спасибо! Заявка отправлена!", "В ближайшее время я свяжусь с вами", "success");
                console.log("request send success");
            }
            else {
                swal("Не все обязательные поля заполнены!", "", "error");
                return;
            }
        },
        error: (XMLHttpRequest, textStatus, errorThrown) => {
            console.log("request send error");
            btnSend.disabled = false;
        }
    });
};

