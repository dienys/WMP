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
using System.IO;

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
        private PlayList p;
        private PlayList inUse;
        private int i;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.s = new Search();
            this.p = new PlayList();
            this.inUse = new PlayList();
        }

        void BrowseClick(Object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.Filter = "Films(wmv, avi, mkv, mp4) | *.wmv ; *.avi ; *.mkv ; *.mp4| Musiques(mp3, wmv, wma, MP3) |*.mp3; *.wmv; *.wma; *.MP3 | All(*.*) | *.*";
            openDlg.ShowDialog();
            MediaPathTextBox.Text = openDlg.FileName;
            lecture = openDlg.FileName;
            if (MediaPathTextBox.Text.Length <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Enter a valid media file");
                return;
            }

            string tocompare = ".rblp";
            if (lecture.IndexOf(System.IO.Path.GetExtension(tocompare)) > 0)
            {
                previous.Visibility = System.Windows.Visibility.Visible;
                next.Visibility = System.Windows.Visibility.Visible;
                
                inUse = new PlayList(lecture);
                if (inUse.mediaList.Count != 0)
                {
                    this.i = 1;
                    VideoControl.Source = new Uri(inUse.mediaList[0].path);
                    VideoControl.Play();
                }
                else
                    System.Windows.Forms.MessageBox.Show("The selected playlist \"" + System.IO.Path.GetFileName(lecture) + "\" is empty.");
            }
            else
            {
                inUse.addMedia(lecture);
                VideoControl.Source = new Uri(MediaPathTextBox.Text);
                VideoControl.Play();
            }
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

        private void openplaylist(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.Filter = "Playlist|*.rblp";
            openDlg.ShowDialog();
            p = new PlayList(openDlg.FileName);
            if (openDlg.FileName != "")
            Save.Visibility = System.Windows.Visibility.Visible;
        }

        private void createPlaylist_Click(object sender, RoutedEventArgs e)
        {
            p = new PlayList();
            Save.Visibility = System.Windows.Visibility.Visible;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Playlist|*.rblp";
            sfd.ShowDialog();
            p.save(sfd.FileName);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Films(wmv, avi, mkv, mp4) | *.wmv ; *.avi ; *.mkv ; *.mp4| Musiques(mp3, wmv, wma, MP3) |*.mp3; *.wmv; *.wma; *.MP3 | All(*.*) | *.*";
            openDlg.ShowDialog();
            if (openDlg.FileName != "")
                p.addMedia(openDlg.FileName);
            listPlaylist.Items.Add(openDlg.FileName);
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void VideoControl_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (this.i < inUse.mediaList.Count)
            {
                VideoControl.Source = new Uri(p.mediaList[this.i++].path);
                VideoControl.Play();
            }

        }

       
      
       
    }
}
