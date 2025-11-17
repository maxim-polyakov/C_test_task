using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace TestTask
{
    public class EmployeeService
    {
        private readonly EmployeeDbContext _context;

        public EmployeeService(EmployeeDbContext context)
        {
            _context = context;
        }

        public void InitializeDatabase()
        {
            try
            {
                // Создаем базу данных и таблицы, если они не существуют
                _context.Database.EnsureCreated();
                Console.WriteLine("✅ База данных и таблицы созданы успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка инициализации базы данных: {ex.Message}");
                throw;
            }
        }

        public void AddNewEmployee()
        {
            try
            {
                Console.WriteLine("\n=== Добавление нового сотрудника ===");
                
                var employee = new Employee();
                
                Console.Write("Введите имя: ");
                employee.FirstName = Console.ReadLine() ?? "";
                
                Console.Write("Введите фамилию: ");
                employee.LastName = Console.ReadLine() ?? "";
                
                Console.Write("Введите email: ");
                employee.Email = Console.ReadLine() ?? "";
                
                Console.Write("Введите дату рождения (гггг-мм-дд): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateOfBirth))
                {
                    Console.WriteLine("❌ Неверный формат даты!");
                    return;
                }
                employee.DateOfBirth = dateOfBirth;
                
                Console.Write("Введите зарплату: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal salary))
                {
                    Console.WriteLine("❌ Неверный формат зарплаты!");
                    return;
                }
                employee.Salary = salary;

                // Проверка валидации данных
                var validationContext = new ValidationContext(employee);
                var validationResults = new List<ValidationResult>();
                
                if (!Validator.TryValidateObject(employee, validationContext, validationResults, true))
                {
                    Console.WriteLine("❌ Ошибки валидации:");
                    foreach (var validationResult in validationResults)
                    {
                        Console.WriteLine($"  - {validationResult.ErrorMessage}");
                    }
                    return;
                }

                _context.Employees.Add(employee);
                _context.SaveChanges();
                
                Console.WriteLine($"✅ Сотрудник успешно добавлен с ID: {employee.EmployeeID}");
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Console.WriteLine($"❌ Ошибка при добавлении сотрудника: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        public void ViewAllEmployees()
        {
            try
            {
                Console.WriteLine("\n=== Список всех сотрудников ===");
                
                var employees = _context.Employees.OrderBy(e => e.EmployeeID).ToList();
                
                if (!employees.Any())
                {
                    Console.WriteLine("Сотрудники не найдены.");
                    return;
                }

                foreach (var employee in employees)
                {
                    Console.WriteLine(employee);
                }
                
                Console.WriteLine($"\nВсего сотрудников: {employees.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при получении списка сотрудников: {ex.Message}");
            }
        }

        public void UpdateEmployee()
        {
            try
            {
                Console.WriteLine("\n=== Обновление информации о сотруднике ===");
                
                Console.Write("Введите ID сотрудника для обновления: ");
                if (!int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    Console.WriteLine("❌ Неверный формат ID!");
                    return;
                }

                var existingEmployee = _context.Employees.Find(employeeId);
                if (existingEmployee == null)
                {
                    Console.WriteLine("❌ Сотрудник с указанным ID не найден!");
                    return;
                }

                Console.WriteLine($"Текущая информация: {existingEmployee}");
                Console.WriteLine("\nКакое поле вы хотите обновить?");
                Console.WriteLine("1. Имя");
                Console.WriteLine("2. Фамилия");
                Console.WriteLine("3. Email");
                Console.WriteLine("4. Дата рождения");
                Console.WriteLine("5. Зарплата");
                Console.WriteLine("6. Все поля");
                Console.Write("Выберите опцию: ");

                var choice = Console.ReadLine();
                var isModified = false;

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите новое имя: ");
                        existingEmployee.FirstName = Console.ReadLine() ?? "";
                        isModified = true;
                        break;
                    case "2":
                        Console.Write("Введите новую фамилию: ");
                        existingEmployee.LastName = Console.ReadLine() ?? "";
                        isModified = true;
                        break;
                    case "3":
                        Console.Write("Введите новый email: ");
                        existingEmployee.Email = Console.ReadLine() ?? "";
                        isModified = true;
                        break;
                    case "4":
                        Console.Write("Введите новую дату рождения (гггг-мм-дд): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newDob))
                        {
                            existingEmployee.DateOfBirth = newDob;
                            isModified = true;
                        }
                        else
                            Console.WriteLine("❌ Неверный формат даты!");
                        break;
                    case "5":
                        Console.Write("Введите новую зарплату: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal newSalary))
                        {
                            existingEmployee.Salary = newSalary;
                            isModified = true;
                        }
                        else
                            Console.WriteLine("❌ Неверный формат зарплаты!");
                        break;
                    case "6":
                        Console.Write("Введите имя: ");
                        existingEmployee.FirstName = Console.ReadLine() ?? "";
                        Console.Write("Введите фамилию: ");
                        existingEmployee.LastName = Console.ReadLine() ?? "";
                        Console.Write("Введите email: ");
                        existingEmployee.Email = Console.ReadLine() ?? "";
                        Console.Write("Введите дату рождения (гггг-мм-дд): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                            existingEmployee.DateOfBirth = dob;
                        else
                            Console.WriteLine("❌ Неверный формат даты!");
                        Console.Write("Введите зарплату: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal salary))
                            existingEmployee.Salary = salary;
                        else
                            Console.WriteLine("❌ Неверный формат зарплаты!");
                        isModified = true;
                        break;
                    default:
                        Console.WriteLine("❌ Неверный выбор!");
                        return;
                }

                if (isModified)
                {
                    // Проверка валидации обновленных данных
                    var validationContext = new ValidationContext(existingEmployee);
                    var validationResults = new List<ValidationResult>();
                    
                    if (!Validator.TryValidateObject(existingEmployee, validationContext, validationResults, true))
                    {
                        Console.WriteLine("❌ Ошибки валидации:");
                        foreach (var validationResult in validationResults)
                        {
                            Console.WriteLine($"  - {validationResult.ErrorMessage}");
                        }
                        return;
                    }

                    _context.SaveChanges();
                    Console.WriteLine("✅ Информация о сотруднике успешно обновлена!");
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Console.WriteLine($"❌ Ошибка при обновлении сотрудника: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        public void DeleteEmployee()
        {
            try
            {
                Console.WriteLine("\n=== Удаление сотрудника ===");
                
                Console.Write("Введите ID сотрудника для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int employeeId))
                {
                    Console.WriteLine("❌ Неверный формат ID!");
                    return;
                }

                var existingEmployee = _context.Employees.Find(employeeId);
                if (existingEmployee == null)
                {
                    Console.WriteLine("❌ Сотрудник с указанным ID не найден!");
                    return;
                }

                Console.WriteLine($"Вы действительно хотите удалить сотрудника: {existingEmployee}?");
                Console.Write("Введите 'да' для подтверждения: ");
                var confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "да")
                {
                    _context.Employees.Remove(existingEmployee);
                    _context.SaveChanges();
                    Console.WriteLine("✅ Сотрудник успешно удален!");
                }
                else
                {
                    Console.WriteLine("❌ Удаление отменено.");
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Console.WriteLine($"❌ Ошибка при удалении сотрудника: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }

        public void ShowEmployeesAboveAverageSalary()
        {
            try
            {
                Console.WriteLine("\n=== Сотрудники с зарплатой выше средней ===");
                
                var averageSalary = _context.Employees.Average(e => e.Salary);
                var count = _context.Employees.Count(e => e.Salary > averageSalary);
                var aboveAverageEmployees = _context.Employees
                    .Where(e => e.Salary > averageSalary)
                    .OrderByDescending(e => e.Salary)
                    .ToList();
                
                Console.WriteLine($"Средняя зарплата: {averageSalary:C}");
                Console.WriteLine($"Количество сотрудников с зарплатой выше средней: {count}");
                
                if (aboveAverageEmployees.Any())
                {
                    Console.WriteLine("\nСотрудники с зарплатой выше средней:");
                    foreach (var employee in aboveAverageEmployees)
                    {
                        Console.WriteLine($"  - {employee.FirstName} {employee.LastName}: {employee.Salary:C}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при получении статистики: {ex.Message}");
            }
        }
    }
}