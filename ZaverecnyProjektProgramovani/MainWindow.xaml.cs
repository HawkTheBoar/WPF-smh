using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using ZaverecnyProjektProgramovani;
using System.Configuration;
using System.Diagnostics;
namespace ZaverecnyProjektProgramovani
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        //NpgsqlConnection connection;
        
        
        string connString = "Host=localhost;Username=postgres;Password=12345;Database=Knihovna";
        public static DatabaseHelper db;
        public static WindowManager windowManager;
        public MainWindow()
        {
            InitializeComponent();
            
            db = new DatabaseHelper(connString);
            windowManager = new WindowManager();
            try
            {
             db.InitDB();
                
            }
            catch {}
            

        }

        private void OpenCustomers_Click(object sender, RoutedEventArgs e)
        {
            var window = new CustomersWindow();
            window.Show();
        }

        private void OpenBooks_Click(object sender, RoutedEventArgs e)
        {
            var window = new BooksWindow();
            window.Show();
        }
    }
}

public class WindowManager
{
    private Window[] windows;
    Window currentWindow;
    public WindowManager()
    {
        //currentWindow = Application.Current.MainWindow;
        //windows = new Window[] { new MainWindow(), new CustomersWindow() };
    } 
    public string[] WindowNames()
    {
        return windows.Select(x => x.Title).ToArray();
    }
    public void Redirect(string title)
    {
        for(int i = 0; i < windows.Length; i++)
            if(windows[i].Title == title)
            {
                currentWindow.Hide();
                windows[i].Show();
                currentWindow = windows[i];
            }
        throw new Exception();
    }
    
}
public class DatabaseHelper
{
    private string _connectionString;
    private NpgsqlConnection conn;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
        Connect();
    }
    private void Connect()
    {
        conn = new NpgsqlConnection(_connectionString);
        conn.Open();
    }

    public DataTable ExecuteQuery(string query)
    {
            
        using (var cmd = new NpgsqlCommand(query, conn))
        {
            using (var adapter = new NpgsqlDataAdapter(cmd))
            {
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
        
    }
    public void InitDB()
    {
        ExecuteQuery("CREATE TABLE borrow_evidence (id SERIAL PRIMARY KEY, customer_id SERIAL REFERENCES customer ON DELETE CASCADE, book_id SERIAL REFERENCES book ON DELETE CASCADE, date_borrowed TIMESTAMP DEFAULT CURRENT_TIMESTAMP, date_returned TIMESTAMP)");
        ExecuteQuery("CREATE TABLE customer (first_name TEXT, last_name TEXT, id SERIAL PRIMARY KEY, date_joined TIMESTAMP DEFAUlT CURRENT_TIMESTAMP, birthday TIMESTAMP)");
        ExecuteQuery("CREATE TABLE book (release_date TIMESTAMP, name TEXT, id SERIAL PRIMARY KEY, date_added TIMESTAMP DEFAULT CURRENT_TIMESTAMP)");
    }
}