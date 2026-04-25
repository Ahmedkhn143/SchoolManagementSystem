using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Managemnet_System
{
    public partial class frmDashboard : Form
    {
        public frmDashboard()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

       
            private void BtnStudent_Click(object sender, EventArgs e)
        {
            frmStudentRegistration studentForm = new frmStudentRegistration();
            studentForm.Show();
            // Yahan hum this.Hide() nahi likh rahe taake Dashboard peeche open rahe
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Logout karne par Login form par wapis bhejna
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }
    }
    
}
