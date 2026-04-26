using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace School_Managemnet_System
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Register button clicked"); // Debug message

            string connString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;";
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string query = "INSERT INTO AdminUsers (Email, Password) VALUES (@Email, @Password)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Account Created! Now please Login.");

                        this.Hide();
                        Login login = new Login();
                        login.Show();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error: " + sqlEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
        }

        private void btnLoginPage_Click(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void SignUp_Load(object sender, EventArgs e) { }
    }
}
