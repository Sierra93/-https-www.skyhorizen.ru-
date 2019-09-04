$(window).ready(function () {
    //console.log("Детали доступны");
});
// Кнопка подробнее о проекте
var onPressBtnDetails = () => {
    var result = "";
    var selectedElem = $("#idImages:hover").find("p")[0].textContent;   // Получаем первый подходящий текст для поиска
    //var replaceSelectedElem = selectedElem.replace("Описание:", "");
    //var sr = replaceSelectedElem.split("\" ")[1];
    //var str = sr.split(".");
    //$("#idImages:hover").find("p")[0].textContent;   
    var strForBack = "";    // В эту переменную будем складывать результат строки и отправим на бэк
    // Ищем если в строке есть две кавычки, то будем сплитить строку и возьмем лишь то, что до двойных кавычек
    strForBack = selectedElem;
    return $.ajax({
        url: '/Details/SearchInDB/',
        type: "POST",
        dataType: "json",
        data: {
            ID: strForBack
        },
        success: (data) => {
            result = "OK";
            console.log(result);
            //window.location.href = 'https://localhost:44377/Details/Details';   // Для теста
        },
        error: (XMLHttpRequest, textStatus, errorThrown) => {
            result = "ERROR";
        }
    });
};