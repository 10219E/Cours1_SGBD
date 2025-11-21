using Castle.Core.Logging;
using InterfacesDLL.Interfaces;
using ModelsDLL.Models;
using ServicesDLL.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace xTEST_Cours1_SGBD.Services
{
    public class x_Student_Service
    {
        Mock<ICoursSGBDRepo> _mockService; //on réutilise le Repo de notre projet
        IStudentService _service;
        private List<UI_Student> mock_students;
        private List<StudentsToInsert> mock_insert = new List<StudentsToInsert>();


        public x_Student_Service()
        {
            _mockService = new Mock<ICoursSGBDRepo>();

            //Définition Etudiants Mockés --définis plus haut (Pas utilisé pour le INSERT car on reset la liste)
            mock_students = new List<UI_Student>()
            {
                new UI_Student()
                {
                    id=001,
                    fname="John",
                    lname="Doe",
                    email="jdoe@mail.com",
                    phone="514-123-4567",
                    confirmed=DateTime.Now,
                    section="A"
                    },
                new UI_Student()
                {
                    id=002,
                    fname="Jane",
                    lname="Doe",
                    email="jdoe@mail.com",
                    phone="514-123-41117",
                    confirmed=DateTime.Now,
                    section="B"
                }

            };

        }

        [Fact]
        public void x_GetStudentsSvc()
        {
            //Arrange
            _mockService.Setup(repo => repo.GetStudentsDb()).Returns(mock_students);
            _service = new StudentService(_mockService.Object);

            //Act
            var result = _service.GetStudentsSvc();

            //Assert
            Assert.Equal(mock_students.Count, result.Count);
            Assert.Equal(mock_students[0].fname, result[0].fname);
            Assert.Equal(mock_students[1].section, result[1].section);

            //Verify
            _mockService.Verify(repo => repo.GetStudentsDb(), Times.Once);

        }

        [Fact]
        public void x_FindStudentDb_diff_searches()
        {
            // This test performs three searches in one block:
            // Arrange: setup FindStudentDb to behave like a simple search over mock_students

            _mockService.Setup(r => r.FindStudentDb("002")) //Testing search by ID
                .Returns(mock_students.Where(s => s.id == 2).ToList());


            _mockService.Setup(r => r.FindStudentDb("Doe")) //Testing search by last name (expected list)
                .Returns(mock_students.ToList());


            _mockService.Setup(r => r.FindStudentDb("Jane")) //Testing search by first name (expected single result)
                .Returns(mock_students.Where(s => s.fname == "Jane").ToList());

            _service = new StudentService(_mockService.Object); //Initialize service with mocked repo

            // Act & Assert 1: Search by id string with leading zeros
            var resultById = _service.FindStudentSvc("002");

            Assert.NotNull(resultById); //Expecting Not null
            Assert.Single(resultById); //Expecting single result
            Assert.Equal(2, resultById[0].id); //Expecting ID 002 (ignoring 00)
            Assert.Equal("Jane", resultById[0].fname); //Expecting Jane

            _mockService.Verify(repo => repo.FindStudentDb("002"), Times.Once);

            // Act & Assert 2: search by last name -> expecting both students as same last name
            var resultByLast = _service.FindStudentSvc("Doe");

            Assert.NotNull(resultByLast); //Expecting Not null
            Assert.Equal("Doe", resultByLast[0].lname); //Expecting last name Doe for first result
            Assert.Equal("Doe", resultByLast[1].lname); //Expecting last name Doe for second result
            Assert.Equal(2, resultByLast.Count); //Expecting 2 results

            _mockService.Verify(repo => repo.FindStudentDb("Doe"), Times.Once); //Verify that the repo method was called once with "Doe"

            // Act & Assert 3: search by first name -> single student Jane
            var resultByFirst = _service.FindStudentSvc("Jane");

            Assert.NotNull(resultByFirst); //Expecting Not null
            Assert.Single(resultByFirst); //Expecting single result
            Assert.Equal("Jane", resultByFirst[0].fname); //Expecting first name Jane

            _mockService.Verify(repo => repo.FindStudentDb("Jane"), Times.Once); //Verify that the repo method was called once with "Jane"
        }



        [Fact]
        public void x_UpdateStudentSvc()
        {
            //Arrange
            var id_toupdate = 002;
            var updatedStudent = new StudentUpdate
            {
                lname = "Mark"
            };

            //Output to console for verification
            Console.WriteLine($"Initial Student: {mock_students[1].fname} {mock_students[1].lname} {mock_students[1].id}");

            //Assert Before
            Assert.Equal("Doe", mock_students.Single(s => s.id == 002).lname); ; //Before update, last name should be Doe for student with id 002

            _mockService.Setup(repo => repo.UpdateStudentDb(id_toupdate, updatedStudent));

            _service = new StudentService(_mockService.Object);

            //Act
            _service.UpdateStudentSvc(id_toupdate, updatedStudent);

            
            //Since the mock does not actually change the data, we manually update the mock list:
            mock_students.SingleOrDefault(x => x.id == id_toupdate).lname = updatedStudent.lname;

            //Output to console for verification
            Console.WriteLine($"Updated Student: {mock_students[1].fname} {mock_students[1].lname} {mock_students[1].id}");

            //Assert
            Assert.Equal(002, mock_students[1].id);//Check that the ID remains the same
            Assert.Equal("Mark", mock_students.Single(s => s.id == 002).lname); //Check that the updated last name is Mark

            //Verify
            _mockService.Verify(repo => repo.UpdateStudentDb(
                It.Is<int>(i => i == id_toupdate),
                It.Is<StudentUpdate>(u => u.lname == updatedStudent.lname)
            ), Times.Once);
        }


        [Theory]
        [InlineData("Alice", "Smith","asmith@recov.org","MB3", "+22918")]
        [InlineData("Nikiya", "De Kat","ndk@cat.meow","IT2", "")]
        public void x_InsertStudentSvc(string fname, string lname, string email, string section, string phone)
        {

            //Clear list mock_insert before each test run
            mock_insert.Clear();

            //Arrange Model for InsertStudentDb()
            var insert = new StudentsToInsert //Based on Model (in project) StudentsToInsert
            {
                fname = fname.Trim(),
                lname = lname.Trim(),
                email = email.Trim(),
                section = section.Trim(),
                phone = phone.Trim(),
                confirmed = DateTime.Now
            };

            //Assert Before - that list is empty before test
            Assert.Empty(mock_insert);

            //Insert in list


            //Simulate InsertStudentDb to add to mock_insert list when called
            _mockService.Setup(repo => repo.InsertStudentDb(It.IsAny<StudentsToInsert>()))
                .Callback<StudentsToInsert>(s => mock_insert.Add(s))
                .Verifiable(); //Mark as verifiable to check later

            Console.WriteLine($"Mock Insert count : {mock_insert.Count}"); //prints to output - Tests

            _service = new StudentService(_mockService.Object);

            //Act
            _service.InsertStudentSvc(insert);

            //Assert After
            Assert.Single(mock_insert); //We expect one student in the list after insertion
            Assert.Equal(fname, mock_insert[0].fname); //Check first name

            //Verify
            _mockService.Verify(repo => repo.InsertStudentDb(
                It.Is<StudentsToInsert>(
                    s => s.fname == fname &&
                    s.lname == lname &&
                    s.email == email &&
                    s.section == section &&
                    s.phone == phone)
                ), Times.Once); //Verify that InsertStudentDb was called once with the correct parameter

        }


        [Theory]
        [InlineData(003, "Cole", "Janett")]
        [InlineData(006, "Robert", "Nobel")]
        public void x_DeleteStudentSvc(int id, string fname, string lname)
        {
            //Arrange
            var id_todelete = 003;

            mock_students.Add(new UI_Student { id = id, fname = fname, lname = lname});

            _mockService.Setup(repo => repo.GetStudentsDb()).Returns(mock_students);
            _mockService.Setup(repo => repo.DeleteStudentDb(id))
                    .Callback<int>(deletedId => mock_students.RemoveAll(s => s.id == deletedId))
                    .Verifiable(); //simulate deletion in the mock list

            _service = new StudentService(_mockService.Object);


            //Assert Before
            var before = _service.GetStudentsSvc();
            var beforeCount = before.Count - 1;

            Console.WriteLine($"Student count: {mock_students.Count}"); //prints to output - Tests
            Assert.True(before.Any(s => s.id == id), "Student to delete must be present before DeleteStudentSvc");

            //Act
            _service.DeleteStudentSvc(id); //no result to capture as it's void

            //Verify
            _mockService.Verify(repo => repo.DeleteStudentDb(id));

            //Assert After
            var after = _service.GetStudentsSvc();
            Assert.False(after.Any(s => s.id == id), "Deleted student should no longer be present");
            Assert.Equal(beforeCount, after.Count);

        }

    }
}
