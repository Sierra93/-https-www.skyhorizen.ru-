using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyEx.Models;
using System.Threading;

namespace SkyEx.Controllers {
    // Получаем подробности выбранной работы
    public class DetailsController : Controller {
        string connectionString = "Server=skyhorizen.ru,1433; Initial Catalog=u0772479_skydb; Persist Security Info=False; User ID=u0772479_admin; Password=K3sxb30*; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=true; Connection Timeout=30";
        // Переходим на страницу подробностей      
        //[HttpGet]
        public IActionResult GetDetails(int id) {
            var data = SearchInDB(id);
            return View(data);
        }
        // Будем искать в БД нужный проект  
        [HttpPost]
        public List<FileModel> SearchInDB(int id) { 
            List<FileModel> imagePath = new List<FileModel>();
            using (var con = new SqlConnection(connectionString)) {
                con.Open();
                using (var com = new SqlCommand("SELECT p1.ID, p1.TITLE AS TITLE_DETAILS, p1.DETAILS_TASK, p1.IMAGE_PATH " +
                    "FROM PortfolioDetails as p1 " +
                    "JOIN Portfolio as p2 ON p1.ID_GROUP = p2.ID_GROUP " +
                    "WHERE p2.ID_GROUP = " + id, con)) {
                    using (var reader = com.ExecuteReader()) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                imagePath.Add(new FileModel {
                                    ID = Convert.ToInt32(reader["ID"]),
                                    Title = reader["TITLE_DETAILS"].ToString(),
                                    ImagePath = reader["IMAGE_PATH"].ToString(),
                                    CommentDetails = reader["DETAILS_TASK"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return imagePath;
        }
        // Возвращаемся на главную
        public IActionResult Index() {
            return RedirectToAction("Index");
        }
    }
}