using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace School_Managemnet_System
{
    public partial class Login : Form
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;";

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Login button clicked"); // Debug message

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter Email and Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM AdminUsers WHERE Email=@Email AND Password=@Password";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 1)
                        {
                            MessageBox.Show("Login Successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            frmDashboard dashboard = new frmDashboard();
                            dashboard.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Email or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database connection error: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGoToSignUp_Click(object sender, EventArgs e)
        {
            SignUp signUpForm = new SignUp();
            signUpForm.FormClosed += (s, args) => this.Show(); // Ensure Login form is shown again when SignUp form is closed
            signUpForm.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // No initialization needed
        }
    }
}
