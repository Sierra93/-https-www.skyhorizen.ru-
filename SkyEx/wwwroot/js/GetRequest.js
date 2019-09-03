// Отправка заявки на бэк
$(window).ready(function () {
    console.log("Страница загружена");
});
var onPressGetRequest = () => {
    console.log("onPressGetRequest");
    var sUserName = document.getElementById("idName").value;     // Получаем имя клиента
    var sEmail = document.getElementById("idEmailOrNumber").value;    // Получаем почту или телефон
    var sMultiText = document.getElementById("idMultiInput").value;    // Получаем описание проекта из мультиинпута
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
            // Если успешно
            if (data === "OK") {
                // В случае успешной отправки, очищаем все поля
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
        }
    });
};

