using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEx.Models {
    // Этот класс будет сопоставляться с таблицей портфолио в БД
    public class FileModel {
        public int ID { get; set; }   
        public string Title { get; set; }
        public string ImagePath { get; set; } 
        // Задача
        public string CommentTask { get; set; }
        // Описание задачи
        public string CommentDetails { get; set; }
    }
}
