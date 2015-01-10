﻿using System;
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
        private PlayList p;
        private PlayList inUse;
        private Library lib;
        private int index;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.s = new Search();
            this.p = new PlayList();
            this.inUse = new PlayList();
            this.lib = new Library();
            this.index = 0;

            foreach (string elem in lib.FolderPath)
                MenuLibrary.Items.Add(elem);
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.Filter = "Video (*.wmv, *.avi, *.mp4)|*.wmv;*.avi;*.mp4|Audio (*.mp3, *.wmv, *.wma)|*.mp3;*.wmv;*.wma;|Image (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Playlist (*.rblp)|*.rblp|All|*.mp3;*.wmv;*.wma;*.MP3;*.wmv ;*.avi;*.mp4;*.jpg;*.jpeg;*.png;*.bmp";
            openDlg.ShowDialog();
            MediaPathTextBox.Text = openDlg.FileName;
            lecture = openDlg.FileName;
            if (MediaPathTextBox.Text.Length <= 0)
            {
                System.Windows.Forms.MessageBox.Show("Enter a valid media file");
                return;
            }

            if (System.IO.Path.GetExtension(lecture) == ".rblp")
            {
                
                inUse = new PlayList(lecture);
                listplay.Items.Clear();
                if (inUse.mediaList.Count != 0)
                {
                    this.index = 0;
                    VideoControl.Source = new Uri(inUse.mediaList[this.index++].path);
                    PlayButton.Content = "Pause";
                    VideoControl.Play();
                    foreach (Media elem in inUse.mediaList) 
                      listplay.Items.Add(elem.name);
                }
                else
                    System.Windows.Forms.MessageBox.Show("The selected playlist \"" + System.IO.Path.GetFileName(lecture) + "\" is empty.");
            }
            else
            {
                inUse.addMedia(lecture);
                this.index = this.inUse.mediaList.Count;
                listplay.Items.Add(inUse.mediaList[this.index - 1].name);
                VideoControl.Source = new Uri(MediaPathTextBox.Text);
                PlayButton.Content = "Pause";
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
            if (PlayButton.Content == "Play")
            {
                PlayButton.Content = "Pause";
                VideoControl.Play();
            }
            else
            {
                PlayButton.Content = "Play";
                VideoControl.Pause();
            }
        }

        void StopClick(object sender, EventArgs e)
        {
            VideoControl.Stop();
            VideoControl.Close();
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
            listPlaylist.Items.Clear();
            openDlg.Filter = "Playlist (*.rblp)|*.rblp";
            openDlg.ShowDialog();
            p = new PlayList(openDlg.FileName);
            if (openDlg.FileName != "")
            {
                Save.Visibility = System.Windows.Visibility.Visible;
                foreach (Media elem in p.mediaList)
                    listPlaylist.Items.Add(elem.name);
            }
        }

      
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Playlist (*.rblp)|*.rblp";
            sfd.ShowDialog();
            p.save(sfd.FileName);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Video(wmv, avi, mkv, mp4)|*.wmv ;*.avi;*.mp4|Audio(mp3, wmv, wma, MP3)|*.mp3;*.wmv;*.wma;*.MP3|Picture (jpg, png, jpeg, bmp)|*.jpg;*.jpeg;*.png;*.bmp|All|*.*";
            openDlg.ShowDialog();
            if (openDlg.FileName != "")
                p.addMedia(openDlg.FileName);
            listPlaylist.Items.Add(openDlg.FileName);
        }

        private void VideoControl_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (loopButton.Content == "Loop: Media")
            {
                VideoControl.Close();
                VideoControl.Source = new Uri((inUse.mediaList[this.index - 1]).path);
                VideoControl.Play();
                return;
            }

            if (this.index < inUse.mediaList.Count)
            {
                VideoControl.Source = new Uri((inUse.mediaList[this.index]).path);
                VideoControl.Play();
            }
            else
            {
                if (loopButton.Content == "Loop: Playlist")
                {
                    this.index = 0;
                    VideoControl.Source = new Uri((inUse.mediaList[this.index]).path);
                    VideoControl.Play();
                }
                else
                    VideoControl.Close();
            }
            if (this.index < inUse.mediaList.Count + 1)
                this.index += 1;
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (loopButton.Content == "Loop: Media")
            {
                VideoControl.Close();
                VideoControl.Source = new Uri((inUse.mediaList[this.index - 1]).path);
                VideoControl.Play();
                return;
            }

            if (this.index < inUse.mediaList.Count)
            {
                VideoControl.Source = new Uri((inUse.mediaList[this.index]).path);
                VideoControl.Play();
            }
            else
            {
                if (loopButton.Content == "Loop: Playlist")
                {
                    this.index = 0;
                    VideoControl.Source = new Uri((inUse.mediaList[this.index]).path);
                    VideoControl.Play();
                }
                else
                    VideoControl.Close();
            }

            if (this.index < inUse.mediaList.Count + 1)
                this.index += 1;
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if (loopButton.Content == "Loop: Media")
            {
                VideoControl.Close();
                VideoControl.Source = new Uri((inUse.mediaList[this.index - 1]).path);
                VideoControl.Play();
                return;
            }

            if (this.index > 1)
            {
                this.index -= 2;
                VideoControl.Source = new Uri((inUse.mediaList[this.index]).path);
                VideoControl.Play();
                this.index += 1;
            }
            else
            {
                if (loopButton.Content == "Loop: Playlist")
                {
                    this.index = inUse.mediaList.Count - 1;
                    VideoControl.Source = new Uri((inUse.mediaList[this.index]).path);
                    VideoControl.Play();
                    this.index += 1;
                }
                else
                    VideoControl.Close();
            }

        }

        private void loopButton_Click(object sender, RoutedEventArgs e)
        {
            if (loopButton.Content == "Loop: None")
                loopButton.Content = "Loop: Playlist";
            else if (loopButton.Content == "Loop: Playlist")
                loopButton.Content = "Loop: Media";
            else
                loopButton.Content = "Loop: None";
        }

        private void volumeUpButton_Click(object sender, RoutedEventArgs e)
        {
            double volume;

            if (VideoControl.Volume < 1)
                VideoControl.Volume += 0.1;
            volume = VideoControl.Volume * 100;
            if (volume > 90)
            {
                VideoControl.Volume = 1;
                volume = 100;
            }
            volumeText.Text = "Volume: " + volume.ToString() + "%";
        }

        private void volumeDownButton_Click(object sender, RoutedEventArgs e)
        {
            double volume;

            if (VideoControl.Volume > 0)
                VideoControl.Volume -= 0.1;
            volume = VideoControl.Volume * 100;
            if (volume < 10)
            {
                VideoControl.Volume = 0;
                volume = 0;
            }
            volumeText.Text = "Volume: " + volume.ToString() + "%";
        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "")
                lib.addFolder(fbd.SelectedPath);
            MenuLibrary.Items.Clear();
            foreach (string elem in lib.FolderPath)
            {
                MenuLibrary.Items.Add(elem);
            }
        }

        private void MenuLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LibrarylistView.Items.Clear();
            if (MenuLibrary.SelectedIndex != -1)
            {
                foreach (Media elem in lib.LibraryList[MenuLibrary.SelectedIndex].mediaList)
                {
                    LibrarylistView.Items.Add(elem.name);
                }
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            lib.refresh(MenuLibrary.SelectedIndex);
            LibrarylistView.Items.Clear();
            if (index != -1)
            {
                foreach (Media elem in lib.LibraryList[MenuLibrary.SelectedIndex].mediaList)
                {
                    LibrarylistView.Items.Add(elem.name);
                }
            }
        }

        private void fullscreenmouse(object sender, RoutedEventArgs e)
        {
            Window tmp = new Window();
            VideoControl.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            VideoControl.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
           tmp.Content = VideoControl;
           
        //this.WindowStyle = WindowStyle.None;
            
            tmp.WindowState = WindowState.Maximized;
           
            
            
            

        }

        private void resize(object sender, RoutedEventArgs e)
        {

            VideoControl.Width = 615;
            //VideoControl.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Content = VideoControl;

        //    this.WindowStyle = WindowStyle.None;

            this.WindowState = WindowState.Normal;
        }

        private void playMedia(object sender, RoutedEventArgs e)
        {
            Media tmp = lib.LibraryList[MenuLibrary.SelectedIndex].mediaList[LibrarylistView.SelectedIndex];
            if (inUse.mediaList.Count == 0)
                this.index = 1;
            if (System.IO.Path.GetExtension(tmp.path) == ".rblp")
            {
                this.inUse = new PlayList(tmp.path);
                this.index = 1;
                VideoControl.Source = new Uri(inUse.mediaList[index - 1].path);
            }
            else
            {
                this.inUse.addMedia(tmp);
                if (inUse.mediaList.Count == 1)
                    VideoControl.Source = new Uri(inUse.mediaList[index - 1].path); 
            }
            listplay.Items.Clear();
            foreach (Media elem in inUse.mediaList)
                listplay.Items.Add(elem.name);
        }

        private void PlaySelectedMedia(object sender, RoutedEventArgs e)
        {
            this.index = listplay.SelectedIndex + 1;
            VideoControl.Source = new Uri(inUse.mediaList[index - 1].path);
            VideoControl.Play();
            PlayButton.Content = "Pause";
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            if (listPlaylist.SelectedIndex != -1)
                p.mediaList.RemoveAt(listPlaylist.SelectedIndex);
            listPlaylist.Items.Clear();
            foreach (Media elem in p.mediaList)
                listPlaylist.Items.Add(elem.name);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (p.path != "")
                p.delete();
            listPlaylist.Items.Clear();
            inUse = new PlayList();
        }

       /* private void DeleteMedia(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (listplay.SelectedIndex >= this.index)
                {
                    this.index -= 1;
                }
                inUse.mediaList.RemoveAt(listplay.SelectedIndex);
                listplay.Items.Clear();
                foreach (Media elem in inUse.mediaList)
                    listplay.Items.Add(elem.name);
            }
        }*/

        private void listplay_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                //System.Windows.Forms.MessageBox.Show("count:" + inUse.mediaList.Count.ToString() + " selectedIndex:" + listplay.SelectedIndex.ToString());
                if (listplay.SelectedIndex != -1)
                {
                    if (listplay.SelectedIndex >= this.index - 1)
                    {   
                        if (listplay.SelectedIndex == this.index - 1)
                        {
                            VideoControl.Close();
                            if (inUse.mediaList.Count == 1)
                            {
                                VideoControl.Source = null;
                                PlayButton.Content = "Play";
                            }
                            else
                            {
                                VideoControl.Source = new Uri(inUse.mediaList[index].path);
                                VideoControl.Play();
                                PlayButton.Content = "Pause";
                            }
                        }
                    }
                    inUse.mediaList.RemoveAt(listplay.SelectedIndex);
                    listplay.Items.Clear();
                    foreach (Media elem in inUse.mediaList)
                        listplay.Items.Add(elem.name);
                }
            }
        }
    }


    
}
