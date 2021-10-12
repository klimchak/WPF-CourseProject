using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfAppCourseProject.pages
{
    /// <summary>
    /// Логика взаимодействия для userPage.xaml
    /// </summary>
    public partial class userPage : Page
    {
        public string userName;
        public Consumer consumer;
        public List<Books> dataBooks = new List<Books>();
        public List<Consumer> dataConsumers = new List<Consumer>();
        public List<User> dataUsers = new List<User>();
        public List<Order> dataAllOrders = new List<Order>();
        public List<Order> dataOrders = new List<Order>();
        public userPage(string usName)
        {
            userName = usName;
            InitializeComponent();
            getAllData(1);
            getAllData(2);
            getAllData(3);
            getAllData(4);
        }

        // чтение данных
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
                            dataAllOrders = (List<Order>)formatter.Deserialize(fs);
                            break;
                    }
                }

                if(mode == 3)
                {
                    for (int i = 0; i < dataConsumers.Count(); i++)
                    {
                        if (dataConsumers[i].userName == userName)
                        {
                            consumer = dataConsumers[i];
                            break;
                        }
                    }
                }
                

                if (mode == 1)
                {
                    allBooksTable.ItemsSource = dataBooks;
                    ICollectionView view = CollectionViewSource.GetDefaultView(allBooksTable.ItemsSource);
                    view.Refresh();
                }
                if (mode == 4)
                {
                    for(int i = 0; i < dataAllOrders.Count(); i++)
                    {
                        if(dataAllOrders[i].consumerPhoneDeal == consumer.consumerPhone)
                        {
                            dataOrders.Add(dataAllOrders[i]);
                        }
                    }
                    allOrdersTable.ItemsSource = dataOrders;
                    ICollectionView view = CollectionViewSource.GetDefaultView(allOrdersTable.ItemsSource);
                    view.Refresh();
                }
            }
            catch
            {
                MessageBox.Show("Файл " + fileNameRus + " не существовует.");
            }
        }

        // добавление заказа
        private void CreateOrder(object sender, RoutedEventArgs e)
        {
            int bookCost = 0;

            for (int i = 0; i < dataBooks.Count(); i++)
            {
                if (dataBooks[i].bookCode == int.Parse(intOrderCodeBook.Text))
                {
                    bookCost = dataBooks[i].bookCost;
                    break;
                }
            }
            if (dataAllOrders.Count() == 0)
            {
                Order order = new Order(1, int.Parse(intOrderCodeBook.Text), consumer.consumerPhone, int.Parse(intOrderQuantity.Text), bookCost * int.Parse(intOrderQuantity.Text));
                dataAllOrders.Add(order);
            }
            else
            {
                Order order = new Order(dataAllOrders[dataAllOrders.Count() - 1].orderCode + 1, int.Parse(intOrderCodeBook.Text), consumer.consumerPhone, int.Parse(intOrderQuantity.Text), bookCost * int.Parse(intOrderQuantity.Text));
                dataAllOrders.Add(order);
            }
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("orders", FileMode.Truncate))
            {
                // сериализуем весь массив books
                formatter.Serialize(fs, dataAllOrders);
            }

            for (int i = 0; i < dataConsumers.Count(); i++)
            {
                if (dataConsumers[i].consumerCode == consumer.consumerCode)
                {
                    dataConsumers[i].consumerBill = bookCost * int.Parse(intOrderQuantity.Text);
                    break;
                }
            }

            using (FileStream fs = new FileStream("consumers", FileMode.Truncate))
            {
                formatter.Serialize(fs, dataConsumers);
            }

            MessageBox.Show("Заказ успешно Добавлен!");
            dataConsumers.Clear();
            dataAllOrders.Clear();
            dataOrders.Clear();
            getAllData(3);
            getAllData(4);
        }

         // удаление заказа
        private void DeleteOrder(object sender, RoutedEventArgs e)
        {
            int orderTotalCost = 0;

            for(int i = 0; i < dataAllOrders.Count; i++)
            {
                if (dataAllOrders[i].orderCode == int.Parse(inpDelOrderCode.Text))
                {
                    orderTotalCost = dataAllOrders[i].totalCost;
                    break;
                }
            }

            for(int i = 0; i < dataConsumers.Count; i++)
            {
                if (dataConsumers[i].consumerCode == consumer.consumerCode)
                {
                    dataConsumers[i].consumerBill = dataConsumers[i].consumerBill - orderTotalCost;
                    break;
                }
            }

            List<Order> interimOrder = new List<Order>();
            for (int i = 0; i < dataAllOrders.Count; i++)
            {
                if (dataAllOrders[i].orderCode != int.Parse(inpDelOrderCode.Text))
                {
                    interimOrder.Add(dataAllOrders[i]);
                }
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream("consumers", FileMode.Truncate))
            {
                formatter.Serialize(fs, dataConsumers);
            }

            using (FileStream fs = new FileStream("orders", FileMode.Truncate))
            {
                formatter.Serialize(fs, interimOrder);
            }

            MessageBox.Show("Заказ успешно удален!");
            dataConsumers.Clear();
            dataAllOrders.Clear();
            dataOrders.Clear();
            getAllData(3);
            getAllData(4);
        }

        // регулярка проверки на тип int 
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
