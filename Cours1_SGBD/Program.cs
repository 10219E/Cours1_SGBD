// See https://aka.ms/new-console-template for more information

using ModelsDLL.Models;
using InterfacesDLL.Interfaces;
using RepositoryDLL.RepoSvc;
using ServicesDLL.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

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
        private static List<UI_Student> student;
        //private static List<UI_Student> ui_student;
        private static List<UI_StudioStudent> studio_student;

        //private static List<StudentsToInsert> insert_student;
        //private static List<StudentUpdate> fields_toupdate;
        // Keep variable names as requested: single instances, not lists
        private static StudentsToInsert? insert_student;
        private static StudentUpdate? fields_toupdate;

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
                Console.WriteLine("2 - View Studio Students");
                Console.WriteLine("3 - Find Student (FIND)");
                Console.WriteLine("4 - Insert New Student (INSERT)");
                Console.WriteLine("5 - Delete Student (DELETE)");
                Console.WriteLine("6 - Update Student (UPDATE)");
                Console.WriteLine("0 - Exit");

                Console.Write("Enter your choice: ");
                var input = Console.ReadLine();
                if (!new[] { "0", "1", "2", "3", "4", "5", "6" }.Contains(input))  //ARRAY WITH POSSIBLE CHOICES - SHORTER THAN OR, AND, ETC
                {
                    Console.WriteLine("Invalid choice. Please select the correct option.");
                    continue; // Skip the rest of the loop and prompt again
                }


                try
                {
                    using var formClosedSignal = new ManualResetEventSlim(false); // Signal to indicate when the form is closed

                    switch (input)
                    {
                        //EXIT
                        case "0":
                            choice_running = false;
                            break;

                        //FIND
                        case "3":
                            Console.Write("Enter search term (ID, First or Last name): ");
                            var search = Console.ReadLine() ?? string.Empty;
                            var foundStudents = studentService.FindStudentSvc(search);
                            if (foundStudents.Count == 0)
                            {
                                Console.WriteLine("No students found matching the search criteria.");
                                PauseAndClear();
                            }
                            else
                            {
                                logger.LogInformation($"Retrieved student from the database with search criteria: {search}.");
                                //ShowStudentsInGrid(foundStudents);
                                /*foreach (var stu in foundStudents)
                                {
                                    Console.WriteLine($"{Environment.NewLine}ID: {stu.id}{Environment.NewLine} First Name: {stu.fname}{Environment.NewLine} Last Name: {stu.lname}{Environment.NewLine} Section: {stu.section}");
                                    ShowStudentsInGrid(foundStudents);
                                }*/
                                //PauseAndClear();
                                ShowStudentsInGrid(foundStudents, formClosedSignal);
                                formClosedSignal.Wait();
                                Console.Clear();
                            }
                            break;

                        //SELECT ALL STUDENTS
                        case "1":

                            student = studentService.GetStudentsSvc();

                            logger.LogInformation("Retrieved all students from the database.");
                            //ShowStudentsInGrid(student);

                            ShowStudentsInGrid(student, formClosedSignal);
                            formClosedSignal.Wait();
                            Console.Clear();
                            //PauseAndClear();
                            break;

                        //REVIEW STUDIO STUDENTS
                        case "2":

                            studio_student = studentService.GetStudioSvc();

                            logger.LogInformation("Retrieved all studio information from the database.");
                            //ShowStudentsInGrid(student);

                            ShowStudioInGrid(studio_student, formClosedSignal);
                            formClosedSignal.Wait();
                            Console.Clear();
                            //PauseAndClear();
                            break;

                        //INSERT
                        case "4":
                            // assign to the existing variable name
                            insert_student = PromptForInsertStudent();

                            logger.LogInformation($"Inserting new student: {insert_student.fname} {insert_student.lname}");
                            studentService.InsertStudentSvc(insert_student);
                            PauseAndClear();
                            break;

                        //DELETE
                        case "5":
                            Console.Write("Enter the ID of the student to delete: ");
                            student_id = Console.ReadLine();
                            if (!int.TryParse(student_id, out var id_to_delete))
                            {
                                Console.WriteLine("Invalid ID. Delete cancelled.");
                                PauseAndClear();
                                break;
                            }

                            logger.LogInformation($"Deleting student with ID {id_to_delete}");
                            studentService.DeleteStudentSvc(id_to_delete);
                            PauseAndClear();
                            break;
  
                        //UPDATE
                        case "6":
                            Console.Write("Enter the ID of the student to update: ");
                            student_id = Console.ReadLine();
                            if (!int.TryParse(student_id, out var id_to_update))
                            {
                                Console.WriteLine("Invalid ID. Update cancelled.");
                                PauseAndClear();
                                break;
                            }

                            // assign to the existing variable name
                            fields_toupdate = PromptForUpdateStudent();

                            logger.LogInformation($"Updating student with ID {id_to_update}");

                            studentService.UpdateStudentSvc(id_to_update, fields_toupdate);
                            PauseAndClear();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during operation.");
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    PauseAndClear();
                }

            }///

            logger.LogInformation("Application Ending");


        }

        private static ServiceProvider ConfigureServices() //DEFINE INJECT LOGGER
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole())
                    .AddSingleton<ICoursSGBDRepo, RepoSvc>()
                    .AddSingleton<IStudentService, StudentService>()
                    .AddSingleton<IStudioRepo, StudioRepoSvc>();

            return services.BuildServiceProvider();
        }

        // Prompts for values required to insert a new student
        private static StudentsToInsert PromptForInsertStudent()
        {
            Console.Write("First name: ");
            var fname = Console.ReadLine() ?? string.Empty;
            Console.Write("Last name: ");
            var lname = Console.ReadLine() ?? string.Empty;
            Console.Write("E-mail: ");
            var email = Console.ReadLine() ?? string.Empty;
            Console.Write("Phone: ");
            var phone = Console.ReadLine() ?? string.Empty;
            Console.Write("Section: ");
            var section = Console.ReadLine() ?? string.Empty;

            return new StudentsToInsert
            {
                fname = fname.Trim(),
                lname = lname.Trim(),
                email = email.Trim(),
                phone = phone.Trim(),
                confirmed = DateTime.Today,
                section = section.Trim()
            };
        }

        // Prompts for optional update fields; empty inputs become null so existing values are kept
        private static StudentUpdate PromptForUpdateStudent()
        {
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

            return new StudentUpdate
            {
                fname = string.IsNullOrWhiteSpace(ufname) ? null : ufname.Trim(),
                lname = string.IsNullOrWhiteSpace(ulname) ? null : ulname.Trim(),
                email = string.IsNullOrWhiteSpace(uemail) ? null : uemail.Trim(),
                phone = string.IsNullOrWhiteSpace(uphone) ? null : uphone.Trim(),
                section = string.IsNullOrWhiteSpace(usection) ? null : usection.Trim()
            };
        }

        // Reusable pause + clear function
        private static void PauseAndClear()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.Clear();
        }

        static void ShowStudentsInGrid(List<UI_Student> students, ManualResetEventSlim? formClosedSignal = null)
        {
            var t = new Thread(() =>
            {
                // Initialize WinForms on this STA thread (replacement for ApplicationConfiguration.Initialize)
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var form = new StudentGridForm(students);
                if (formClosedSignal != null)
                {
                    form.FormClosed += (s, e) => formClosedSignal.Set();
                }

                Application.Run(form);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

        static void ShowStudioInGrid(List<UI_StudioStudent> studios, ManualResetEventSlim? formClosedSignal = null)
        {
            var t = new Thread(() =>
            {
                // Initialize WinForms on this STA thread (replacement for ApplicationConfiguration.Initialize)
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var form = new StudioGridForm(studios);
                if (formClosedSignal != null)
                {
                    form.FormClosed += (s, e) => formClosedSignal.Set();
                }

                Application.Run(form);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }
    }
}