using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEx.Models {
    // Модель данных, которую связываем с таблицей в БД
    public class FileModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; } 
    }
}
