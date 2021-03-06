﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyEx.Models;
using System.Data.SqlClient;
using System.IO;

namespace SkyEx.Controllers {
    // В этом контроллере реализован функционал по загрузке и выгрузке моих работ из БД и хранении изображений в папке
    public class FileUploadController : Controller {
        string connectionString = "Server=skyhorizen.ru,1433; Initial Catalog=u0772479_skydb; Persist Security Info=False; User ID=u0772479_admin; Password=K3sxb30*; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=true; Connection Timeout=30";
        public IActionResult Index() {
            // Выводим данные из БД
            var model = FetchImageFromDB();
            return View(model);
        }
        // Прописываем путь загружаемых файлов
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormCollection form, string user_title, string full_name, string link_site) {
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
            StoreInDB(storePath + form.Files[0].FileName, user_title, full_name, link_site);
            return RedirectToAction("Index");
        }
        // Записываем данные в БД
        public void StoreInDB(string path, string user_title, string full_name, string link_site) {
            using (var con = new SqlConnection(connectionString)) {
                con.Open();
                // Записываем изображения в БД
                using (var com = new SqlCommand("INSERT INTO Portfolio(TITLE, IMAGE_PATH, FULL_NAME, LINK_SITE) VALUES" +
                    "('" + user_title + "', '" + full_name + "', '" + link_site + "', '" + path + "')", con)) {
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
                using (var com = new SqlCommand("SELECT ID, TITLE, IMAGE_PATH, COMMENT_TASK, COMMENT_DETAILS FROM Portfolio", con)) {
                    using (var reader = com.ExecuteReader()) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                imagePath.Add(new FileModel {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Title = reader["TITLE"].ToString(),
                                    ImagePath = reader["IMAGE_PATH"].ToString(),
                                    CommentTask = reader["COMMENT_TASK"].ToString(),
                                    CommentDetails = reader["COMMENT_DETAILS"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return imagePath;
        }

        /// <summary>
        /// Метод выбирает проекты определенной категории.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetCategoryProject(string category) {
            var data = await GetCategoryProjects(category);
            return View(data);
        }

        /// <summary>
        /// Метод находит проекты выбранной категории.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<FileModel>> GetCategoryProjects(string category) {
            List<FileModel> imagePath = new List<FileModel>();  // Создаем коллекцию на основе полей модели
            using (var con = new SqlConnection(connectionString)) {
                con.Open();
                using (var com = new SqlCommand("SELECT ID, TITLE, IMAGE_PATH, COMMENT_TASK, COMMENT_DETAILS, CATEGORY_PROJECT " +
                    "FROM Portfolio " +
                    "WHERE CATEGORY_PROJECT = " + "\'" + category + "\'", con)) {
                    using (var reader = com.ExecuteReader()) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                imagePath.Add(new FileModel {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Title = reader["TITLE"].ToString(),
                                    ImagePath = reader["IMAGE_PATH"].ToString(),
                                    CommentTask = reader["COMMENT_TASK"].ToString(),
                                    CommentDetails = reader["COMMENT_DETAILS"].ToString(),
                                    Category = reader["CATEGORY_PROJECT"].ToString()
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