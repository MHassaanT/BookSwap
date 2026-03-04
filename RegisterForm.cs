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
    public partial class RegisterForm : Form
    {
        
        public RegisterForm()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
       
           {
            if (txtName.Text == "" || txtEmail.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("All fields required!");
                return;
            }

            if (File.Exists("users.txt"))
            {
                foreach (string line in File.ReadAllLines("users.txt"))
                {
                    User existing = User.FromString(line);

                    if (existing.Email == txtEmail.Text)
                    {
                        MessageBox.Show("Email already exists!");
                        return;
                    }
                }
            }

            User user = new User
            {
                Name = txtName.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Role = comboRole.Text,
                Contact = txtContact.Text
            };

            File.AppendAllText("users.txt", user.ToString() + Environment.NewLine);

            MessageBox.Show("Registration Successful!");
            this.Close();
        }
    }
}
