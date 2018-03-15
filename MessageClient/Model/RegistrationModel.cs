using MessageClient.MessageServiceReference;
using System;
using System.Windows;

namespace MessageClient.Model
{
    class RegistrationModel : BaseModel
    {

        private Employee NewEmployee { get; set; }

        private Departments Department { get; set; }

        private ServiceClient service = new ServiceClient();

        //Login генирируется автоматически, из фамилии сотрудника и его отдела,
        //если сотрудник с таким именем и фамилией уже работает в отделе, 
        //после фамилии ставится точка и записывается первая буква имени, если
        //сотрудники с одинаковыми именами у второго пользователя записываются две буквы имени и т.д.
        //пример: Bykov_Developer, Bykov.K_Developer

        public Employee Accept(Employee NewEmployee)
        {
            try
            {
                if(service != null) {

                    Department = (Departments)NewEmployee.Department;                  

                    string login = NewEmployee.LastName + "_" + Department.ToString();

                    if (service.SearchDuplicateLogin(login))
                    {
                        int i = 1;
                        while (service.SearchDuplicateLogin(login))
                        {
                            login = NewEmployee.LastName + "." + NewEmployee.FirstName.Remove(i) + "_" + Department.ToString();
                            i++;
                        }

                        NewEmployee.Login = login;
                        NewEmployee.Password = NewEmployee.Password;

                    }
                    else
                    {
                        NewEmployee.Login = login;
                        NewEmployee.Password = NewEmployee.Password;
                    }
                    
                    //Получение зарегистрированного пользователя
                    NewEmployee = service.Registration(NewEmployee);

                    service.Close();

                    return NewEmployee;
                }
                else
                    throw new Exception("Сервер не доступен.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

        }

    }
}