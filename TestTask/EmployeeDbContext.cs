using Microsoft.EntityFrameworkCore;

namespace TestTask
{
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Строка подключения к PostgreSQL
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=EmployeeDB;Username=postgres;Password=password");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка таблицы Employees
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                
                entity.HasKey(e => e.EmployeeID);
                
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.DateOfBirth)
                    .IsRequired()
                    .HasColumnType("date");
                
                entity.Property(e => e.Salary)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)");

                // Уникальный индекс для email
                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });

            // Опционально: добавим начальные данные для тестирования
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeID = 1,
                    FirstName = "Иван",
                    LastName = "Петров",
                    Email = "ivan.petrov@company.com",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    Salary = 50000
                },
                new Employee
                {
                    EmployeeID = 2,
                    FirstName = "Мария",
                    LastName = "Сидорова",
                    Email = "maria.sidorova@company.com",
                    DateOfBirth = new DateTime(1990, 8, 22),
                    Salary = 60000
                }
            );
        }
    }
}