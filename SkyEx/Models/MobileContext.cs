using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SkyEx.Models {
    // Класс контекста базы данных
    public class MobileContext : DbContext {
        public DbSet<FileModel> Files { get; set; }
        public MobileContext(DbContextOptions<MobileContext> options)
            : base(options) { }
    }
}
