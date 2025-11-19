using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsDLL.Models
{
    public class UI_Student : INotifyPropertyChanged
    {
        // use public properties for WinForms data-binding
        [DisplayName("PSR-")] public int id { get; set; }

        [DisplayName("First Name")] public required string fname { get; set; }
        [DisplayName("Last Name")] public required string lname { get; set; }
        [DisplayName("E-Mail")] public string? email { get; set; }
        [DisplayName("TEL")] public string? phone { get; set; }
        [DisplayName("Inscription")] public DateTime confirmed { get; set; }
        [DisplayName("Section")] public required string section { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
