using System;
using System.Collections.Generic;
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
using System.Data;
using System.Diagnostics;

namespace ZaverecnyProjektProgramovani
{
    /// <summary>
    /// Interaction logic for BooksWindow.xaml
    /// </summary>
    public partial class BooksWindow : Window
    {
        public BooksWindow()
        {
            InitializeComponent();
            LoadData();
        }
        void LoadData()
        {
            MyStackPanel.Children.Clear();
            var rows = MainWindow.db.ExecuteQuery("SELECT * FROM book").Rows;
            foreach (DataRow row in rows)
            {
                string book_title = row["name"].ToString();
                string book_release_date = row["release_date"].ToString();
                int book_id = Convert.ToInt32(row["id"].ToString());
                var panel = new TabPanel();
                var label = new Label();
                label.Content = book_title;
                label.Width = 169;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                var label2 = new Label();
                label2.Content = book_release_date;
                label2.Width = 169;
                label2.HorizontalContentAlignment = HorizontalAlignment.Center;
                panel.Children.Add(label);
                panel.Children.Add(label2);
                MyStackPanel.Children.Add(panel);
                var Actionpanel = new TabPanel();
                var evidenceRows = MainWindow.db.ExecuteQuery($"SELECT borrow_evidence.date_returned, borrow_evidence.date_borrowed, borrow_evidence.book_id, book.id FROM borrow_evidence INNER JOIN book ON book.id = borrow_evidence.book_id WHERE book.id = {book_id} ORDER BY borrow_evidence.date_borrowed DESC;").Rows;
                bool is_returned = true;
                if (evidenceRows.Count > 0)
                {
                    var evidence_row = evidenceRows[0];
                    var date_returned = evidence_row["date_returned"];
                    is_returned = date_returned != DBNull.Value;
                    //Trace.WriteLine(is_returned);
                }
               
                var rentOut = new Button();
                rentOut.Content = is_returned ? "Borrow" : "Return";
                rentOut.ContentStringFormat = $"{book_title} {book_id}"; // lol uplne hack
                if (is_returned)
                    rentOut.Click += RentBook;
                else
                    rentOut.Click += ReturnBook;
                var history = new Button();
                history.Content = "History";
                history.ContentStringFormat = $"{book_title} {book_id}"; // lol uplne hack
                history.Click += ShowHistory;

                var delete = new Button();
                delete.Content = "Delete";
                delete.ContentStringFormat = $"{book_title} {book_id}"; // lol uplne hack
                delete.Click += DeleteBook;
                new List<Button> { rentOut, history, delete }.ForEach(x =>
                {
                    x.Width = 320 / 3;
                    Actionpanel.Children.Add(x);
                });
                MyStackPanel.Children.Add(Actionpanel);
            }
        }
        private void RentBook(object sender, RoutedEventArgs e)
        {
            var dialogueWindow = new RentBookWindow();
            dialogueWindow.book_title.Content = (e.Source as Button).ContentStringFormat.ToString().Split(' ').Take((e.Source as Button).ContentStringFormat.ToString().Split(' ').Count() - 1).Aggregate((x, y) => x + " " + y);
            dialogueWindow.book_id = Convert.ToInt32((e.Source as Button).ContentStringFormat.ToString().Split(' ')[(e.Source as Button).ContentStringFormat.ToString().Split(' ').Length - 1]);
            if (dialogueWindow.ShowDialog() == true)
            {
                MainWindow.db.ExecuteQuery($"INSERT INTO borrow_evidence (customer_id, book_id) VALUES ({dialogueWindow.customer_id}, {dialogueWindow.book_id})");
                LoadData();
            }
        }
        private void ReturnBook(object sender, RoutedEventArgs e)
        {
            MainWindow.db.ExecuteQuery($"UPDATE borrow_evidence SET date_returned = CURRENT_TIMESTAMP WHERE book_id = {Convert.ToInt32((e.Source as Button).ContentStringFormat.ToString().Split(' ')[(e.Source as Button).ContentStringFormat.ToString().Split(' ').Length - 1])}");
            LoadData();
        }
        private void DeleteBook(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show($"Are you sure you want to delete {(e.Source as Button).ContentStringFormat.ToString().Split(' ').Take((e.Source as Button).ContentStringFormat.ToString().Split(' ').Count() - 1).Aggregate((x, y) => x + " " + y)}?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                MainWindow.db.ExecuteQuery($"DELETE FROM book WHERE id = {Convert.ToInt32((e.Source as Button).ContentStringFormat.ToString().Split(' ')[(e.Source as Button).ContentStringFormat.ToString().Split(' ').Length - 1])}");
            }
            LoadData();
        }
        private void AddBook(object sender, RoutedEventArgs e)
        {
            var book_name = add_book_title.Text;
            var date_released = add_date_released.Text;
            if (book_name.Length > 0 && date_released.Length > 0)
                    MainWindow.db.ExecuteQuery("INSERT INTO book (name, release_date)" +
                    $" VALUES (\'{book_name}\', \'{date_released}\')");
            LoadData();
                
        }
        private void ShowHistory(object sender, RoutedEventArgs e)
        {
           
            var book_id = Convert.ToInt32((e.Source as Button).ContentStringFormat.ToString().Split(' ')[(e.Source as Button).ContentStringFormat.ToString().Split(' ').Length - 1]);
            //window.thing_name = (e.Source as Button).ContentStringFormat.ToString().Split(' ').Take((e.Source as Button).ContentStringFormat.ToString().Split(' ').Count() - 1).Aggregate((x, y) => x + " " + y);
            var thing_name = (e.Source as Button).ContentStringFormat.ToString().Split(' ').Take((e.Source as Button).ContentStringFormat.ToString().Split(' ').Count() - 1).Aggregate((x, y) => x + " " + y);
            var window = new HistoryWindow(thing_name, -1, book_id);
            window.Show();
    }
    }
}
