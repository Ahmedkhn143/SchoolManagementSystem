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
        private string connectionString = @"Data Source=.\\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;";

        public frmFeeManagement()
        {
            InitializeComponent();
        }

        private void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            // 1. Basic Validation: Check karein ke koi field khali na ho
            if (string.IsNullOrWhiteSpace(txtRegNo.Text) || cmbStatus.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Please fill all fields (Registration No, Month, and Amount).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Amount Validation: Check karein ke user ne Amount mein ABCD toh nahi likh diya
            // TryParse check karega ke value number hai ya nahi. Agar number hai toh 'feeAmount' mein save kar dega.
            if (!decimal.TryParse(txtAmount.Text, out decimal feeAmount))
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
                        txtAmount.Text = string.Empty;
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
    }
}
