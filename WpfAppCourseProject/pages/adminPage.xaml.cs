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
using MahApps.Metro.Controls;
using Gu.Wpf.NumericInput;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;

namespace WpfAppCourseProject.pages
{
    /// <summary>
    /// Логика взаимодействия для adminPage.xaml
    /// </summary>
    public partial class adminPage : Page
    {
        public int quantitiBooks = 0;
        public List<Books> dataBooks = new List<Books>();
        public List<Consumer> dataConsumers = new List<Consumer>();
        public List<User> dataUsers = new List<User>();
        public List<Order> dataOrders = new List<Order>();
        public adminPage()
        {
            InitializeComponent();
            getAllData(1);
            getAllData(2);
            getAllData(3);
            getAllData(4);
        }

        // чтение книг
        public void getAllData(int mode)
        {
            string fileName = "";
            string fileNameRus = "";

            switch (mode)
            {
                case 1:
                    fileName = "books";
                    fileNameRus = "книг";
                    break;
                case 2:
                    fileName = "users";
                    fileNameRus = "пользователей";
                    break;
                case 3:
                    fileName = "consumers";
                    fileNameRus = "покупателей";
                    break;
                case 4:
                    fileName = "orders";
                    fileNameRus = "заказов";
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
                            dataBooks = (List<Books>)formatter.Deserialize(fs);
                            break;
                        case 2:
                            dataUsers = (List<User>)formatter.Deserialize(fs);
                            break;
                        case 3:
                            dataConsumers = (List<Consumer>)formatter.Deserialize(fs);
                            break;
                        case 4:
                            dataOrders = (List<Order>)formatter.Deserialize(fs);
                            break;
                    }
                }

                if(mode == 1)
                {
                    allBooksTable.ItemsSource = dataBooks;
                    quantitiBooks = dataBooks.Count;
                    ICollectionView view = CollectionViewSource.GetDefaultView(allBooksTable.ItemsSource);
                    view.Refresh();
                }
                if(mode == 3)
                {
                    allConsumersTable.ItemsSource = dataConsumers;
                    ICollectionView view = CollectionViewSource.GetDefaultView(allConsumersTable.ItemsSource);
                    view.Refresh();
                }
                if(mode == 4)
                {
                    allOrdersTable.ItemsSource = dataOrders;
                    ICollectionView view = CollectionViewSource.GetDefaultView(allOrdersTable.ItemsSource);
                    view.Refresh();
                }
            }
            catch
            {
                using BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate));
                writer.Close();
                MessageBox.Show("Файл " + fileNameRus + " не существовал.");
            }
        }
        
        // добавление книги
        private void AddBook(object sender, RoutedEventArgs e)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            Books book = new Books(quantitiBooks + 1, inpBookName.Text, inpBookAuthor.Text, int.Parse(inpBookCost.Text));
            dataBooks.Add(book);
            using (FileStream fs = new FileStream("books", FileMode.Truncate))
            {
                // сериализуем весь массив books
                formatter.Serialize(fs, dataBooks);
            }
                       
            MessageBox.Show("Книга успешно добавлена!");
            dataBooks.Clear();
            getAllData(1);
        }

        // изменение книги
        private void EditBook(object sender, RoutedEventArgs e) {
            for(int i = 0; i < dataBooks.Count; i++)
            {
                if(dataBooks[i].bookCode == int.Parse(inpEditCode.Text))
                {
                    dataBooks[i].bookCode = int.Parse(inpEditCode.Text);
                    dataBooks[i].bookName = inpEditName.Text;
                    dataBooks[i].bookAuthor = inpEditAuthor.Text;
                    dataBooks[i].bookCost = int.Parse(inpEditCost.Text);
                    break;
                }
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("books", FileMode.Truncate))
            {
                // сериализуем весь массив books
                formatter.Serialize(fs, dataBooks);
            }
            
            MessageBox.Show("Книга успешно изменена!");
            dataBooks.Clear();
            getAllData(1);
        }

        // удаление книги
        private void DeleteBook(object sender, RoutedEventArgs e) {

            List<Books> interimBooks = new List<Books>();
            for (int i = 0; i < dataBooks.Count; i++)
            {
                if (dataBooks[i].bookCode != int.Parse(inpDeleteCode.Text))
                {
                    interimBooks.Add(dataBooks[i]);
                }
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("books", FileMode.Truncate))
            {
                // сериализуем весь массив books
                formatter.Serialize(fs, interimBooks);
            }

            MessageBox.Show("Книга успешно удалена!");
            dataBooks.Clear();
            getAllData(1);
        }

        // удаление покупателя и пользователя
        private void DeleteConsumerAndUser(object sender, RoutedEventArgs e) {

            List<Consumer> interimConsumers = new List<Consumer>();
            List<User> interimUsers = new List<User>();
            for (int i = 0; i < dataConsumers.Count; i++)
            {
                if (dataConsumers[i].consumerCode == int.Parse(inpDelConsumerCode.Text))
                {
                    for (int j = 0; j < dataUsers.Count; j++)
                    {
                        if (dataConsumers[i].userName != dataUsers[j].userName)
                        {
                            interimUsers.Add(dataUsers[j]);
                        }
                    }
                    break;
                }
            }
            for (int i = 0; i < dataConsumers.Count; i++)
            {
                if (dataConsumers[i].consumerCode != int.Parse(inpDelConsumerCode.Text))
                {
                    interimConsumers.Add(dataConsumers[i]);
                }
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("users", FileMode.Truncate))
            {
                // сериализуем весь массив books
                formatter.Serialize(fs, interimUsers);
            }
            using (FileStream fs = new FileStream("consumers", FileMode.Truncate))
            {
                // сериализуем весь массив books
                formatter.Serialize(fs, interimConsumers);
            }

            MessageBox.Show("Покупатель успешно удален!");
            dataUsers.Clear();
            dataConsumers.Clear();
            getAllData(2);
            getAllData(3);
        }

        // удаление заказа
        private void DeleteOrder(object sender, RoutedEventArgs e) {

            List<Order> interimOrder = new List<Order>();
            for (int i = 0; i < dataOrders.Count; i++)
            {
                if (dataOrders[i].orderCode != int.Parse(inpDelOrderCode.Text))
                {
                    interimOrder.Add(dataOrders[i]);
                }
            }

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("orders", FileMode.Truncate))
            {
                // сериализуем весь массив books
                formatter.Serialize(fs, interimOrder);
            }

            MessageBox.Show("Заказ успешно удален!");
            dataBooks.Clear();
            getAllData(4);
        }

        // регулярное выражение проверки на тип int 
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


    }
}
