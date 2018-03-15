using System;
using System.Linq;
using MessageService.SupportClass;
using MessageService.DataBase;
using System.Collections.Generic;

namespace MessageService
{
    public class Service : IService
    {

        public Employee SearchEmployee(string login)
        {
            using(var db = new DataBaseContext())
            {
                try
                {
                    var foundEmployeeAcc = db.Accounts.FirstOrDefault(p => p.Login == login);

                    var foundEmploeeProfile = db.AccountProfiles.FirstOrDefault(p => p.EmployeeId == foundEmployeeAcc.EmployeeId);

                    Employee employee = new Employee
                    {
                        Id = foundEmployeeAcc.EmployeeId,
                        Login = foundEmployeeAcc.Login,
                        FirstName = foundEmploeeProfile.FirstName,
                        LastName = foundEmploeeProfile.LastName,
                        Department = foundEmploeeProfile.Department,
                    };

                    return employee;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public bool AddNewMessage(SupportClass.Message message)
    {
            using(var db = new DataBaseContext())
            {
                try
                {
                    DataBase.Message newMessage = new DataBase.Message
                    {
                        Contents = message.Contents,
                        Date = DateTime.UtcNow,
                        Name = message.Name,
                        RecipientEmployeeId = message.Recipient.Id,
                        SenderEmployeeId = message.Sender.Id,
                    };

                    db.Messages.Add(newMessage);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        

        public List<SupportClass.Message> GetMessage(int currentUserId, int selectedUserId)
        {
            using (var db = new DataBaseContext())
            {
                //Поиск сообщений, где учавствовали оба пользователя
                var messagesDb = db.Messages.Where(p => p.SenderEmployeeId == currentUserId && p.RecipientEmployeeId == selectedUserId ||
                p.SenderEmployeeId == selectedUserId && p.RecipientEmployeeId == currentUserId).ToList();

                List<SupportClass.Message> messages = new List<SupportClass.Message>();

                //Создание списка пользователей с соединением таблиц AccountProfile и Account
                List<Employee> employees = db.Accounts.Join(db.AccountProfiles,
                    a => a.EmployeeId,
                    p => p.EmployeeId,
                    (a, p) => new Employee
                    {
                        Id = a.EmployeeId,
                        Login = a.Login,
                        Password = a.Password,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        Department = p.Department,
                    }).ToList();

                //Добавление в список messages сообщений с ссылками на отправителя и получателя
                for (int i = 0; i < messagesDb.Count; i++)
                {
                    messages.Add(new SupportClass.Message
                    {
                        MessageId = messagesDb[i].MessageId,
                        Name = messagesDb[i].Name,
                        Recipient = employees.Find(p => p.Id == messagesDb[i].RecipientEmployeeId),
                        Sender = employees.Find(p => p.Id == messagesDb[i].SenderEmployeeId),
                        Contents = messagesDb[i].Contents,
                        Date = messagesDb[i].Date
                    });
                }
                return messages;
            }
        }

        public Employee VerificationAccount(Employee employee)
        {
            using (var db = new DataBaseContext())
            {
                //Поиск пользователя в базе
                var verification = db.Accounts.FirstOrDefault(p => p.Login.ToLower().Equals(employee.Login.ToLower()) && p.Password.Equals(employee.Password));
               
                //Если пользователь присутствует
                if (verification != null)
                {
                    var foundAccount = db.Accounts.FirstOrDefault(p => p.EmployeeId.Equals(verification.EmployeeId));
                    var foundAccountProfile = db.AccountProfiles.FirstOrDefault(p => p.EmployeeId.Equals(verification.EmployeeId));

                    Employee currentUser = new Employee
                    {
                        Id = foundAccount.EmployeeId,
                        Login = foundAccount.Login,
                        Password = foundAccount.Password,
                        FirstName = foundAccountProfile.FirstName,
                        LastName = foundAccountProfile.LastName,
                        Department = foundAccountProfile.Department
                    };

                    //Добавляем всех сотрудников, с кем был диалог
                    currentUser.DialogWith = GetContactList(currentUser.Id);

                    return currentUser;
                }
                return null;
            }
        }

        public List<Employee> GetContactList(int UserId)
        {
            using (var db = new DataBaseContext())
            {
                //Находим все сообщения, с которыми связан пользователь 
                var contacts = db.Messages.Where(p => p.SenderEmployeeId == UserId || p.RecipientEmployeeId == UserId).ToList();

                List<Employee> employees = new List<Employee>();

                int? id;
                for (int i = 0; i < contacts.Count; i++)
                {
                    //Находим всех пользователей с которыми общался пользователь
                    id = contacts[i].RecipientEmployeeId == UserId ? contacts[i].SenderEmployeeId : contacts[i].RecipientEmployeeId;

                    var acc = db.Accounts.Find(id);
                    var prof = db.AccountProfiles.Find(id);

                    if (employees.FirstOrDefault(p => p.Id == acc.EmployeeId) == null)
                    {
                        employees.Add(new Employee
                        {
                            Id = acc.EmployeeId,
                            Login = acc.Login,
                            Password = acc.Password,
                            FirstName = prof.FirstName,
                            LastName = prof.LastName,
                            Department = prof.Department
                        });
                    }

                }

                return employees;

            }
        }

        public Employee Registration(Employee employee)
        {
            using (var db = new DataBaseContext())
            {
                
                if (db.Accounts.FirstOrDefault(p => p.Login == employee.Login) == null)
                {
                    var newAccount = new Account
                    {
                        Login = employee.Login,
                        Password = employee.Password
                    };

                    var newAccountProfile = new AccountProfile
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Department = employee.Department
                    };

                    
                    db.Accounts.Add(newAccount);
                    db.AccountProfiles.Add(newAccountProfile);
                    db.SaveChanges();

                    int id = db.Accounts.FirstOrDefault(p => p.Login == employee.Login).EmployeeId;
                    employee.Id = id;
                    
                    return employee;
                }
                else
                    return null;
            }
            
        }

        public bool SearchDuplicateLogin(string login)
        {
            using (var db = new DataBaseContext())
            {
                Account dup = db.Accounts.FirstOrDefault(p => p.Login == login);

                if (dup != null)
                    return true;
                else
                    return false;
            }
        }
    }
}

