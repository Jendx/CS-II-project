using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Datalayer;


namespace WPF
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            Pokladna pokladna = new Pokladna(this.textBoxLog.Text);
            //JSONAPI Json = new JSONAPI();
            //var users = await JSONAPI.GetUsers();
            //var p = await Json.GetItem(1234);
            if (!(this.textBoxLog.Text.Length == 0))
            {
                this.Hide();
                pokladna.ShowDialog(); 
            }
            else
                { MessageBox.Show("LOGIN MUST NOT BE EMPTY!", "Alert", MessageBoxButtons.OK);  }
        }
    }
}
