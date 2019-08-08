using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEx.Models {
    // Модель моих работ
    public class FileModel {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; } 
        public string FullName { get; set; }
        public string LinkSite { get; set; }
    }
}
