using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MahApps.Metro.Controls;
using Gu.Wpf.NumericInput;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace WpfAppCourseProject.pages
{
    /// <summary>
    /// Логика взаимодействия для loginPage.xaml
    /// </summary>
    public partial class loginPage : Page
    {
        public List<Consumer> dataConsumers = new List<Consumer>();
        public List<User> dataUsers = new List<User>();

        public loginPage()
        {
            InitializeComponent();
            GetAllData(1);
            GetAllData(2);
        }

        private void Login(object sender, RoutedEventArgs e)
        {

            if (loginName.Text == "admin")
            {
                User loginUser = new User(loginName.Text, loginPass.Password, 1);
                try
                {
                    using BinaryReader reader = new BinaryReader(File.Open(loginName.Text, FileMode.Open));
                    // пока не достигнут конец файла
                    // считываем каждое значение из файла
                    string name = "";
                    string pass = "";
                    int profile = 0;
                    while (reader.PeekChar() > -1)
                    {
                        name = reader.ReadString();
                        pass = reader.ReadString();
                        profile = reader.ReadInt32();
                    }
                    if (name == loginUser.userName && pass == loginUser.userPass)
                    {
                        NavigationService.Navigate(new pages.adminPage());
                    }
                    else
                    {
                        MessageBox.Show("Ошибка логина или пароля.");
                    }
                }
                catch
                {
                    using BinaryWriter writer = new BinaryWriter(File.Open(loginName.Text, FileMode.OpenOrCreate));
                    // записываем в файл значение каждого поля структуры
                    writer.Write(loginName.Text);
                    writer.Write(loginPass.Password);
                    writer.Write(1);
                    writer.Close();
                    MessageBox.Show("Файл админа не существовал и создан с введенными данными.");
                }

            }
            else
            {
                
                if (dataUsers.Count() != 0)
                {
                    bool reg = false;
                    for(int i = 0; i < dataUsers.Count(); i++)
                    {
                        if (dataUsers[i].userName == loginName.Text && dataUsers[i].userPass == loginPass.Password)
                        {
                            reg = true;
                            break;
                        }
                    }
                    if (reg == true)
                    {
                        NavigationService.Navigate(new pages.userPage(loginName.Text));
                    }
                    else
                    {
                        MessageBox.Show("Ошибка логина или пароля.");
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не зарегистрирован.");
                }
            }
        }


        private void GetAllData(int mode)
        {
            string fileName = "";
            string fileNameRus = "";

            switch (mode)
            {
                case 1:
                    fileName = "users";
                    fileNameRus = "пользователей";
                    break;
                case 2:
                    fileName = "consumers";
                    fileNameRus = "покупателей";
                    break;
            }

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    switch (mode)
                    {
                        case 1:
                            dataUsers = (List<User>)formatter.Deserialize(fs);
                            break;
                        case 2:
                            dataConsumers = (List<Consumer>)formatter.Deserialize(fs);
                            break;
                    }
                }
            }
            catch
            {
                using BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate));
                writer.Close();
                MessageBox.Show("Файл " + fileNameRus + " не существовал.");
            }
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            Consumer cons = new Consumer(dataConsumers.Count() + 1, regPhone.Text, regAddress.Text, regName.Text, regPass.Password, 2);
            User us = new User(regName.Text, regPass.Password, 2);
            dataUsers.Add(us);
            dataConsumers.Add(cons);
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("users", FileMode.Truncate))
            {
                formatter.Serialize(fs, dataUsers);
            }

            using (FileStream fs = new FileStream("consumers", FileMode.Truncate))
            {
                formatter.Serialize(fs, dataConsumers);
            }

            MessageBox.Show("Регистрация успешно пройдена!");
            dataUsers.Clear();
            dataConsumers.Clear();
            GetAllData(1);
            GetAllData(2);
        }

    }
}
