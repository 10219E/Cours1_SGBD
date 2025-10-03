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
        private static List<StudentsToInsert> insert_student;

        public static void Main(string[] args)
        {
            using var serviceProvider = ConfigureServices(); //DEFINE SERVICE FOR LOGGER
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>(); //CREATE LOGGER


            var studentService = serviceProvider.GetRequiredService<IStudentService>(); //CALL LOGGER INJECT

            logger.LogInformation("Application Starting");

            //SELECT
            student = studentService.GetStudentsSvc();


            foreach (var student in student)
            {
                Console.WriteLine($"ID: {student.id}, First Name: {student.fname}, Last Name: {student.lname}, Section: {student.section}");
            }

            //INSERT
            var insert_student = new StudentsToInsert
            {
                fname = "Nana",
                lname = "Mocha",
                email = "nmocha@ep.com",
                phone = "+32002921113",
                confirmed = DateTime.Today,
                section = "IT1"
            };

            studentService.InsertStudentSvc(insert_student);

            //DELETE
            studentService.DeleteStudentSvc(8);


            logger.LogInformation("Application Ending");

        }

        private static ServiceProvider ConfigureServices() //DEFINE INJECT LOGGER
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole())
                    .AddSingleton<ICoursSGBDRepo, CoursSGBDRepo>()
                    .AddSingleton<IStudentService, StudentService>();

            return services.BuildServiceProvider();
        }
    }

}