using System;
using System.Windows;
using MessageClient.View;
using MessageClient.MessageServiceReference;
using MessageClient.Model;
using MessageClient.SupportClass;

namespace MessageClient.ViewModel
{
    class RegistrationViewModel : BaseViewModel
    {
        private Window registrationWindow;

        private readonly RegistrationModel registrationModel = new RegistrationModel();

        public Employee NewEmployee { get; set; }
        
        public string RepeatPassword { get; set; }

        public Array DepartmentsArray
        {
            get { return Enum.GetValues(typeof(Departments)); }
        }

        private Departments department;

        public Departments Department
        {
            get { return department; }
            set
            {
                department = value;
                NewEmployee.Department = (int)department;
                OnPropertyChanged("Department");
            }
        }

        //Попытка регистрации
        public RelayCommand AcceptCommand
        {
            get
            {
                return new RelayCommand(obj => 
                {

                    if (!string.IsNullOrEmpty(NewEmployee.FirstName) && !string.IsNullOrEmpty(NewEmployee.LastName) &&
                        !string.IsNullOrEmpty(NewEmployee.Password) && !string.IsNullOrEmpty(RepeatPassword))
                    {
                        if (NewEmployee.FirstName.Length > 3 && NewEmployee.FirstName.Length < 20 &&
                            NewEmployee.LastName.Length > 3 && NewEmployee.LastName.Length < 20 &&
                            NewEmployee.Password.Length > 5 && NewEmployee.Password.Length < 20)
                        {
                            if (NewEmployee.Password == RepeatPassword)
                            {
                                //Получение зарегистрированного пользователя
                                NewEmployee = registrationModel.Accept(NewEmployee);

                                if (NewEmployee != null)
                                {
                                    MessageBox.Show(String.Format("Ваше имя в системе: {0}", NewEmployee.Login), "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);

                                    //Создание главного окна при успешной регистрации
                                    ShowMainForm();
                                }
                                else
                                    MessageBox.Show("Сервер недоступен.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                            MessageBox.Show("Неверная длина имени или пароля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("Поля не могут быть пустыми.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        //Отмена регистрации
        public RelayCommand CancelCommand
        {
            get { return new RelayCommand(obj => { Cancel(); }); }
        }
        
        public RegistrationViewModel(Window registrationWindow)
        {
            NewEmployee = new Employee();

            this.registrationWindow = registrationWindow;
        }


        private void Cancel()
        {
            var autorizationWindow = new AutorizationWindow();
            var autorizationViewModel = new AutorizationViewModel(autorizationWindow);

            autorizationWindow.DataContext = autorizationViewModel;

            autorizationWindow.Show();

            registrationWindow.Close();

        }

        private void ShowMainForm()
        {
            var mainWindow = new MainWindow();
            var mainViewModel = new MainViewModel(NewEmployee);

            mainWindow.DataContext = mainViewModel;
            mainWindow.Title = NewEmployee.Login + " " + NewEmployee.FirstName + " " + NewEmployee.LastName;

            registrationWindow.Close();
            mainWindow.Show();
        }
    }
}
