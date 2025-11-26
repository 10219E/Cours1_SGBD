using InterfacesDLL.Interfaces;
using ModelsDLL.Models;
using ServicesDLL.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace xTEST_Cours1_SGBD.Services
{
    public class x_Err_Student_Service
    {
        Mock<ICoursSGBDRepo> _mockService; //on réutilise le Repo de notre projet
        Mock<IStudioRepo> _mockStudio; //mock pour le studio si besoin
        IStudentService _service;


        public x_Err_Student_Service()
        {
            _mockService = new Mock<ICoursSGBDRepo>();
            _mockStudio = new Mock<IStudioRepo>();
        }

        [Fact]
        public void x_Err_DeleteStudentSvc_IdNotFound_Throws()
        {
            // Arrange
            var id_notfound = 999; // inexistant

            // Repo lève une exception quand l'ID n'existe pas
            _mockService.Setup(repo => repo.DeleteStudentDb(id_notfound))
                        .Throws(new KeyNotFoundException("Student not found"));

            _service = new StudentService(_mockService.Object, _mockStudio.Object);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _service.DeleteStudentSvc(id_notfound));

            // Verify
            _mockService.Verify(repo => repo.DeleteStudentDb(id_notfound), Times.Once);
        }

        [Fact]
        public void x_Err_FindStudentDb_diff_searches_NotFound_Throws()
        {
            // Arrange: on simule des recherches introuvables
            _mockService.Setup(r => r.FindStudentDb("999")) // id n'existe pas
                        .Throws(new InvalidOperationException("Not found"));

            _mockService.Setup(r => r.FindStudentDb("Nobody")) // nom de famille n'existe pas
                        .Throws(new InvalidOperationException("Not found"));

            _mockService.Setup(r => r.FindStudentDb("Ghost")) // prénom n'existe pas
                        .Throws(new InvalidOperationException("Not found"));

            _service = new StudentService(_mockService.Object, _mockStudio.Object);

            // Act & Assert 1: par id (string)
            Assert.Throws<InvalidOperationException>(() => _service.FindStudentSvc("999"));
            _mockService.Verify(repo => repo.FindStudentDb("999"), Times.Once);

            // Act & Assert 2: par nom de famille
            Assert.Throws<InvalidOperationException>(() => _service.FindStudentSvc("Nobody"));
            _mockService.Verify(repo => repo.FindStudentDb("Nobody"), Times.Once);

            // Act & Assert 3: par prénom
            Assert.Throws<InvalidOperationException>(() => _service.FindStudentSvc("Ghost"));
            _mockService.Verify(repo => repo.FindStudentDb("Ghost"), Times.Once);
        }

        [Fact]
        public void x_Err_UpdateStudentSvc_IdNotFound_Throws()
        {
            // Arrange
            var id_notfound = 999; // inexistant
            var updatedStudent = new StudentUpdate
            {
                lname = "Mark"
            };

            _mockService.Setup(repo => repo.UpdateStudentDb(id_notfound, It.IsAny<StudentUpdate>()))
                        .Throws(new KeyNotFoundException("Student not found"));

            _service = new StudentService(_mockService.Object, _mockStudio.Object);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => _service.UpdateStudentSvc(id_notfound, updatedStudent));

            // Verify
            _mockService.Verify(repo => repo.UpdateStudentDb(
                It.Is<int>(i => i == id_notfound),
                It.Is<StudentUpdate>(u => u.lname == updatedStudent.lname)
            ), Times.Once);
        }
    }
}
