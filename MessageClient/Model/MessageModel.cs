using MessageClient.MessageServiceReference;
using System;
using System.Windows;

namespace MessageClient.Model
{
    class MessageModel : BaseModel
    {
        public Employee SelectedUser { get; set; }

        private ServiceClient service = new ServiceClient();

        public bool SendMessage(Message message)
        {
            try
            {
                service = new ServiceClient();

                if (service != null)
                {
                    if (service.AddNewMessage(message))
                    {
                        service.Close();

                        return true; 
                    }
                    else
                    {
                        MessageBox.Show("Сообщение не доставлено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                    throw new Exception("Сервер недоступен!");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

        public Employee SearchEmployee(string login)
        {
            try
            {
                service = new ServiceClient();

                SelectedUser = service.SearchEmployee(login);

                if (SelectedUser != null)
                {
                    service.Close();

                    return SelectedUser;
                }
                else
                {
                    MessageBox.Show(string.Format("{0} пользователь не найден!", login), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public bool SendNewAccountMessage(string login, Message message)
        {
            try
            {
                service = new ServiceClient();

                if (service != null)
                {
                    if (service.AddNewMessage(message))
                    {
                        service.Close();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Сообщение не доставлено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }                       
                }
                else
                    throw new Exception("Сервер недоступен!");     
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }

    }
}
