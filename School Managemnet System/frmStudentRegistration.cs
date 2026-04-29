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

namespace School_Managemnet_System
{
    public partial class frmStudentRegistration : Form
    {
        private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;TrustServerCertificate=True;";

        public frmStudentRegistration()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            // 1. Basic Validation (Check karna ke zaroori boxes khali na hon)
            if (string.IsNullOrWhiteSpace(txtRegNo.Text) || string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Registration Number and Full Name are required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. Database Connection Open Karna
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 3. SQL Insert Query
                    string query = "INSERT INTO Students (RegistrationNo, FullName, FatherName, ClassName, ContactNo) " +
                                   "VALUES (@RegNo, @FullName, @FatherName, @ClassName, @ContactNo)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        //4.Data ko parameters ke zariye bhejna(SQL Injection se bachne ke liye)
                        cmd.Parameters.AddWithValue("@RegNo", txtRegNo.Text);
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
                        cmd.Parameters.AddWithValue("@ClassName", txtClassName.Text);
                        cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text);

                        // 5. Query Execute Karna
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student Registered Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadLiveStudentData(); // Data save hone ke fauran baad grid ko refresh kar

                        // Fields ko clear karna
                        txtRegNo.Clear();
                        txtFullName.Clear();
                        txtFatherName.Clear();
                        txtClassName.Clear();
                        txtContactNo.Clear();
                        txtRegNo.Focus();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Agar Registration Number (Roll No) pehle se majood ho (UNIQUE Constraint)
                if (sqlEx.Number == 2627)
                {
                    MessageBox.Show("This Registration Number already exists! Please enter a unique number.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Database Error: " + sqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("System Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void LoadLiveStudentData()
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Database se bachon ka data laane ki query
                    string query = "SELECT RegistrationNo, FullName, FatherName, ClassName, ContactNo FROM Students ORDER BY StudentID DESC";

                    // SqlDataAdapter data ko table ki shakal mein uthaata hai
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // GridView ko data de dena
                    dgvStudents.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void frmStudentRegistration_Load(object sender, EventArgs e)
        {
            LoadLiveStudentData(); // Form khulte hi data load hoga
        }

        private void txtRegNo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
