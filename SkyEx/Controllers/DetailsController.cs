using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyEx.Models;

namespace SkyEx.Controllers {
    // Получаем подробности выбранной работы
    public class DetailsController : Controller {
        string connectionString = "Server=skyhorizen.ru,1433; Initial Catalog=u0772479_skydb; Persist Security Info=False; User ID=u0772479_admin; Password=K3sxb30*; MultipleActiveResultSets=False; Encrypt=True; TrustServerCertificate=true; Connection Timeout=30";
        public IActionResult GetDetails() {
            return View("GetDetails");
        }
        // Будем искать в БД нужный проект
        public List<FileModel> SearchInDB(FileModel model) {
            string sTitle = model.Title;    // Получаем title с фронта
            List<FileModel> imagePath = new List<FileModel>();  // Создаем коллекцию на основе полей модели
            using (var con = new SqlConnection(connectionString)) {
                con.Open();
                using (var com = new SqlCommand("SELECT * FROM Portfolio WHERE COMMENT_DETAILS LIKE " + "'%" + sTitle + "%'", con)) {
                    using (var reader = com.ExecuteReader()) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                imagePath.Add(new FileModel {
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
    }
}