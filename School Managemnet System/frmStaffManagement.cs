using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace School_Managemnet_System
{
    public partial class frmStaffManagement : Form
    {
        public frmStaffManagement()
        {
            InitializeComponent();
        }
        // Apna theek wala Connection String (Dot ke sath)
        private readonly string connectionString = @"Data Source=AHMAD-KHAN\SQLEXPRESS;Initial Catalog=SchoolDB;Integrated Security=True;TrustServerCertificate=True;";

        // 1. Form Load par live data lana
        private void frmStaffManagement_Load(object sender, EventArgs e)
        {
            LoadStaffData();
        }

        // 2. Live Data Function (Grid ko refresh karne ke liye)
        private void LoadStaffData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT StaffID, FullName, Designation, Salary, ContactNo, HireDate FROM Staff ORDER BY StaffID DESC";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvStaff.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        // 3. Save Button ka Code
        private void btnSaveStaff_Click(object sender, EventArgs e)
        {
            // Validation: Check karna ke khali boxes save na hon
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || cmbDesignation.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtSalary.Text))
            {
                MessageBox.Show("Please fill Name, Designation, and Salary.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Amount Validation: Salary mein ABCD na likha ho
            if (!decimal.TryParse(txtSalary.Text, out decimal staffSalary))
            {
                MessageBox.Show("Please enter a valid number for Salary.", "Invalid Amount", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Staff (FullName, Designation, Salary, ContactNo) VALUES (@FullName, @Designation, @Salary, @ContactNo)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                        cmd.Parameters.AddWithValue("@Designation", cmbDesignation.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Salary", staffSalary);
                        cmd.Parameters.AddWithValue("@ContactNo", txtContactNo.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Staff Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Fields clear karna naye data ke liye
                        txtFullName.Clear();
                        cmbDesignation.SelectedIndex = -1;
                        txtSalary.Clear();
                        txtContactNo.Clear();

                        // Live Grid ko refresh karna taake naya record foran nazar aaye
                        LoadStaffData(); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStaff_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvStaff_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            dgvStaff_CellContentClick(sender, e);
        }

       
    }
}
