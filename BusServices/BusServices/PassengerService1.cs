using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BusServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class PassengerService1 : IPassengerService1
    {
        
        string connectionString = @"Data Source=(LocalDb)\MSSqlLocaldb;Initial Catalog=Passenger;Integrated Security=True;Pooling=False";
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }
        public string InsertPassengerDetails(Passenger passInfo)
        {

            string Message = "default";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                passInfo.JourneyDate = passInfo.JourneyDate.Date;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText =
                    "INSERT INTO tblPassenger(Name,Gender,JourneyDate,Email,SeatNumber) " +
                    "VALUES(@Name,@Gender,@JourneyDate,@Email,@SeatNumber)";
                cmd.Parameters.AddWithValue("@Name", passInfo.Name);
                cmd.Parameters.AddWithValue("@Gender", passInfo.Gender);
                cmd.Parameters.AddWithValue("@JourneyDate", passInfo.JourneyDate);
                cmd.Parameters.AddWithValue("@Email", passInfo.Email);
                cmd.Parameters.AddWithValue("@SeatNumber", passInfo.SeatNumber);
                con.Open();
                int result = (int)cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    Message = passInfo.Name + " added successfully ";
                    Console.WriteLine(passInfo.Name + " added successfully ");
                }
                else
                {
                    Message = passInfo.Name + " not added ";
                    Console.WriteLine(passInfo.Name + " not added ");
                }

            }
            return Message;

        }

        public DataSet GetAllPassengerDetails()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM tblPassenger ORDER BY EMAIL DESC", con);

                DataSet dataSet = new DataSet();

                adapter.Fill(dataSet);

                Console.WriteLine("DataSet of All Passengers return successfully ");

                return dataSet;

            }

        }

        public bool DeletePassengerDetails(int Id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "DELETE FROM tblPassenger " +
                                  "WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", Id);

                con.Open();
                int result = (int)cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    Console.WriteLine("Passenger with id " + Id.ToString() + " Deleted successfully ");
                    return true;

                }
                Console.WriteLine("Passenger with id " + Id.ToString() + " not Deleted ");
                return false;

            }
        }

        public string UpdatePassengerDetails(Passenger passInfo)
        {
            string Message = "uDefault";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                passInfo.JourneyDate = passInfo.JourneyDate.Date;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "UPDATE tblPassenger SET NAME = @NAME," +
                                  "Gender = @Gender,JourneyDate = @JourneyDate," +
                                  "Email = @Email,SeatNumber=@SeatNumber WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", passInfo.Id);
                cmd.Parameters.AddWithValue("@NAME", passInfo.Name);
                cmd.Parameters.AddWithValue("@Gender", passInfo.Gender);
                cmd.Parameters.AddWithValue("@JourneyDate", passInfo.JourneyDate);
                cmd.Parameters.AddWithValue("@Email", passInfo.Email);
                cmd.Parameters.AddWithValue("@SeatNumber", passInfo.SeatNumber);
                

                con.Open();
                int result = (int)cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    Message = passInfo.Name + " successfully updated";
                }
                else
                {
                    Message = passInfo.Name + " not updated  !!";
                }

            }
            return Message;
        }
        public DataSet GetPassengerByName(string passengerInfoName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from tblPassenger where Name like" +
                    " @passengerInfoName+'%'", con);
                adapter.SelectCommand.Parameters.AddWithValue("@passengerInfoName", passengerInfoName);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                if (dataSet.Tables[0].Rows.Count != 0)
                    Console.WriteLine("DataSet of PassengerName:" + passengerInfoName.ToString() + " return successfully ");
                else
                    Console.WriteLine("Null DataSet  return ");
                return dataSet;
            }
        }
    }
}
