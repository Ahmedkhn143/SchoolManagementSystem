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
    public partial class frmAttendance : Form
    {
        string connectionString = @"Data Source=AHMAD-KHAN\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;TrustServerCertificate=True;";

        public frmAttendance()
        {
            InitializeComponent();
        }

        private void frmAttendance_Load(object sender, EventArgs e)
        {
            LoadAttendanceData();
        }

        private void LoadAttendanceData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT a.AttendanceID, a.RegistrationNo, s.FullName, a.AttendanceDate, a.Status " +
                                   "FROM Attendance a " +
                                   "JOIN Students s ON a.RegistrationNo = s.RegistrationNo " +
                                   "ORDER BY a.AttendanceDate DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvStudents.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading attendance: " + ex.Message);
            }
        }

        private void txtRegNo_TextChanged(object sender, EventArgs e)
        {
            // Optional: Live search or validation could go here
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            // 1. Validation: Check karein ke Roll No aur Status khali na hon
            if (string.IsNullOrWhiteSpace(txtRegNo.Text) || cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter Registration Number and select a Status.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 2. Insert Query
                    string query = "INSERT INTO Attendance (RegistrationNo, AttendanceDate, Status) VALUES (@RegNo, @Date, @Status)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RegNo", txtRegNo.Text);

                        // DateTimePicker se sirf Date uthani hai, time nahi
                        cmd.Parameters.AddWithValue("@Date", dtpDate.Value.Date);

                        cmd.Parameters.AddWithValue("@Status", cmbStatus.SelectedItem.ToString());

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Attendance Marked Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // 3. Save hone ke baad agle bachay ke liye fields saaf kar dein
                        txtRegNo.Clear();
                        cmbStatus.SelectedIndex = -1; // ComboBox ko reset karna
                        LoadAttendanceData(); // Refresh grid
                        txtRegNo.Focus(); // Cursor wapis pehlay box mein le aana
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // 4. Foreign Key Error (Error No 547 tab aata hai jab ghalat Roll No diya jaye)
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

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
