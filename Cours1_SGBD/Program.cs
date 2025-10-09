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
        private static List<StudentUpdate> fields_toupdate;

        public static void Main(string[] args)
        {
            using var serviceProvider = ConfigureServices(); //DEFINE SERVICE FOR LOGGER
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>(); //CREATE LOGGER


            var studentService = serviceProvider.GetRequiredService<IStudentService>(); //CALL LOGGER INJECT

            logger.LogInformation("Application Starting");
            Thread.Sleep(50); //LET LOGGER PRINT BEFORE OTHER THINGS


            //VAR DECLARATION
            var student_id = "";

            //USER CHOICE
            bool choice_running = true;


            while (choice_running)
            {

                Console.WriteLine("Choose an operation:");
                Console.WriteLine("1 - View All Students (SELECT)");
                Console.WriteLine("2 - Insert New Student (INSERT)");
                Console.WriteLine("3 - Delete Student (DELETE)");
                Console.WriteLine("4 - Update Student (UPDATE)");
                Console.WriteLine("0 - Exit");

                Console.Write("Enter your choice: ");
                var input = Console.ReadLine();
                if (!new[] { "0", "1", "2", "3", "4" }.Contains(input))  //ARRAY WITH POSSIBLE CHOICES - SHORTER THAN OR, AND, ETC
                {
                    Console.WriteLine("Invalid choice. Please select the correct option.");
                    continue; // Skip the rest of the loop and prompt again
                }


                try
                {

                    switch (input)
                    {
                        //EXIT
                        case "0":
                            choice_running = false;
                            break;

                        //SELECT
                        case "1":

                            student = studentService.GetStudentsSvc();

                            logger.LogInformation("Retrieved all students from the database.");
                            foreach (var stu in student)
                            {
                                Console.WriteLine($"ID: {stu.id}, First Name: {stu.fname}, Last Name: {stu.lname}, Section: {stu.section}");
                            }
                            break;

                        //INSERT
                        case "2":
                            var insert_student = new StudentsToInsert
                            {
                                fname = "Roma",
                                lname = "Crop",
                                email = "rcrop@ep.com",
                                phone = "+320011222113",
                                confirmed = DateTime.Today,
                                section = "MB2"
                            };

                            logger.LogInformation($"Inserting new student: {insert_student.fname} {insert_student.lname}");
                            studentService.InsertStudentSvc(insert_student);
                            break;

                        //DELETE
                        case "3":
                            Console.Write("Enter the ID of the student to delete: ");
                            student_id = Console.ReadLine();
                            if (!int.TryParse(student_id, out var id_to_delete))
                            {
                                Console.WriteLine("Invalid ID. Delete cancelled.");
                                break;
                            }

                            logger.LogInformation($"Deleting student with ID {id_to_delete}");
                            studentService.DeleteStudentSvc(id_to_delete);
                            break;
  


                        //UPDATE
                        case "4":
                            Console.Write("Enter the ID of the student to update: ");
                            student_id = Console.ReadLine();
                            if (!int.TryParse(student_id, out var id_to_update))
                            {
                                Console.WriteLine("Invalid ID. Update cancelled.");
                                break;
                            }

                            Console.WriteLine("Leave a field empty to keep its current value.");
                            Console.Write("First name: ");
                            var ufname = Console.ReadLine();
                            Console.Write("Last name: ");
                            var ulname = Console.ReadLine();
                            Console.Write("E-mail: ");
                            var uemail = Console.ReadLine();
                            Console.Write("Phone: ");
                            var uphone = Console.ReadLine();
                            Console.Write("Section: ");
                            var usection = Console.ReadLine();

                            var fields_toupdate = new StudentUpdate
                            {
                                fname = string.IsNullOrWhiteSpace(ufname) ? null : ufname.Trim(),
                                lname = string.IsNullOrWhiteSpace(ulname) ? null : ulname.Trim(),
                                email = string.IsNullOrWhiteSpace(uemail) ? null : uemail.Trim(),
                                phone = string.IsNullOrWhiteSpace(uphone) ? null : uphone.Trim(),
                                section = string.IsNullOrWhiteSpace(usection) ? null : usection.Trim()
                            };

                            logger.LogInformation($"Updating student with ID {id_to_update}");

                            studentService.UpdateStudentSvc(id_to_update, fields_toupdate);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during operation.");
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

            }///

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