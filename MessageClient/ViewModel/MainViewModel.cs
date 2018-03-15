using System;
using MessageClient.MessageServiceReference;
using MessageClient.View;
using MessageClient.SupportClass;
using MessageClient.Model;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace MessageClient.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        readonly MainModel mainModel = new MainModel();

        private DispatcherTimer dispatcherTimer;

        private ObservableCollection<Employee> contactList;

        private ObservableCollection<Message> messageList;

        private Employee selectedEmployee;

        //Быстрый ответ пользователю
        public RelayCommand SendMessageCommand
        {
            get { return new RelayCommand(obj => { SendMessage(); }); }
        }

        //Сообщение пользователю, с которым ранее не было диалогов (новый пользователь добавится в список),
        //либо пользователю, с которым уже был диалог
        public RelayCommand SendNewUserMessageCommand
        {
            get { return new RelayCommand(obj => { SendNewUserMessage(); }); }
        }

        //Обновление списка контактов и сообщений
        public RelayCommand RefreshMessageCommand
        {
            get { return new RelayCommand(obj => { Refresh(); }); }
            
        }

        public Employee CurrentUser { get; set; }
        
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                //Получение списка сообщений
                MessageList = mainModel.GetMessageList(SelectedEmployee, CurrentUser);
                OnPropertyChanged("SelectedEmployee");
            }
        }

        public ObservableCollection<Employee> ContactList
        {
            get { return contactList; }
            set
            {
                contactList = value;
                OnPropertyChanged("ContactList");
            }
        }

        public ObservableCollection<Message> MessageList
        {
            get { return messageList; }
            set
            {
                messageList = value;
                OnPropertyChanged("MessageList");
            }
        }
        
        public MainViewModel(Employee currentUser)
        {
            CurrentUser = currentUser;
            ContactList = mainModel.GetContactList(CurrentUser);
            
            Timer();
        }
        
        private void Refresh()
        {
            MessageList = mainModel.GetMessageList(SelectedEmployee, CurrentUser);
            ContactList = mainModel.GetContactList(CurrentUser);
        }
        
        //Таймер для автоматического обновления через каждые 5 мин.
        private void Timer()
        {
            dispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(0, 5, 0) };
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void SendMessage()
        {
            var messageWindow = new MessageWindow();
            var messageViewModel = new MessageViewModel(CurrentUser, selectedEmployee, this, messageWindow);

            messageWindow.DataContext = messageViewModel;

            messageWindow.Show();
        }

        private void SendNewUserMessage()
        {
            var newUserMessageWindow = new NewUserMessageWindow();
            var messageViewModel = new MessageViewModel(CurrentUser, selectedEmployee, this, newUserMessageWindow);

            newUserMessageWindow.DataContext = messageViewModel;

            newUserMessageWindow.Show();
        }
    }
}
