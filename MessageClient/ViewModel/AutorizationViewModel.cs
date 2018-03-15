using System.Windows;
using System.Windows.Controls;
using MessageClient.SupportClass;
using MessageClient.View;
using MessageClient.Model;
using MessageClient.MessageServiceReference;


namespace MessageClient.ViewModel
{
    class AutorizationViewModel : BaseViewModel
    {
        private Window autorizationWindow;

        private Window registrationWindow;

        private Window mainWindow;

        readonly AutorizationModel autorizationModel = new AutorizationModel();

        public Employee CurrentUser { get; set; }    

        private string login;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password { get; set; }
        
        //Попытка авторизации
        public RelayCommand EnterCommand
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    //Получение пароля формы
                    var passwordBox = obj as PasswordBox;
                    if (passwordBox == null)
                        return;
                    var Password = passwordBox.Password;

                    //Проверка введенных данных
                    if (!string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password))
                    {
                        //Получение текущего пользователя
                        CurrentUser = autorizationModel.Autorization(Login, Password);

                        if(CurrentUser!= null)        
                            ShowMainForm();
                        else
                        {
                            Login = null;
                            Password = null;
                        }
                    }
                    else
                        MessageBox.Show("Логин или пароль не могут быть пустыми", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }
        
        public RelayCommand RegistrationCommand
        {
            get { return new RelayCommand(obj => { ShowRegistrationForm(); }); }
        }


        public AutorizationViewModel(Window autorizationWindow)
        {
            this.autorizationWindow = autorizationWindow;
        }

        private void ShowMainForm()
        {
            mainWindow = new MainWindow();
            var mainViewModel = new MainViewModel(CurrentUser);

            mainWindow.DataContext = mainViewModel;

            mainWindow.Title = CurrentUser.Login + " " + CurrentUser.FirstName + " " + CurrentUser.LastName;

            mainWindow.Show();
            autorizationWindow.Close();
        }

        private void ShowRegistrationForm()
        {
            registrationWindow = new RegistrationWindow();

            var registrationViewModel = new RegistrationViewModel(registrationWindow);

            registrationWindow.DataContext = registrationViewModel;

            autorizationWindow.Close();
            registrationWindow.Show();
        }
    }
}
