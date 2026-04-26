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
            LoadFormInPanel(new frmStudentRegistration());
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

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new frmAttendance());
            attendanceForm.Show();
        }

        private void btnFee_Click(object sender, EventArgs e)
        {
            LoadFormInPanel(new frmFeeManagement());
        }
        private void LoadFormInPanel(Form childForm)
        {
            // Agar panel mein pehle se koi form khula hai, toh usko hata do
            if (this.mainPanel.Controls.Count > 0)
            {
                this.mainPanel.Controls.RemoveAt(0);
            }

            // Naye form ko panel ke hisaab se set karna
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None; // Form ka apna border khatam kar dega
            childForm.Dock = DockStyle.Fill; // Panel ki puri jagah le lega

            // Form ko panel mein add karna aur show karna
            this.mainPanel.Controls.Add(childForm);
            this.mainPanel.Tag = childForm;
            childForm.Show();
        }
    }
    
}
