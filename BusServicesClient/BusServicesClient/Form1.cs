using BusServicesClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusServicesClient
{
    public partial class Form1 : Form
    {

        PassengerService1Client client = new PassengerService1Client();
        private int PassIdForDeletion;
        int seat = -1;
        public Form1()
        {
            InitializeComponent();
            Reset();
        }
        public bool ValidateForm(Passenger passenger)
        {
            if (passenger.Name == "" || passenger.Gender == "" || passenger.Email == "" || passenger.SeatNumber < 0)
                return false;
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Passenger passInfo = new Passenger();
                passInfo.Name = tbPassengerName.Text;
                passInfo.Gender = tbPassengerGender.Text;
                passInfo.JourneyDate = Convert.ToDateTime(tbDateOfBirth.Text);
                passInfo.Email = tbEmail.Text;

                foreach (Control c in groupBox1.Controls)
                {
                    if (c is RadioButton)
                    {
                        RadioButton x = (RadioButton)c;
                        if (x.Checked)
                            seat = Convert.ToInt32(c.Text);
                        x.Checked = false;
                    }

                }
                passInfo.SeatNumber = seat;
                passInfo.Id = PassIdForDeletion;
                if (ValidateForm(passInfo))
                {
                    if (btnSave.Text.Equals("Save"))
                        lblTest.Text = SavePassengerInfo(passInfo);
                    else
                        lblTest.Text = UpdatePassengerInfo(passInfo);
                    lblTest.ForeColor = Color.Green;
                }
                else
                {
                    lblTest.Text = "Enter valid data";
                    lblTest.ForeColor = Color.OrangeRed;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                MessageBox.Show("Something went wrong", "Error");
            }
            Reset();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (DeletePassengerInfo())
                {
                    lblTest.Text = "Delete SuccessFully";
                    lblTest.ForeColor = Color.Green;
                }
                else
                {
                    lblTest.Text = "Some Problem !!";
                    lblTest.ForeColor = Color.OrangeRed;

                }
                Reset();
                lblTest.Text = "";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                MessageBox.Show("Something went wrong", "Error");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet dataSet = client.GetPassengerByName(tbSearchBox.Text);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    PopulateDataGridView(dataSet);
                    lblTest.Text = "Showing search results";
                    lblTest.ForeColor = Color.Green;
                }
                else
                {
                    lblTest.Text = "No data found";
                    lblTest.ForeColor = Color.OrangeRed;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                MessageBox.Show("Something went wrong", "Error");
            }

        }
        private void dgvPassengerDetails_DoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine(dgvPassengerDetails.CurrentRow.Index);
            if (dgvPassengerDetails.CurrentRow.Index != -1)
            {
                PassIdForDeletion = Convert.ToInt32(dgvPassengerDetails.CurrentRow.Cells[0].Value.ToString());
                tbPassengerName.Text = dgvPassengerDetails.CurrentRow.Cells[1].Value.ToString();
                tbPassengerGender.Text = dgvPassengerDetails.CurrentRow.Cells[2].Value.ToString();
                tbDateOfBirth.Text = dgvPassengerDetails.CurrentRow.Cells[3].Value.ToString();
                tbEmail.Text = dgvPassengerDetails.CurrentRow.Cells[4].Value.ToString();
                string temp = dgvPassengerDetails.CurrentRow.Cells[5].Value.ToString();
                foreach (Control c in groupBox1.Controls)
                {
                    if (c is RadioButton)
                    {

                        RadioButton x = (RadioButton)c;
                        if (x.BackColor == Color.LightGreen)
                            x.BackColor = Color.LightGray;
                        if (x.Text == temp)
                        {
                            x.Checked = true;
                            x.BackColor = Color.LightGreen;
                        }
                    }
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;

                Console.WriteLine(PassIdForDeletion);
            }
        }
        //************************Operation****************************************//
        private string SavePassengerInfo(Passenger passInfo)
        {
            string Message = client.InsertPassengerDetails(passInfo);
            return Message;
        }
        private string UpdatePassengerInfo(Passenger passInfo)
        {
            string Message = client.UpdatePassengerDetails(passInfo);
            return Message;
        }
        private bool DeletePassengerInfo()
        {
            bool deletebool = client.DeletePassengerDetails(PassIdForDeletion);
            return deletebool;
        }
        private void Reset()
        {
            tbPassengerName.Text = tbPassengerGender.Text = tbEmail.Text = label8.Text = String.Empty;
            tbDateOfBirth.MinDate = DateTime.Now.Date;
            Console.WriteLine(tbDateOfBirth.Value);
            btnSave.Text = "Save";
            btnDelete.Enabled = true;
            PassIdForDeletion = 0;
            seat = -1;
            DataSet ds = new DataSet();
            ds = client.GetAllPassengerDetails();
            PopulateDataGridView(ds);
            foreach (Control c in groupBox1.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rd = (RadioButton)c;
                    rd.BackColor = Color.Empty;
                    rd.Enabled = true;
                    rd.Checked = false;
                }
            }
            GetReservedSeats();
        }
        public void GetReservedSeats(){
            DataSet ds = client.GetAllPassengerDetails();
            List<int> l = new List<int>();
        var x = (from DataRow dr in ds.Tables[0].Rows where
                 dr["JourneyDate"].ToString()==tbDateOfBirth.Value.ToString()
                 select Convert.ToInt32(dr["SeatNumber"]));
            foreach(int p in x)
                l.Add(p);
            foreach (Control c in groupBox1.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rd = (RadioButton)c;
                    if (l.Contains(Convert.ToInt32(rd.Text)))
                    {
                        rd.BackColor = Color.LightGray;
                        rd.Enabled = false;
                    }
                }
            }
        }
        private void PopulateDataGridView(DataSet dataSet)
        {
            dgvPassengerDetails.DataSource = dataSet.Tables[0];
            for (int i = 0; i < dgvPassengerDetails.Columns.Count; i++)
                dgvPassengerDetails.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void groupBox1_Enter(object sender, MouseEventArgs e)
        {
        }

        private void tbDateOfBirth_ValueChanged(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
