using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ConstructEd.Data
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            // Set base path to the MVC project where appsettings.json is located
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ConstructEd"); // Adjust project name if needed

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath) // Ensure it looks inside the MVC project
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("CS"));

            return new DataContext(optionsBuilder.Options);
        }

    }
}
