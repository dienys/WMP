using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace InterfaceWindowMediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private bool userIsDraggingSlider = false;
        string lecture;
        private Search s;
       

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.s = new Search();
        }

        void BrowseClick(Object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.Filter = "Films(wmv, avi, mkv, mp4) | *.wmv ; *.avi ; *.mkv ; *.mp4| Musiques(mp3, wmv, wma, MP3) |*.mp3; *.wmv; *.wma; *.MP3 | All(*.*) | *.*";
            openDlg.ShowDialog();
            MediaPathTextBox.Text = openDlg.FileName;
            lecture = openDlg.FileName;
            if (MediaPathTextBox.Text.Length
     <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Enter a valid media file");
                return;
            }
            VideoControl.Source = new Uri(MediaPathTextBox.Text);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((VideoControl.Source != null) && (VideoControl.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            {
                timelineSlider.Minimum = 0;
                timelineSlider.Maximum = VideoControl.NaturalDuration.TimeSpan.TotalSeconds;
                timelineSlider.Value = VideoControl.Position.TotalSeconds;
            }
        }

        void PlayClick(object sender, EventArgs e)
        {

            VideoControl.Play();
        }
        void PauseClick(object sender, EventArgs e)
        {
            VideoControl.Pause();
        }
        void StopClick(object sender, EventArgs e)
        {
            VideoControl.Stop();
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void VideoControl_MediaOpened(object sender, RoutedEventArgs e)
        {
        }
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            VideoControl.Position = new TimeSpan(0);
        }
        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            VideoControl.Position = TimeSpan.FromSeconds(timelineSlider.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(timelineSlider.Value).ToString(@"hh\:mm\:ss");
        }

       

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           FolderBrowserDialog openDlg = new FolderBrowserDialog();
           openDlg.ShowDialog();

           
           
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
