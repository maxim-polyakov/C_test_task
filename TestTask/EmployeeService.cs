// EmployeeService.cs
using System;
using System.Collections.Generic;

namespace TestTask
{
    public class EmployeeService
    {
        private readonly DatabaseHelper _dbHelper;

        public EmployeeService(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void AddNewEmployee()
        {
            try
            {
                Console.WriteLine("\n=== Добавление нового сотрудника ===");
            
                var employee = new Employee();
            
                Console.Write("Введите имя: ");
                employee.FirstName = Console.ReadLine();
            
                Console.Write("Введите фамилию: ");
                employee.LastName = Console.ReadLine();
            
                Console.Write("Введите email: ");
                employee.Email = Console.ReadLine();
            
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

                var employeeId = _dbHelper.AddEmployee(employee);
                Console.WriteLine($"✅ Сотрудник успешно добавлен с ID: {employeeId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при добавлении сотрудника: {ex.Message}");
            }
        }

        public void ViewAllEmployees()
        {
            try
            {
                Console.WriteLine("\n=== Список всех сотрудников ===");
            
                var employees = _dbHelper.GetAllEmployees();
            
                if (employees.Count == 0)
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

                var existingEmployee = _dbHelper.GetEmployeeById(employeeId);
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
                var updatedEmployee = new Employee
                {
                    EmployeeID = existingEmployee.EmployeeID,
                    FirstName = existingEmployee.FirstName,
                    LastName = existingEmployee.LastName,
                    Email = existingEmployee.Email,
                    DateOfBirth = existingEmployee.DateOfBirth,
                    Salary = existingEmployee.Salary
                };

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите новое имя: ");
                        updatedEmployee.FirstName = Console.ReadLine();
                        break;
                    case "2":
                        Console.Write("Введите новую фамилию: ");
                        updatedEmployee.LastName = Console.ReadLine();
                        break;
                    case "3":
                        Console.Write("Введите новый email: ");
                        updatedEmployee.Email = Console.ReadLine();
                        break;
                    case "4":
                        Console.Write("Введите новую дату рождения (гггг-мм-дд): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime newDob))
                            updatedEmployee.DateOfBirth = newDob;
                        else
                            Console.WriteLine("❌ Неверный формат даты!");
                        break;
                    case "5":
                        Console.Write("Введите новую зарплату: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal newSalary))
                            updatedEmployee.Salary = newSalary;
                        else
                            Console.WriteLine("❌ Неверный формат зарплаты!");
                        break;
                    case "6":
                        Console.Write("Введите имя: ");
                        updatedEmployee.FirstName = Console.ReadLine();
                        Console.Write("Введите фамилию: ");
                        updatedEmployee.LastName = Console.ReadLine();
                        Console.Write("Введите email: ");
                        updatedEmployee.Email = Console.ReadLine();
                        Console.Write("Введите дату рождения (гггг-мм-дд): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                            updatedEmployee.DateOfBirth = dob;
                        else
                            Console.WriteLine("❌ Неверный формат даты!");
                        Console.Write("Введите зарплату: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal salary))
                            updatedEmployee.Salary = salary;
                        else
                            Console.WriteLine("❌ Неверный формат зарплаты!");
                        break;
                    default:
                        Console.WriteLine("❌ Неверный выбор!");
                        return;
                }

                if (_dbHelper.UpdateEmployee(updatedEmployee))
                    Console.WriteLine("✅ Информация о сотруднике успешно обновлена!");
                else
                    Console.WriteLine("❌ Ошибка при обновлении информации о сотруднике!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при обновлении сотрудника: {ex.Message}");
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

                var existingEmployee = _dbHelper.GetEmployeeById(employeeId);
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
                    if (_dbHelper.DeleteEmployee(employeeId))
                        Console.WriteLine("✅ Сотрудник успешно удален!");
                    else
                        Console.WriteLine("❌ Ошибка при удалении сотрудника!");
                }
                else
                {
                    Console.WriteLine("❌ Удаление отменено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при удалении сотрудника: {ex.Message}");
            }
        }

        public void ShowEmployeesAboveAverageSalary()
        {
            try
            {
                Console.WriteLine("\n=== Сотрудники с зарплатой выше средней ===");
            
                var count = _dbHelper.GetEmployeesCountAboveAverageSalary();
                Console.WriteLine($"Количество сотрудников с зарплатой выше средней: {count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при получении статистики: {ex.Message}");
            }
        }
    }
}
