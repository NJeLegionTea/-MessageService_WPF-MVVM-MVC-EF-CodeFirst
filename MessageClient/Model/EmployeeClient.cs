using System.ComponentModel;

namespace MessageClient.Model
{

    public enum Departments
    {
        Economic,
        Developer,
        Designers
    }

    public class EmployeeClient: INotifyPropertyChanged
    {
        
        private string login;
        private string password;
        private string repeatPassword;
        private string firstName;
        private string lastName;
        private Departments department;


        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public string RepeatPassword
        {
            get { return repeatPassword; }
            set
            {
                repeatPassword = value;
                OnPropertyChanged("RepeatPassword");
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public Departments Department
        {
            get { return department; }
            set
            {
                department = value;
                OnPropertyChanged("Department");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
