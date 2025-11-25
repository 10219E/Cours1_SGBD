using ModelsDLL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StudioGridForm : Form
{
    private List<UI_StudioStudent> studios;

    public StudioGridForm(List<UI_StudioStudent> studios)
    {
        var dgv = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = true,
            DataSource = new BindingList<UI_StudioStudent>(studios)
        };
        Controls.Add(dgv);
        Text = "Studios";
        Width = 900;
        Height = 400;
    }

}