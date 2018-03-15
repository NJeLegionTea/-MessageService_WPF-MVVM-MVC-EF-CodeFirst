using MessageClient.MessageServiceReference;
using System;
using System.Windows;


namespace MessageClient.Model
{
    enum Departments
    {
        Economic,
        Developer,
        Designer
    }

    class AutorizationModel : BaseModel
    {

        private ServiceClient service = new ServiceClient();

        private Employee CurrentUser;

        //Авторизация пользователя
        public Employee Autorization(string login, string password)
        {
            try
            {
                if (service != null)
                {
                    CurrentUser = new Employee
                    {
                        Login = login,
                        Password = password
                    };

                    CurrentUser = service.VerificationAccount(CurrentUser);

                    //Возврат текущего пользователя если такой пользователь существует в БД
                    if (CurrentUser != null)
                    {                        
                        service.Close();

                        return CurrentUser;
                    }
                    else
                        throw new Exception("Неверный логин или пароль");
                }
                else
                    throw new Exception("Сервер недоступен!");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

    }
}