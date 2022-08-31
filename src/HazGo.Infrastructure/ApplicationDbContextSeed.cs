using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HazGo.Infrastructure
{
    public static class ApplicationDbContextSeed
    {
        public static void SeedPostMigrationDataAsync(ApplicationDbContext context)
        {
            string postMigrationScriptsPath = Directory.GetParent(Directory.GetCurrentDirectory()) + @"\HazGo.Infrastructure\Scripts\PostMigrationScripts";

            try
            {
                foreach (string file in Directory.EnumerateFiles(postMigrationScriptsPath, "*.sql"))
                {
                    string contents = File.ReadAllText(file);
                    context.Database.ExecuteSqlRaw(contents, new List<object>());
                    context.SaveChanges();
                }
            }
            catch
            {

            }
        }
    }
}
