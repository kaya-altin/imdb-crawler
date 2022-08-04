using ImdbCrawler.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
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

namespace ImdbCrawler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;

        //List<Core.Movie> movies = new List<Core.Movie>();
        private readonly BackgroundWorker worker = new BackgroundWorker();
        int imdbId = 0;
        int imdbIdEnd = 0;
        Movie movie;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;

            txtImdbIdStart.Text = Properties.Settings.Default.ImdbId.ToString();

            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Database\\db.mdb";
        }



        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!worker.IsBusy)
            {
                if (String.IsNullOrEmpty(txtImdbIdStart.Text) | String.IsNullOrEmpty(txtImdbIdEnd.Text))
                {
                    MessageBox.Show("ImdbId boş bırakılamaz.!");
                    return;
                }

                imdbId = Util.convertToInt(txtImdbIdStart.Text);
                imdbIdEnd = Util.convertToInt(txtImdbIdEnd.Text);

                if (imdbId == 0 | imdbIdEnd == 0)
                {
                    MessageBox.Show("ImdbId 0 olamaz!");
                    return;
                }

                if (imdbId > imdbIdEnd)
                {
                    MessageBox.Show("Hatalı giriş!");
                    return;
                }


                progressBar.Minimum = imdbId;
                progressBar.Maximum = imdbIdEnd;

                txtImdbIdStart.IsEnabled = false;
                txtImdbIdEnd.IsEnabled = false;

                btnStart.Content = "Durdur";
                worker.RunWorkerAsync();
            }
            else
            {
                worker.CancelAsync();
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();



                for (int i = imdbId; i < imdbIdEnd; i++)
                {
                    if (worker.CancellationPending)
                    {
                        break;
                    }

                    movie = new Movie.Parse.Imdb(new Core.Movie.Search.SearchResult("tt" + imdbId.ToString("D7")), true);
                    if (movie != null)
                    {

                        using (OleDbCommand cmd = con.CreateCommand())
                        {
                            cmd.CommandText =
                               "INSERT INTO Movies " +
                               "([MovieName], [ReleaseDate], [Director], [Writer], [ImdbNumber]) " +
                               "VALUES(@MovieName, @ReleaseDate, @Director, @Writer, @ImdbNumber)";


                            cmd.Parameters.AddRange(new OleDbParameter[]
                            {
                           new OleDbParameter("@MovieName", movie.OrginalName),
                           new OleDbParameter("@ReleaseDate", movie.Year),
                           new OleDbParameter("@Director", movie.Director),
                           new OleDbParameter("@Writer", movie.Writer),
                           new OleDbParameter("@ImdbNumber", movie.ImdbNumber),
                           });

                            cmd.ExecuteNonQuery();
                        }


                        worker.ReportProgress(imdbId);
                    }
                    //movies.Add(movie);
                    imdbId++;
                }
                Debug.WriteLine("worker_DoWork");
            }
            catch (Exception)
            {
                Properties.Settings.Default.ImdbId = imdbId;
                Properties.Settings.Default.Save();

                txtImdbIdStart.IsEnabled = true;
                txtImdbIdEnd.IsEnabled = true;
                btnStart.Content = "Başlat";
            }
          
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Properties.Settings.Default.ImdbId = imdbId;
            Properties.Settings.Default.Save();

            txtImdbIdStart.Text = imdbId.ToString("D7");
            imdbId = 0;

            if (e.Cancelled)
            {

            }
            else
            {

            }


            txtImdbIdStart.IsEnabled = true;
            txtImdbIdEnd.IsEnabled = true;
            btnStart.Content = "Başlat";

            Debug.WriteLine("worker_RunWorkerCompleted");
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblState.Content = movie.OrginalName + " - (" + movie.ImdbNumber + ") tamamlandı." + Environment.NewLine;
            progressBar.Value = e.ProgressPercentage + 1;
            //Debug.WriteLine(e.ProgressPercentage);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
