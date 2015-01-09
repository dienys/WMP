using System;

namespace WpfApplication1
{
    public class ListPlaylist
    {
        private
            List<Playlist> Mylist;

        private var pathSave;

        public ListPlaylist()
        {
            pathSave = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationPlaylist.xml";
            if (pathSave)
            {
                LoadPlaylists(pathSave);
            }
            else
            {
                Mylist = new List<Playlist>();
                Playlist BasicPlaylist = new Playlist("BasicPlaylist");
                Mylist.Add(BasicPlaylist);
            }
        }

        public void LoadPlaylists(var FilePath)
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof (ListPlaylist));
            System.IO.StreamReader file = new System.IO.StreamReader(pathSave);
            Mylist = new List<Playlist>();
            Mylist = (ListPlaylist) reader.Deserialize(file);
        }

        public void SavePlaylists()
        {
            System.Xml.Serialization.XmlSerializer MyWrite = new System.Xml.Serialization.XmlSerializer(typeof (Mylist));
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//SerializationMylist.xml";
            System.IO.FileStream file = System.IO.File.Create(path);
            MyWrite.Serialize(file, Mylist);
        }

        public void AddPlaylist(string Name)
        {
            Playlist newOne = new Playlist(Name);
            Mylist.Add(newOne);
        }

        public void PlaylistSuppr(string name)
        {

        }
    }
}