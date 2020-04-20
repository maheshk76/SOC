using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusServices
{
    public class Passenger
    {
        private int _passID;
        private string _name;
        private string _gender;
        private DateTime _jdate;
        private string _email;
        private int _seat;
        //string _phone = "";


        public int Id
        {
            get { return _passID; }
            set { _passID = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }


        public DateTime JourneyDate
        {
            get { return _jdate; }
            set { _jdate = value; }
        }


        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public int SeatNumber
        {
            get { return _seat; }
            set { _seat = value; }
        }
    }
}
