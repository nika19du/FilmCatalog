using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FilmCatalog.View;
using FilmCatalog.Controller;
using FilmCatalog.Data;

namespace FilmCatalog
{
    public partial class Form2 : Form
    {
        private UserController userController;
        private FilmCatalogContext context;
        public static string loginUser;
        public static string passUser;
        public Form2()
        {
            InitializeComponent();

            this.context = new FilmCatalogContext();
            this.userController = new UserController(context);
        }

        
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loginUser = UserNametxtbox.Text;
            passUser = passtxtBox.Text;

            if (loginUser == "" ||passUser == "")
            {
                MessageBox.Show("Please enter values", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] input = new string[] { loginUser, passUser };
            bool logIn = userController.LogIn(input);

            if (logIn == true)
            {
                MessageBox.Show("Successfully log in","Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                Display display = new Display(new FilmCatalogContext());
            }
            else
            {
                MessageBox.Show("Unsuccessfully log in. Try again!","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                loginUser = null;
                passUser = null;
            }
        }

        private void signUpLabel_Click(object sender, EventArgs e)
        {
            using (RegisterForm rf=new RegisterForm())
            {
                rf.ShowDialog();
            }
        }
    }
}
