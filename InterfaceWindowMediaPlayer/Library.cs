using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows;
using System.Xml.Serialization;
using System.IO;

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

            if (File.Exists("Library.mwmplib") == true)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                    FileStream stream = File.OpenRead("Library.mwmplib");
                    tmpFolderList = (List<string>)serializer.Deserialize(stream);
                    stream.Close();
                }
            tmpFolderList.Insert(0, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

        }

        private bool findFileInFolders(List<string> PathList)
        {
            List<string> fileList = new List<string>();
            List<string> dirList = new List<string>();

            foreach (string elem in PathList)
            {

            }
            return (false);
        }
        public void addFolder(string path)
        {

        }
        public void removeFolder(int index)
        {
            this.FolderPath.RemoveAt(index);
            this.LibraryList.RemoveAt(index);
        }

        public void refresh()
        {
 
        }
        ~Library()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            FileStream stream;

            stream = File.Open("Library.mwmplib", FileMode.Create | FileMode.Truncate);
            serializer.Serialize(stream, this.FolderPath);
            stream.Close();
        }
    }
}
