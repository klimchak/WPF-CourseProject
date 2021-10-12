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

using MahApps.Metro.Controls;

namespace WpfAppCourseProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Content = new pages.loginPage();
        }
        
    }


    [Serializable]
    public class User
    {
        public string userName { get; set; }
        public string userPass { get; set; }
        public int userProfile { get; set; }
        public User(string usName, string usPass, int usProfile)
        {
            userName = usName;
            userPass = usPass;
            userProfile = usProfile;

        }
        
    }

    [Serializable]
    public class Books
    {
        public int bookCode { get; set; }
        public string bookName { get; set; }
        public string bookAuthor { get; set; }
        public int bookCost { get; set; }

        public Books(int code, string name, string author, int cost)
        {
            bookCode = code;
            bookName = name;
            bookAuthor = author;
            bookCost = cost;
        }

    }

    [Serializable]
    public class Consumer : User
    {
        public int consumerCode { get; set; }
        public string consumerPhone { get; set; }
        public string consumerAddress { get; set; }
        public int consumerBill { get; set; }

        public Consumer(int code, string phone, string address, string usName, string usPass, int usProfile, int bill = 0) : base(usName, usPass, usProfile)
        {
            consumerCode = code;
            consumerPhone = phone;
            consumerAddress = address;
            consumerBill = bill;
        }
    }

    [Serializable]
    public class Order
    {
        public int orderCode { get; set; }
        public int bookCodeDeal { get; set; }
        public string consumerPhoneDeal { get; set; }
        public int quantity { get; set; }
        public int totalCost { get; set; }

        public Order(int ordCode, int bookCode, string phoneDeal, int quant, int totCost)
        {
            orderCode = ordCode;
            bookCodeDeal = bookCode;
            consumerPhoneDeal = phoneDeal;
            quantity = quant;
            totalCost = totCost;
        }
    }


}
