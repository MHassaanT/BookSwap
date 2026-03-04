using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;



namespace loginpage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void Email_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnlogin_Click(object sender, EventArgs e)
        {

            if (!File.Exists("users.txt"))
            {
                MessageBox.Show("No users found.");
                return;
            }

            foreach (string line in File.ReadAllLines("users.txt"))
            {
                User user = User.FromString(line);

                if (user.Email == txtEmail.Text && user.Password == txtPassword.Text)
                {
                    MessageBox.Show("Login Successful!");

                    if (user.Role == "Admin")
                        new AdminForm().Show();
                    else
                        new StudentForm().Show();

                    this.Hide();
                    return;
                }
            }

            MessageBox.Show("Invalid Email or Password");
        }

        private void btnGoToRegister_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
        }
    }
}
