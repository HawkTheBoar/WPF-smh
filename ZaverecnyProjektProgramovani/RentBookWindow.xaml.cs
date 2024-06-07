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

namespace ZaverecnyProjektProgramovani
{
    /// <summary>
    /// Interaction logic for RentBookWindow.xaml
    /// </summary>
    public partial class RentBookWindow : Window
    {
        public int customer_id = -1;
        public int book_id = -1;
        public RentBookWindow()
        {
            InitializeComponent();
            CustomerStackPanel.Children.Add(ShowCustomers());
        }
        public StackPanel ShowCustomers()
        {
            StackPanel MyStackPanel = new StackPanel();
            var rows = MainWindow.db.ExecuteQuery("SELECT * FROM customer ORDER BY date_joined DESC").Rows;
            
            foreach (DataRow row in rows)
            {
                var name = $"{row["first_name"]} {row["last_name"]}";
                var button = new Button();
                button.Content = $"{name}: {row["birthday"]}";
                button.ContentStringFormat = $"{name} {row["id"]}";
                button.Click += UpdateLabels;
                MyStackPanel.Children.Add(button);
            }
            return MyStackPanel;
        }
        public void UpdateLabels(object sender, RoutedEventArgs e)
        {
            customer_name.Content = (e.Source as Button).ContentStringFormat.ToString().Split(' ').Take((e.Source as Button).ContentStringFormat.ToString().Split(' ').Count() - 1).Aggregate((x, y) => x + " " + y);
            customer_id = Convert.ToInt32((e.Source as Button).ContentStringFormat.ToString().Split(' ')[(e.Source as Button).ContentStringFormat.ToString().Split(' ').Length - 1]);
        }

        private void Rent_Click(object sender, RoutedEventArgs e)
        {
            if(book_id > 0 && customer_id > 0)
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
