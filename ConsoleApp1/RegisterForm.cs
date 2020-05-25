using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FilmCatalog.Controller;
using FilmCatalog.Data;
using FilmCatalog.View;
using Microsoft.EntityFrameworkCore.Internal;

namespace FilmCatalog
{
    public partial class RegisterForm : Form
    {
        public UserController userController;
        public static string user;
        public static string password;
        public static string mail;
        public static string confirmPass;


        public RegisterForm()
        {
            InitializeComponent();

            userController = new UserController();

        }

        public object Replay(string user, string mail, string password, string confirmPass)
        {
            user = this.textBoxUsername.Text.Trim();
            mail = this.textBoxEmail.Text;
            password = this.textBoxPassword.Text;
            confirmPass = this.textBoxConfirmPass.Text;

            if (user == "" || mail == "" || password == "" || confirmPass == "")
            {
                MessageBox.Show("Please enter values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return Replay(user, mail, password, confirmPass);
            }

            if (password != confirmPass)
            {
                MessageBox.Show("Password not matching", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                user = "";
                mail = "";
                password = "";
                confirmPass = "";
                return Replay(user, mail, password, confirmPass);
            }
            else return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            user = this.textBoxUsername.Text.Trim();
            mail = this.textBoxEmail.Text;
            password = this.textBoxPassword.Text;
            confirmPass = this.textBoxConfirmPass.Text;

            using (SqlConnection con = new SqlConnection(Connection.ConnectionString))
            {
                con.Open();

                bool exists = false;

                // create a command to check if the username exists
                using (SqlCommand cmd = new SqlCommand("select count(*) from [Users] where UserName = @UserName", con))
                {
                    cmd.Parameters.AddWithValue("UserName", user);
                    exists = (int)cmd.ExecuteScalar() > 0;
                }

                // if exists, show a message error
                if (exists)
                {
                    MessageBox.Show(user, "This username has been using by another user.");
                    user = "";
                    Replay(user, mail, password, confirmPass);
                }
                else
                {
                    // does not exists, so, persist the user
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [Users] values (@Username, @Password,@email)", con))
                    {
                        cmd.Parameters.AddWithValue("UserName", user);
                        cmd.Parameters.AddWithValue("Password", password);
                        cmd.Parameters.AddWithValue("email", mail);

                        cmd.ExecuteNonQuery();
                        //Display display = new Display(new FilmCatalogContext());

                        MessageBox.Show("Account successfully create.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        Display display = new Display(new FilmCatalogContext());
                    }
                }
                con.Close();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void labelLogin_Click(object sender, EventArgs e)
        {
            using (Form2 form = new Form2())
            {
                form.ShowDialog();
            }
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }
    }
}
