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
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;";

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
                        // 4. Data ko parameters ke zariye bhejna (SQL Injection se bachne ke liye)
                        cmd.Parameters.AddWithValue("@RegNo", txtRegNo.Text);
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
                        cmd.Parameters.AddWithValue("@ClassName", txtClassName.Text);
                        cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text);

                        // 5. Query Execute Karna
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Student Registered Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 6. Save hone ke baad form ko saaf (clear) kar dena
                        txtRegNo.Text = string.Empty;
                        txtFullName.Text = string.Empty;
                        txtFatherName.Text = string.Empty;
                        txtClassName.Text = string.Empty;
                        txtContactNo.Text = string.Empty;
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
    }
}
