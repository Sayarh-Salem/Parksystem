using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VeniceParking
{
    public partial class Form2 : Form
    {
        private ParkingGarage garage;
        private Form1 form1;

        private string username = "username";
        private string password = "password123";

        public Form2(ParkingGarage garage, Form1 form1)
        {
            InitializeComponent();
            this.garage = garage;
            this.form1 = form1;
        }
        private void FloorsDisplay()
        {
            dataGridView1.DataSource = null;
            // Get the dictionary of available spaces per floor
            Dictionary<int, int> availableSpacesPerFloor = garage.GetAvailableSpacesPerFloor();

            // Create a new DataTable to hold the data
            DataTable dataTable = new DataTable();

            // Add the columns to the DataTable
            dataTable.Columns.Add("Floor Number", typeof(int));
            dataTable.Columns.Add("Available Spaces", typeof(int));

            // Add the data rows to the DataTable
            foreach (var floor in availableSpacesPerFloor)
            {
                dataTable.Rows.Add(floor.Key, floor.Value);
            }

            // Bind the DataTable to the DataGridView
            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e) //LOGIN
        {
            string user = textBox1.Text;
            string pass = textBox2.Text;

            if (user.Trim() == username && pass.Trim() == password)
            {
                MessageBox.Show("Welcome to admin page","welcome",MessageBoxButtons.OK,MessageBoxIcon.Information);
                groupBox1.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox2.Visible = false;
                FloorsDisplay();
                dataGridView1.Visible = true;
            }
            else
            {
                MessageBox.Show("Please check ure password or username", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label6.Text = garage.numFloors.ToString();
            label8.Text = garage.numSpacesPerFloor.ToString();
            groupBox1.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
            dataGridView1.Visible = false;

        }

        private void button2_Click(object sender, EventArgs e) //Update Garage
        {
            int numFloors = Convert.ToInt32(numericUpDown1.Value);
            int numSpacesPerFloor = Convert.ToInt32(textBox3.Text);
            garage.UpdateGarage(numFloors, numSpacesPerFloor);


            label6.Text = garage.numFloors.ToString();
            label8.Text = garage.numSpacesPerFloor.ToString();

            FloorsDisplay();

            form1.UpdateGarage(garage);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close(); // Close Form2
            form1.Show(); // Show Form1
        }

        private void button4_Click(object sender, EventArgs e) // try to find Vehicle Position
        {
            string result = garage.FindVehiclePosition(textBox4.Text);
            if (textBox4.Text != "")
            {
                if (result.Contains("nicht gefunden"))
                {
                    MessageBox.Show(result, "Vehicle Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result, "Vehicle Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } 
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            form1.Show(); // Show Form1
        }
    }
}
