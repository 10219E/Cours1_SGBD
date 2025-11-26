using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsDLL.Models;

namespace ModelsDLL.Models
{
    public class UI_StudioStudent : INotifyPropertyChanged
    {
        [DisplayName("UID")] public int UID { get; set; }
        [DisplayName("Studio NO")] public int StudioNo { get; set; }
        
        // Keep the complex object for internal use
        [Browsable(false)] // Hide from DataGridView
        public UI_Student? Student { get; set; }
        
        // Add flattened properties for display
        [DisplayName("Student ID")] 
        public int? StudentId => Student?.id;
        
        [DisplayName("Student Name")] 
        public string StudentName => Student != null ? $"{Student.fname} {Student.lname}" : string.Empty;

        [DisplayName("Student Email")]
        public string StudentEmail => Student != null ? $"{Student.email}" : string.Empty;

        [DisplayName("Building Name")] public string Building { get; set; }
        [DisplayName("Lease Renewal")] public DateTime? lease { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

    }
}
