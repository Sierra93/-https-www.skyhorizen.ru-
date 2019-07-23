using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyEx.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace SkyEx.Controllers {
    // Сохранение новой заявки в БД
    public class GetRequestController : Controller {
        public string sResult = "";     // Флаг ошибки
        [HttpPost]
        // GetRequestModel - Модель данных, с которой здесь работаем
        // Метод для работы с завками и сохраняем в БД
        public JsonResult SaveRequest(GetRequestModel request) {
            string connectionString = "Server=u499383.mssql.masterhost.ru,1433;Initial Catalog=u499383_skyexdb;Persist Security Info=False;User ID=u499383;Password=inessen9hoo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=true;Connection Timeout=30";
            using (SqlConnection strConn = new SqlConnection(connectionString)) {
                try {
                    var sClientName = request.sUserName;    // Имя клиента
                    var sEmailOrNumber = request.sEmailOrNumber;    // Почта или телефон клиента
                    var sCommentResuest = request.sMultiTextRequest;    // Краткое описание проекта
                    // Проверяем все ли поля заполненны
                    if (string.IsNullOrWhiteSpace(sClientName) || (string.IsNullOrWhiteSpace(sEmailOrNumber))) {
                        sResult = "Error";
                    }
                    // Если ощибок нет, то разрешаем сохранение заявки
                    if (sResult != "Error") {
                        string comandDB = "SELECT * From REQUEST WHERE EMAIL_OR_NUMBER = '" + sEmailOrNumber + "'";   // Строка запроса к БД
                        SqlCommand checkRequest = new SqlCommand(comandDB, strConn);   // Команда к БД 
                        strConn.Open();
                        // Проверем, существует ли заявка в БД 
                        //if (checkRequest.ExecuteScalar() != null) {
                        using (var con = new SqlConnection(connectionString)) {
                            con.Open();
                            // Сохранение заявки в БД
                            using (var com = new SqlCommand("INSERT INTO REQUEST(CLIENT_NAME, EMAIL_OR_NUMBER, COMMENT_REQUEST) VALUES('" + sClientName + "', '" + sEmailOrNumber + "', '" + sCommentResuest + "')", con)) {
                                com.ExecuteNonQuery();
                                sResult = "OK";
                                SendMessage(request);
                            }
                        }
                        //}   
                    }
                }
                catch (Exception ex) {
                    new Exception(ex.Message.ToString());
                }
                finally {
                    if (strConn.State == ConnectionState.Open) {
                        strConn.Close();
                    }
                }
            }
            return Json(sResult);
        }
        // Отправляем письма на почту
        public JsonResult SendMessage(GetRequestModel request) {
            SmtpClient client = new SmtpClient("smtp.mail.ru");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("skyexx@mail.ru", "13467982dd");
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("skyexx@mail.ru");
            mailMessage.To.Add("skyexx@mail.ru");
            mailMessage.Body = 
                "Имя: " + request.sUserName + "\n" +
                "E-mail или телефон: " + request.sEmailOrNumber + "\n" +
                "Коротко о проекте: " + request.sMultiTextRequest;
            mailMessage.Subject = "Новая заявка";
            client.Send(mailMessage);
            return Json(request);
        }
    }
}