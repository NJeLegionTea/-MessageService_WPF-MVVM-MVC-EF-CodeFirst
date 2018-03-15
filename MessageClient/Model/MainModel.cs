using System;
using System.Collections.ObjectModel;
using System.Windows;
using MessageClient.MessageServiceReference;

namespace MessageClient.Model
{
    class MainModel : BaseModel
    {
        private ServiceClient service = new ServiceClient();

        private ObservableCollection<Employee> contactList;

        private ObservableCollection<Message> messageList;

        //Получение списка контактов
        public ObservableCollection<Employee> GetContactList(Employee currentUser)
        {
            try
            {
                if (service != null)
                {
                    contactList = new ObservableCollection<Employee>();

                    var contactUser = service.GetContactList(currentUser.Id);

                    currentUser.DialogWith = contactUser;

                    for (int i = 0; i < currentUser.DialogWith.Length; i++)
                    {
                        contactList.Add(currentUser.DialogWith[i]);
                    }

                    return contactList;
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

        //Получение списка сообщений
        public ObservableCollection<Message> GetMessageList(Employee selectedEmployee, Employee currentUser)
        {
            try
            {
                if (service != null)
                {
                    messageList = new ObservableCollection<Message>();

                    if (selectedEmployee != null)
                    {

                        var messages = service.GetMessage(selectedEmployee.Id, currentUser.Id);

                        for (int i = 0; i < messages.Length; i++)
                        {
                            messageList.Add(messages[i]);
                        }

                        return messageList;
                    }
                    return null;

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
