// Отправка заявки на бэк
window.onload = function(oEvent) {
    console.log("Страница загружена");
};
function onPressGetRequest(oEvent) {    
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
        success: function(data) {
            if (data === "OK") { 
                swal("Спасибо! Заявка отправлена!", "В ближайшее время я свяжусь с вами", "success");
                console.log("Заявка отправлена");
            }
            else { 
                console.log("Не все обязательные поля заполнены");
                return;
            }            
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log("Ошибка отправки заявки");
        }
    });    
};
