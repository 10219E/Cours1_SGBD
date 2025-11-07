using ModelsDLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StudentGridForm : Form
{
    private List<UI_Student> students;

    public StudentGridForm(List<UI_Student> students)
    {
        var dgv = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = true,
            DataSource = new BindingList<UI_Student>(students)
        };
        Controls.Add(dgv);
        Text = "Students";
        Width = 900;
        Height = 400;
    }

}