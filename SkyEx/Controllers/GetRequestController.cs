﻿using System;
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
//using System.Net.Mail;
// Пространства для работы с почтой
using MimeKit;
using MailKit.Net.Smtp;

namespace SkyEx.Controllers {
    // Сохранение новой заявки в БД и отправление на почту
    public class GetRequestController : Controller {
        public string sResult = "";     // Флаг ошибки
        [HttpPost]
        // GetRequestModel - Модель данных, с которой здесь работаем
        // Метод для работы с заявками и сохраняем в БД
        public async Task<JsonResult> SaveRequest(GetRequestModel request) {
            string connectionString = "Server=skyhorizen.ru,1433; Initial Catalog=u0772479_skydb; Persist Security Info=False; User ID=u0772479_admin; Password=K3sxb30*; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=true; Connection Timeout=30";
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
                        using (var con = new SqlConnection(connectionString)) {
                            con.Open();
                            // Сохранение заявки в БД
                            using (var com = new SqlCommand("INSERT INTO REQUEST(CLIENT_NAME, EMAIL_OR_NUMBER, COMMENT_REQUEST) VALUES('" + sClientName + "', '" + sEmailOrNumber + "', '" + sCommentResuest + "')", con)) {
                                com.ExecuteNonQuery();
                                sResult = "OK";
                                await SendEmailAsync(request);
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    sResult = ex.Message.ToString();
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
        public async Task SendEmailAsync(GetRequestModel request) {
            try {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("sierra_93@mail.ru"));
                emailMessage.To.Add(new MailboxAddress("sierra_93@mail.ru"));
                emailMessage.Subject = "Новая заявка";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {
                    Text = "Имя: " + request.sUserName + "</br>" +
                "E-mail или телефон: " + request.sEmailOrNumber + "</br>" +
                "Коротко о проекте: " + request.sMultiTextRequest
                };
                using (var client = new SmtpClient()) {
                    await client.ConnectAsync("smtp.mail.ru", 2525, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("sierra_93@mail.ru", "13467kvm");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex) {
                new Exception(ex.Message.ToString());
            }
        }
    }
}