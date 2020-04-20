using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BusServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPassengerService1
    {
        [OperationContract]
        string GetData(int value);
  
        [OperationContract]
        string InsertPassengerDetails(Passenger passeInfo);

        [OperationContract]
        DataSet GetAllPassengerDetails();

        [OperationContract]
        bool DeletePassengerDetails(int Id);

        [OperationContract]
        string UpdatePassengerDetails(Passenger passInfo);

        [OperationContract]
        DataSet GetPassengerByName(string passInfoName);
    }
}
