using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SkyEx.Models {
    // Загрузка моих работ в папку
    public class FileManager {
        // В этом классе реализована загрузка файла в папку
        public async Task<bool> UploadFile(IFormFile file) {
            try {
                bool isCopied = false;
                if (file.Length > 0) {
                    string fileName = file.FileName;
                    string extension = Path.GetExtension(fileName);
                    // Задаем расширение файлов
                    if (extension == ".png" || extension == ".jpg") {
                        // Записываем путь к папке, в которую будем загружать файлы
                        string filePath = Path.GetFullPath(
                            Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
                        using (var fileStream = new FileStream(
                            Path.Combine(filePath, fileName),
                                           FileMode.Create)) {
                            await file.CopyToAsync(fileStream);
                            isCopied = true;
                        }
                    }
                    else {
                        throw new Exception("Файл должен быть формата .png или .JPG");
                    }
                }
                return isCopied;
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
