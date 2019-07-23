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
                //alert("Спасибо! Заявка отправлена"); 
                console.log("Заявка отправлена");
            }
            else { 
                //alert("Не все обязательные поля заполнены"); 
                console.log("Не все обязательные поля заполнены");
                return;
            }            
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log("Ошибка отправки заявки");
        }
    });    
};
