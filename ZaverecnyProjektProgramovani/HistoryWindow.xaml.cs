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
using System.Windows.Shapes;
using System.Data;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
namespace ZaverecnyProjektProgramovani
{
    /// <summary>
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        //public int book_id = -1;
        //public int customer_id = -1;
        //public string thing_name;
        public HistoryWindow(string thing_name, int customer_id, int book_id)
        {
            InitializeComponent();
            historyLabel.Content = $"{thing_name}'s History";
            Trace.WriteLine($"name: {thing_name}");
            Trace.WriteLine($"{book_id} {customer_id}");
            DataRowCollection data = null;
            if (book_id >= 0)
            data = MainWindow.db.ExecuteQuery($"SELECT * FROM borrow_evidence INNER JOIN customer ON borrow_evidence.customer_id = customer.id INNER JOIN book ON borrow_evidence.book_id = book.id WHERE book.id = {book_id}").Rows;
            else if (customer_id >= 0)
                data = MainWindow.db.ExecuteQuery($"SELECT * FROM borrow_evidence INNER JOIN customer ON borrow_evidence.customer_id = customer.id INNER JOIN book ON borrow_evidence.book_id = book.id WHERE customer.id = {customer_id}").Rows; ;
            
            Trace.WriteLine(data.Count);

            if (data != null)
                foreach (DataRow row in data)
                {
                    string thing = "";
                    string date_borrowed = "";
                    string date_returned = "";
                    if (customer_id < 0)
                        thing = row["first_name"] + row["last_name"].ToString();
                    else if (book_id < 0)
                        thing = row["name"].ToString();
                    date_returned = row["date_returned"].ToString();
                    date_borrowed = row["date_borrowed"].ToString();

                    var panel = new TabPanel();
                    
                    panel.Children.Add(new Label() { Content=$"{thing}", HorizontalContentAlignment = HorizontalAlignment.Center, Width=650/3 });
                    panel.Children.Add(new Label() { Content = $"{date_borrowed}", HorizontalContentAlignment = HorizontalAlignment.Center, Width = 650 / 3 });
                    panel.Children.Add(new Label() { Content = $"{(date_returned == "" ? "Not Returned":$"{date_returned}")}", HorizontalContentAlignment = HorizontalAlignment.Center, Width = 650 / 3 });
                    MyStackPanel.Children.Add(panel);
                }
            else
                MyStackPanel.Children.Add(new Label() { Content = "No available history for this book!"});
        }
    }
}
