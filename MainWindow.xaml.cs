using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Data.Sqlite;
using Path = System.IO.Path;

namespace simplon_todolist_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string sqlitePath =  "Data Source=./bdd/todo.db";
        public MainWindow()
        {
            InitializeComponent();
            this.RunBdd();
            this.ListTask();
        }
        public void RunBdd()
        {
           
            {
             
                var bdd = new SqliteConnection(sqlitePath);
                
                bdd.Open();

                var command = bdd.CreateCommand();

                command.CommandText = @"CREATE TABLE IF NOT EXISTS tasks ( id INT PRIMARY KEY,label TEXT (255) );";
                command.ExecuteReader();

            }
        }

        public void AddTask(object sender, EventArgs e)
        {
            {
                if(newTask.Text.Length <=0)
                {
                    return;
                }

            var bdd = new SqliteConnection(sqlitePath);

            bdd.Open();

            var command = bdd.CreateCommand();
            command.CommandText = @"INSERT INTO tasks (label) VALUES ($label)";
                command.Parameters.AddWithValue("$label", newTask.Text);
                command.ExecuteReader();
                
                listNames.Items.Add(newTask.Text);

            }

        }

        public object ListTask()
        {
            {
                var bdd = new SqliteConnection(sqlitePath);

                bdd.Open();

                var command = bdd.CreateCommand();
                command.CommandText = @"SELECT id,label FROM tasks";


                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(1);
                        listNames.Items.Add(name);
                    }
                }
                return true;
            }
        }

        public void DeleteTask(object sender, EventArgs e)
        {
            newTask.Text = listNames.SelectedItem.ToString();
            var bdd = new SqliteConnection(sqlitePath);

            bdd.Open();

            var command = bdd.CreateCommand();
            command.CommandText = @"DELETE from tasks where label = $label";
            command.Parameters.AddWithValue("$label", listNames.SelectedItem);
            command.ExecuteReader();

            listNames.Items.Remove(listNames.SelectedItem.ToString());
        }
        public void UpdateTask(object sender, EventArgs e)
        {
            {
                if (newTask.Text.Length <= 0)
                {
                    return;
                }

                var bdd = new SqliteConnection(sqlitePath);

                bdd.Open();

                var command = bdd.CreateCommand();
                var query = @"UPDATE tasks SET label=$label where label=$oldlabel";

                command.CommandText = query;
                command.Parameters.AddWithValue("$label", newTask.Text);
                command.Parameters.AddWithValue("$oldlabel", listNames.SelectedItem.ToString());
                command.ExecuteNonQuery();

                listNames.Items.Remove(listNames.SelectedItem);
                listNames.Items.Add(newTask.Text);

            }
        }
    }
}
