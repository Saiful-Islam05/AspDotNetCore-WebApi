using Microsoft.EntityFrameworkCore;
using StudentAPI.Models;
using System.Security.Principal;

namespace StudentAPI.Data
{
        // AppDbContext = Database এর সাথে connection এর bridge
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
                // base(options) মানে parent class DbContext কে options দিচ্ছি
            }
            // ✅ DbSet = Database এর Table
            // "Students" table এর সাথে কথা বলার জন্য
            public DbSet<Student> Students { get; set; }

            public DbSet<Course> Courses { get; set; }

            public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // ✅ Seed Data — App চালু হলে এই data automatically যাবে DB তে
                modelBuilder.Entity<Student>().HasData(
                    new Student
                    {
                        Id = 1,
                        Name = "Rahim",
                        Age = 20,
                        City = "Dhaka",
                        Password = "pass123",
                        BankAccount = "BD111"
                    },
                    new Student
                    {
                        Id = 2,
                        Name = "Karim",
                        Age = 22,
                        City = "Chittagong",
                        Password = "pass456",
                        BankAccount = "BD222"
                    },
                    new Student
                    {
                        Id = 3,
                        Name = "Jamal",
                        Age = 21,
                        City = "Khulna",
                        Password = "pass789",
                        BankAccount = "BD333"
                    });

            // Seed Data for Courses
            modelBuilder.Entity<Course>().HasData(

                new Course
                {
                    Id = 1,
                    Title = "C# Programming",
                    Description = "This is a C# programming course."
                },
                new Course
                {
                    Id = 2,
                    Title = "Asp Dot net Web API",
                    Description = "This is an ASP.NET Web API course."
                },
                new Course
                {
                    Id = 3,
                    Title = "SQL Server",
                    Description = "This is a SQL Server course."
                }
                );
        }
    }
}
