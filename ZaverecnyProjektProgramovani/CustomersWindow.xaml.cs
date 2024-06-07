using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace ZaverecnyProjektProgramovani
{
    /// <summary>
    /// Interaction logic for CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow : Window
    {
        public CustomersWindow()
        {
            InitializeComponent();
            LoadData();
        }
        void LoadData()
        {
            CustomerStackPanel.Children.Clear();
            CustomerStackPanel.Children.Add(ShowCustomers());
        }
        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            var birthday = add_birthday.Text;
            var first_name = add_first_name.Text;
            var last_name = add_last_name.Text;
            if (last_name.Length > 0 && first_name.Length > 0 && birthday.Length > 0)
                MainWindow.db.ExecuteQuery("INSERT INTO customer (first_name, last_name, birthday)" +
                    $" VALUES (\'{first_name}\', \'{last_name}\', \'{birthday}\')");
            LoadData();

        }
        public StackPanel ShowCustomers()
        {
            StackPanel MyStackPanel = new StackPanel();
            var rows = MainWindow.db.ExecuteQuery("SELECT * FROM customer ORDER BY date_joined DESC").Rows;
            var tabpanel = new TabPanel();
            tabpanel.Children.Add(new Label() { Content = "Name", Width = 160, HorizontalContentAlignment=HorizontalAlignment.Center});
            tabpanel.Children.Add(new Label() { Content = "Birthday", Width = 160, HorizontalContentAlignment = HorizontalAlignment.Center });
            MyStackPanel.Children.Add(tabpanel);
            foreach (DataRow row in rows)
            {
                var customer_name = row["first_name"].ToString() + " " + row["last_name"];
                var customer_id = Convert.ToInt32(row["id"]);
                var panel = new TabPanel();
                var label = new Label();
                label.Content = customer_name;
                label.MinWidth = 160;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                var label2 = new Label();
                label2.Content = row["birthday"];
                label2.MinWidth = 160;
                label2.HorizontalContentAlignment = HorizontalAlignment.Center;
                panel.Children.Add(label);
                panel.Children.Add(label2);
                MyStackPanel.Children.Add(panel);

                var Actionpanel = new TabPanel();

                var history = new Button();
                history.Content = "History";
                history.ContentStringFormat = $"{customer_name} {customer_id}";
                history.Click += ShowHistory;
                var delete = new Button();
                delete.Content = "Delete";
                delete.ContentStringFormat = $"{customer_name} {customer_id}";
                delete.Click += DeleteCustomer;
                new List<Button> { history, delete }.ForEach(x =>
                {
                    x.Width = 320 / 2;
                    Actionpanel.Children.Add(x);
                });
                MyStackPanel.Children.Add(Actionpanel);
                
            }
            return MyStackPanel;
        }
        private void DeleteCustomer(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"Are you sure you want to delete {(e.Source as Button).ContentStringFormat.ToString().Split(' ').Take((e.Source as Button).ContentStringFormat.ToString().Split(' ').Count() - 1).Aggregate((x, y) => x + " " + y)}?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                MainWindow.db.ExecuteQuery($"DELETE FROM customer WHERE id = {Convert.ToInt32((e.Source as Button).ContentStringFormat.ToString().Split(' ')[(e.Source as Button).ContentStringFormat.ToString().Split(' ').Length - 1])}");
            }
            LoadData();
        }
        private void ShowHistory(object sender, RoutedEventArgs e)
        {
            var customer_id = Convert.ToInt32((e.Source as Button).ContentStringFormat.ToString().Split(' ')[(e.Source as Button).ContentStringFormat.ToString().Split(' ').Length - 1]);
            var thing_name = (e.Source as Button).ContentStringFormat.ToString().Split(' ').Take((e.Source as Button).ContentStringFormat.ToString().Split(' ').Count() - 1).Aggregate((x, y) => x + " " + y);
            var window = new HistoryWindow(thing_name, customer_id, -1);
            window.Show();
        }
    }
}
