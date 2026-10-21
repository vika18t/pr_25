using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace pr_25
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ObservableCollection<Reminder> reminders = new ObservableCollection<Reminder>();

        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

          
            RemindersGrid.ItemsSource = reminders;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(50);
            timer.Tick += Timer_Tick;
            timer.Start(); 
        }

 
        private void Timer_Tick(object? sender, EventArgs e)
        {
            int h = DateTime.Now.Hour;   
            int m = DateTime.Now.Minute; 

            foreach (Reminder r in reminders)
            {

                if (h == r.Hour && m == r.Minute && r.IsShown == false)
                {
                    r.IsShown = true;

                    MessageBox.Show(r.Text, "Напоминание!");
                }
            }

            RemindersGrid.ItemsSource = null;
            RemindersGrid.ItemsSource = reminders;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int h = Convert.ToInt32(txtHour.Text);
                int m = Convert.ToInt32(txtMinute.Text);
                string text = txtText.Text;

                if (h < 0 || h > 23 || m < 0 || m > 59)
                {
                    MessageBox.Show("Введите корректное время");
                    return;
                }

                if (string.IsNullOrWhiteSpace(text))
                {
                    MessageBox.Show("Введите текст напоминания");
                    return;
                }

                reminders.Add(new Reminder
                {
                    Hour = h,
                    Minute = m,
                    Text = text,
                    IsShown = false
                });

                txtHour.Clear();
                txtMinute.Clear();
                txtText.Clear();
            }
            catch
            {
                MessageBox.Show("Ошибка! Поля часов и минут должны содержать только числа");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (RemindersGrid.SelectedItem is Reminder selectedReminder)
            {
                reminders.Remove(selectedReminder);
            }
            else
            {
                MessageBox.Show("Выберите строку в таблице для удаления!");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt";

            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FileName;

                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (Reminder r in reminders)
                    {
                        
                        writer.WriteLine($"{r.Hour}%%{r.Minute}%%{r.Text}%%{r.IsShown}");
                    }
                }

                MessageBox.Show("Файл сохранён!");
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                string path = dlg.FileName;

                string[] t = File.ReadAllLines(path);

                reminders.Clear(); 

                foreach (string line in t)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] temp = line.Split("%%");

                    if (temp.Length >= 4)
                    {
                        reminders.Add(new Reminder
                        {
                            Hour = Convert.ToInt32(temp[0]),
                            Minute = Convert.ToInt32(temp[1]),
                            Text = temp[2],
                            IsShown = Convert.ToBoolean(temp[3]) 
                        });
                    }
                }

                RemindersGrid.ItemsSource = null;
                RemindersGrid.ItemsSource = reminders;
            }
        }
    }
}