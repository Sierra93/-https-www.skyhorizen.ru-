using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyEx.Models;
using System.Data.SqlClient;
using System.IO;

namespace SkyEx.Controllers {
    // В этом контроллере реализован функционал загрузки и выгрузке моих работ из БД на основании папки
    public class FileUploadController : Controller {
        string connectionString = "Server=u499383.mssql.masterhost.ru,1433;Initial Catalog=u499383_skyexdb;Persist Security Info=False;User ID=u499383;Password=inessen9hoo;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=true;Connection Timeout=30";
        public IActionResult Index() {
            // Выводим данные из БД в указанную вью
            var model = FetchImageFromDB();
            return View(model);
        }
        // Прописываем путь загружаемых файлов
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormCollection form, string user_title) {
            string storePath = "wwwroot/images/";   // Путь к папке с изображениями
            if (form.Files == null || form.Files[0].Length == 0)
                return RedirectToAction("Index");
            // Полный локальный путь к файлу включая папку проекта wwwroot
            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), storePath,
                        form.Files[0].FileName);
            using (var stream = new FileStream(path, FileMode.Create)) {
                await form.Files[0].CopyToAsync(stream);
            }
            StoreInDB(storePath + form.Files[0].FileName, user_title);
            return RedirectToAction("Index");
        }
        // Записываем данные в БД
        public void StoreInDB(string path, string user_title) {
            using (var con = new SqlConnection(connectionString)) {
                con.Open();
                // Записываем изображения в БД
                using (var com = new SqlCommand("INSERT INTO AllWorksInMyPortfolio(TITLE, IMAGE_PATH) VALUES('" + user_title + "', '" + path + "')", con)) {
                    try {
                        com.ExecuteNonQuery();
                    }
                    catch (Exception ex) {
                        throw new Exception(ex.Message.ToString());
                    }
                }
            }
        }
        // Отображаем данные из БД
        public List<FileModel> FetchImageFromDB() {
            List<FileModel> imagePath = new List<FileModel>();  // Создаем коллекцию на основе полей модели
            using (var con = new SqlConnection(connectionString)) {
                con.Open();
                using (var com = new SqlCommand("SELECT TITLE, IMAGE_PATH FROM AllWorksInMyPortfolio", con)) {
                    using (var reader = com.ExecuteReader()) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                imagePath.Add(new FileModel {
                                    Title = reader["TITLE"].ToString(),
                                    ImagePath = reader["IMAGE_PATH"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return imagePath;
        }
    }
}