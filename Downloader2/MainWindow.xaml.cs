using System.IO;
using System.Net;
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

namespace DownLoader
{

   


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ///<summary>
        ///
        /// </summary>


        public string DownloadedString { get; set; }


        ///<summary>
        ///Event start when program downloaded string from website
        /// </summary>

        public event Action<string> StringDownloaded = (x) => { };


        ///<summary>
        ///Event start when program got file name From User
        /// </summary>
        public event Action<string,string> FileNameProvided = (x,y) => { };


        ///<summary>
        ///Default constuctor
        /// </summary>

        public MainWindow()
        {
           


            InitializeComponent();

            StringDownloaded += (x) => SetControlStateAfterDowload();
            StringDownloaded += (x) => DownloadedString = x;
            StringDownloaded += (x) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DisplayText.Text = "Podaj nazwe pliku";

                    MessageBox.Show("Podaj nazwe pilku");
                });
            };

            FileNameProvided += SaveToFile;
            FileNameProvided += (x,y) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("zapisano plik");
                });
            };

            FileNameBox.Visibility  = Visibility.Hidden;
        }

 

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DownloadedString != null)
            {
                FileNameProvided.Invoke(FileNameBox.Text,DownloadedString);
                return;
            }

            var currentUrl = WebsiteURL.Text;

            await Task.Run(async () =>

            {
                var webClient = new WebClient();

   

                var downloadedString = await webClient.DownloadStringTaskAsync(currentUrl);



                StringDownloaded.Invoke(downloadedString);

            });

        }

        private void SaveToFile(string fileName, string downloadedString)
        {
            File.WriteAllText(fileName, downloadedString);
        }

        private void SetControlStateAfterDowload()
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                WebsiteURL.Visibility = Visibility.Hidden;
                FileNameBox.Visibility = Visibility.Visible;

                SumbitButton.Content = "Click so save";
            });
         
        }
    }
}