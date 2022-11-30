using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace FutureApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rnd = new Random();
        private string[] _predictions;
        private readonly string PREDICTIONS_CONFIG_PATH = $"{Environment.CurrentDirectory}\\predictionsConfig.json";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            BackgroundWorker worker= new BackgroundWorker();
            worker.WorkerReportsProgress= true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerAsync();
            try
            {
                var date = File.ReadAllText(PREDICTIONS_CONFIG_PATH);
                _predictions = JsonConvert.DeserializeObject < string[]>(date);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(_predictions == null)
                {
                    Close();
                }
                else if(_predictions.Length == 0) 
                {
                    MessageBox.Show("Комплименты кончились, извиняюсь");
                    Close();
                }
            }
        }       
        private void worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            for(int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(20);
            }
            var index = rnd.Next(_predictions.Length);
            var prediction = _predictions[index];
            MessageBox.Show($"{prediction}!");
            button1.IsEnabled = true;
        }
        private void worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;  
        }
    }
}
