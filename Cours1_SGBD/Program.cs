// See https://aka.ms/new-console-template for more information
using Cours1_SGBD.Interfaces;
using Cours1_SGBD.Models;
using Cours1_SGBD.Repositories;
using Cours1_SGBD.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

/*StudentService service = new StudentService();

List<Student> students = service.GetStudents();

foreach (var student in students)
{
    Console.WriteLine($"ID: {student.id}, First Name: {student.fname}, Last Name: {student.lname}, Section: {student.section}");
}

Console.WriteLine("Hello, World!");*/

namespace Cours1_SGBD
{
    class Program
    {
        private static List<Student> student;

        public static void Main(string[] args)
        {
            using var serviceProvider = ConfigureServices();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            //var loggerF = serviceProvider.GetService<ILoggerFactory>();
            //var logger = loggerF.CreateLogger("My SGBD Program");

            var studentService = serviceProvider.GetRequiredService<IStudentService>();
            //_repo = new CoursSGBDRepo(LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("SQL_Exception"));

            logger.LogInformation("Application Starting");
            student = studentService.GetStudentsSvc();

            foreach (var student in student)
            {
                Console.WriteLine($"ID: {student.id}, First Name: {student.fname}, Last Name: {student.lname}, Section: {student.section}");
            }


            logger.LogInformation("Application Ending");

        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole())
                    .AddSingleton<ICoursSGBDRepo, CoursSGBDRepo>()
                    .AddSingleton<IStudentService, StudentService>();

            return services.BuildServiceProvider();
        }
    }

}
