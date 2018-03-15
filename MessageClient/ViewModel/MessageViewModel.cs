using System;
using System.Windows;
using MessageClient.View;
using MessageClient.Model;
using MessageClient.MessageServiceReference;
using MessageClient.SupportClass;

namespace MessageClient.ViewModel
{
    class MessageViewModel : BaseViewModel
    {
        readonly MessageModel messageModel = new MessageModel();

        readonly MainModel mainModel = new MainModel();

        private MainViewModel mainViewModel;

        private MessageWindow messageWindow;

        private NewUserMessageWindow newUserMessageWindow;

        public Employee CurrentUser { get; set; }

        public Employee SelectedUser { get; set; }

        public Message CurrentMessage { get; set; }

        //Быстрая отправка сообщения
        public RelayCommand SendMessageCommand
        {
            get
            {
                return new RelayCommand(obj => 
                {
                    if (CurrentMessage.Contents != null && CurrentMessage.Name != null)
                    {
                        CurrentMessage.Sender = CurrentUser;
                        CurrentMessage.Recipient = SelectedUser;

                        bool success = messageModel.SendMessage(CurrentMessage);

                        if (success)
                        {
                            MessageBox.Show(string.Format("Сообщение для {0} отправлено", SelectedUser.FirstName), "Отправлено", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            mainViewModel.MessageList = mainModel.GetMessageList(SelectedUser, CurrentUser);

                            messageWindow.Close();
                        }
                        else
                            MessageBox.Show("Сообщение не доставлено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                });
            }
        }

        //Отправка сообшения новому пользователю
        public RelayCommand SendMessageNewUserCommand
        {
            get
            {
                return new RelayCommand(obj => 
                {
                    string login = obj as string;

                    //Поиск пользователя в БД
                    SelectedUser = messageModel.SearchEmployee(login);

                    if (SelectedUser != null)
                    {
                        if (CurrentMessage.Contents != null && CurrentMessage.Name != null && SelectedUser != null)
                        {
                            CurrentMessage.Sender = CurrentUser;
                            CurrentMessage.Recipient = SelectedUser;

                            bool success = messageModel.SendNewAccountMessage(login, CurrentMessage);

                            if (success)
                            {
                                MessageBox.Show(string.Format("Сообщение для {0} отправлено", login), "Отправлено", MessageBoxButton.OK, MessageBoxImage.Information);

                                mainViewModel.ContactList = mainModel.GetContactList(CurrentUser);
                                mainViewModel.MessageList = mainModel.GetMessageList(SelectedUser, CurrentUser);

                                newUserMessageWindow.Close();
                            }
                            else
                                MessageBox.Show("Сообщение не доставлено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                            MessageBox.Show("Все поля должны быть заполнены!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                });
            }
        }

        public MessageViewModel(Employee currentUser, Employee selectedUser, MainViewModel mainViewModel, MessageWindow messageWindow)
        {
            CurrentUser = currentUser;
            SelectedUser = selectedUser;

            CurrentMessage = new Message();

            this.mainViewModel = mainViewModel;
            this.messageWindow = messageWindow;
        }

        public MessageViewModel(Employee currentUser, Employee selectedUser, MainViewModel mainViewModel, NewUserMessageWindow newUserMessageWindow)
        {
            CurrentUser = currentUser;
            SelectedUser = selectedUser;

            CurrentMessage = new Message();

            this.mainViewModel = mainViewModel;
            this.newUserMessageWindow = newUserMessageWindow;
        }
    }
}
