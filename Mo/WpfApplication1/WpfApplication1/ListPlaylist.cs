using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class ListPlaylist
    {
        private List<Playlist> Mylist;

        ~ListPlaylist()
        {
            SavePlaylists();
        }

        public ListPlaylist()
        {
            try
            {LoadPlaylists();}
            catch (FileNotFoundException)
            {
                Mylist = new List<Playlist>();
                Playlist PlaylistBasic = new Playlist("BasicPlaylist");
                Mylist.Add(PlaylistBasic);
            }
        }

        public void LoadPlaylists()
        {
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(List<Playlist>));
            System.IO.StreamReader file = new System.IO.StreamReader("SerializationList.xml");
            Mylist = new List<Playlist>();
            Mylist = reader.Deserialize(file) as List<Playlist>;
        }

        public void SavePlaylists()
        {
            System.Xml.Serialization.XmlSerializer MyWrite = new System.Xml.Serialization.XmlSerializer(typeof(List<Playlist>));
            System.IO.FileStream file = System.IO.File.Create("SerializationList.xml");
            MyWrite.Serialize(file, Mylist);
        }

        public void AddPlaylist(string Name)
        {
            Playlist newOne = new Playlist(Name);
            Mylist.Add(newOne);
        }

        public void PlaylistSuppr()
        {
//            Mylist.RemoveAt();
        }
    }
}
