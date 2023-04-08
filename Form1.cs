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
    public partial class Form1 : Form

    {
        private ParkingGarage garage;

        public Form1()
        {
            InitializeComponent();
        }
        public void UpdateGarage(ParkingGarage updatedGarage)
        {
            garage = updatedGarage;
            comboBox2.Items.Clear();
            for (int i = 1; i <= garage.numFloors; i++)
            {
                comboBox2.Items.Add(i);
            }
            comboBox2.SelectedIndex = 0;
        }
        void ClearAllText(Control con)  // Clears all text fields on the form
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                else if (c is ComboBox)
                {
                    ComboBox dd = (ComboBox)c;
                    if (dd != null)
                    {
                        dd.Text = string.Empty;
                        dd.SelectedIndex = -1;
                    }
                }
                else
                    ClearAllText(c);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = "";
            int numFloors = 3;
            int numSpacesPerFloor = 10;
            garage = new ParkingGarage(numFloors, numSpacesPerFloor);

            //Setting the drop down values from enum VehicleType
            comboBox1.DataSource = Enum.GetValues(typeof(VehicleType));
            for (int i = 1; i <= garage.numFloors; i++)
            {
                comboBox2.Items.Add(i);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Park Car Button
        {
            if(textBox1.Text != "" && comboBox1.Text != "" && comboBox2.Text != "" )
            {
                string vehicleId = textBox1.Text;
                Vehicle vehicle = new Vehicle { Id = vehicleId };

                bool isParked;
                string message = garage.TryParkVehicle(vehicle, out isParked);

                if (isParked)
                {
                    label4.ForeColor = System.Drawing.Color.Green;
                    ClearAllText(this);
                }
                else
                {
                    label4.ForeColor = System.Drawing.Color.Red;
                }

                label4.Text = message;

                MessageBoxIcon icon;
                if (isParked)
                {
                    icon = MessageBoxIcon.Information;
                }
                else
                {
                    icon = MessageBoxIcon.Error;
                }

                MessageBox.Show(message, "Parkstatus", MessageBoxButtons.OK, icon);
            }
            else
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus !", "Fehlende Felder", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e) //Remove Car Button
        {
            if (textBox1.Text != "")
            {
                if (garage.FindVehiclePosition(textBox1.Text).Contains("nicht gefunden"))
                {
                    MessageBox.Show("Fahrzeug nicht gefunden", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    label4.Text = "Fahrzeug nicht gefunden";
                    label4.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    string vehicleId = textBox1.Text;
                    garage.RemoveVehicle(vehicleId);
                    MessageBox.Show("Auto erfolgreich entfernt", "Entfernung erfolgreich", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    label4.Text = "Auto erfolgreich entfernt";
                    label4.ForeColor = System.Drawing.Color.Green;
                    ClearAllText(this);
                }

            }
            else
            {
                MessageBox.Show("Bitte füllen Sie Fahrzeug-ID aus", "Fehlende Informationen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                label4.Text = "Bitte füllen Sie Fahrzeug-ID aus";
                label4.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void button3_Click(object sender, EventArgs e) // Go to Form 2
        {
            Form2 myForm = new Form2(garage,this);
            this.Hide();
            myForm.ShowDialog();
        }
    }
}
