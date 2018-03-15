using System.ServiceModel;
using MessageService.SupportClass;
using System.Collections.Generic;

namespace MessageService
{
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        Employee VerificationAccount(Employee employee);

        [OperationContract]
        Employee Registration(Employee employee);

        [OperationContract]
        bool SearchDuplicateLogin(string login);

        [OperationContract]
        List<SupportClass.Message> GetMessage(int currentUserId, int selectedUserId);

        [OperationContract]
        bool AddNewMessage(SupportClass.Message message);

        [OperationContract]
        Employee SearchEmployee(string login);

        [OperationContract]
        List<Employee> GetContactList(int UserId);
    }

}
