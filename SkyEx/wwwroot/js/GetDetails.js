$(window).ready(function () {
    //console.log("Детали доступны");
});
// Кнопка подробнее о проекте
let onPressBtnDetails = () => {
    let result = "";
    let selectedElem = $("#idImages:hover").find("p")[2].textContent;   // Получаем первый подходящий текст для поиска
    let replaceSelectedElem = selectedElem.replace("Описание:", "");
    let sr = replaceSelectedElem.split("\" ")[1];
    let str = sr.split(".");
    //$("#idImages:hover").find("p")[0].textContent;   
    var strForBack = "";    // В эту переменную будем складывать результат строки и отправим на бэк
    // Ищем если в строке есть две кавычки, то будем сплитить строку и возьмем лишь то, что до двойных кавычек
    if (replaceSelectedElem.indexOf("\"") > 0) {
        strForBack = str[0];
            //replaceSelectedElem.split("\" ")[1];
            //replaceSelectedElem.split(" \"")[2];
    }
    else {
        strForBack = replaceSelectedElem;
    }
    return $.ajax({
        url: '/Details/SearchInDB/',
        type: "POST",
        dataType: "json",
        data: {
            Title: strForBack
        },
        success: (data) => {
            result = "OK";
            console.log(result);
            //console.log(data);
            //window.location.href = 'https://localhost:44377/Details/Details';   // Для теста
        },
        error: (XMLHttpRequest, textStatus, errorThrown) => {
            result = "ERROR";
        }
    });
};