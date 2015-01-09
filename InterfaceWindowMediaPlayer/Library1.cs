using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace MyWindowsMediaPlayer
{
    public class Library
    {
        List<PlayList>  LibraryList { get; set; }
        List<string> FolderPath { get; set; }

        public Library()
        {

            List<string> tmpFolderList = new List<string>();
            List<string> tmpFileList = new List<string>();

            if (File.Exists("Library.rbl") == true)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                    FileStream stream = File.OpenRead("Library.rbl");
                    tmpFolderList = (List<string>)serializer.Deserialize(stream);
                    stream.Close();
                }
            tmpFolderList.Insert(0, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        }

        private bool findFileInFolders(List<string> PathList)
        {
            List<string>    dirList = new List<string>();
            string          toCompare = ".jpg;.png;.jpeg;.bmp;.mp3;.acc;.wav;.wma;.mp4;.wmv;.avi;";
            PlayList        playList;
            string[]        tmp;


            foreach (string directory in PathList)
            {
                tmp = Directory.GetFiles(directory);
                if (this.FolderPath.IndexOf(directory) != -1)
                {
                    foreach (string file in tmp)
                    {
                        playList = new PlayList();
                        if (toCompare.IndexOf(Path.GetExtension(file)) > 0)
                            playList.addMedia(new Media(file));
                        this.FolderPath.Add(directory);
                        this.LibraryList.Add(playList);
                    }
                    tmp = Directory.GetDirectories(directory);
                    dirList = tmp.ToList<string>();
                    if (dirList.Count != 0)
                        findFileInFolders(dirList);
                }
            }
            return (false);
        }
        public void addFolder(string path)
        {
            if (this.FolderPath.IndexOf(path) != -1)
            {
                List<string> dirList = new List<string>(); ;
                dirList.Add(path);
                findFileInFolders(dirList);
            }
        }
        public void removeFolder(int index)
        {
            this.FolderPath.RemoveAt(index);
            this.LibraryList.RemoveAt(index);
        }

        public void refresh()
        {
            List<string> tmp = this.FolderPath;
            this.FolderPath = new List<string>();
            findFileInFolders(tmp);
        }
        ~Library()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            FileStream stream;

            stream = File.Open("Library.rbl", FileMode.Create, FileAccess.Write, FileShare.None);
            serializer.Serialize(stream, this.FolderPath);
            stream.Close();
        }
    }
}
