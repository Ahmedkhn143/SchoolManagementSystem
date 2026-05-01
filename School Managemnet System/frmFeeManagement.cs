using MongoDB.Driver.Core.Configuration;
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
    public partial class frmFeeManagement : Form
    {
        private readonly string connectionString = @"Data Source=AHMAD-KHAN\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;TrustServerCertificate=True;";

        public frmFeeManagement()
        {
            InitializeComponent();
        }

        private void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            // 1. Basic Validation: Check karein ke koi field khali na ho
            if (string.IsNullOrWhiteSpace(txtRegNo.Text) || cmbStatus.SelectedIndex == -1 || string.IsNullOrWhiteSpace(amount.Text))
            {
                MessageBox.Show("Please fill all fields (Registration No, Month, and Amount).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Amount Validation: Check karein ke user ne Amount mein ABCD toh nahi likh diya
            // TryParse check karega ke value number hai ya nahi. Agar number hai toh 'feeAmount' mein save kar dega.
            if (!decimal.TryParse(amount.Text, out decimal feeAmount))
            {
                MessageBox.Show("Please enter a valid number for the Fee Amount.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 3. Insert Query
                    string query = "INSERT INTO Fees (RegistrationNo, FeeMonth, AmountPaid) VALUES (@RegNo, @Month, @Amount)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RegNo", txtRegNo.Text);
                        cmd.Parameters.AddWithValue("@Month", cmbStatus.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Amount", feeAmount); // Yahan humne convert kiya hua number bheja hai

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Fee Submitted Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 4. Save hone ke baad agle bachay ke liye fields saaf kar dein
                        txtRegNo.Clear();
                        cmbStatus.SelectedIndex = -1;
                        amount.Text = string.Empty;
                        txtRegNo.Focus();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // 5. Foreign Key Error: Agar ghalat Roll No diya jaye
                if (sqlEx.Number == 547)
                {
                    MessageBox.Show("This Registration Number does not exist! Please enter a valid registered student.", "Student Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnPayFee_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Basic Validation
                if (string.IsNullOrWhiteSpace(txtRegNo.Text) || cmbStatus.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtAmount.Text))
                {
                    MessageBox.Show("Please fill all fields (Registration No, Month, and Amount).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Amount Validation
                if (!decimal.TryParse(txtAmount.Text, out decimal feeAmount))
                {
                    MessageBox.Show("Please enter a valid number for the Fee Amount.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Database Logic
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Ensure the query matches the table structure
                    string query = "INSERT INTO Fees (RegistrationNo, FeeMonth, AmountPaid) VALUES (@RegNo, @Month, @Amount)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RegNo", txtRegNo.Text);
                        command.Parameters.AddWithValue("@Month", cmbStatus.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@Amount", feeAmount);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Fee payment recorded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear fields after successful insertion
                            txtRegNo.Clear();
                            cmbStatus.SelectedIndex = -1;
                            txtAmount.Clear();
                            txtRegNo.Focus();

                            LoadFeeData();
                        }
                        else
                        {
                            MessageBox.Show("Failed to record fee payment.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFeeData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to fetch data from the Fees table
                    string query = "SELECT * FROM Fees";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the data to the DataGridView
                        dgvStudents.DataSource = dataTable;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmFeeManagement_Load(object sender, EventArgs e)
        {
            // Load data into the DataGridView when the form loads
            LoadFeeData();
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtRegNo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
