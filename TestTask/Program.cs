using System;

namespace TestTask
{
    class Program
    {
        static void Main()
        {
            // Создаем контекст базы данных
            using var context = new EmployeeDbContext();
            var employeeService = new EmployeeService(context);

            try
            {
                // Инициализируем базу данных (создаем таблицы если их нет)
                employeeService.InitializeDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Критическая ошибка инициализации базы данных: {ex.Message}");
                Console.WriteLine("Проверьте строку подключения и настройки PostgreSQL.");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
                return;
            }

            ShowMenu(employeeService);
        }

        static void ShowMenu(EmployeeService employeeService)
        {
            while (true)
            {
                Console.WriteLine("\n=== Система управления сотрудниками ===");
                Console.WriteLine("1. Добавить нового сотрудника");
                Console.WriteLine("2. Посмотреть всех сотрудников");
                Console.WriteLine("3. Обновить информацию о сотруднике");
                Console.WriteLine("4. Удалить сотрудника");
                Console.WriteLine("5. Сотрудники с зарплатой выше средней");
                Console.WriteLine("6. Выйти из приложения");
                Console.Write("Выберите опцию: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        employeeService.AddNewEmployee();
                        break;
                    case "2":
                        employeeService.ViewAllEmployees();
                        break;
                    case "3":
                        employeeService.UpdateEmployee();
                        break;
                    case "4":
                        employeeService.DeleteEmployee();
                        break;
                    case "5":
                        employeeService.ShowEmployeesAboveAverageSalary();
                        break;
                    case "6":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("❌ Неверный выбор! Пожалуйста, выберите опцию от 1 до 6.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}