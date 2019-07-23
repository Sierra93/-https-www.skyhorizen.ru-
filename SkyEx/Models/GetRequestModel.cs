using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// Пространства для работы с почтой
using MimeKit;
using MailKit.Net.Smtp;

namespace SkyEx.Models {
    // Модель заявок
    public class GetRequestModel {
        public int ID { get; set; }
        public string sUserName { get; set; }
        public string sEmailOrNumber { get; set; }
        public string sMultiTextRequest { get; set; }
    }  
}
