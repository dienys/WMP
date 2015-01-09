using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace InterfaceWindowMediaPlayer
{
    public class PlayList
    {
        public List<Media> mediaList{ get; set; }

        public PlayList()
        {
            this.mediaList = new List<Media>();
        }
        public PlayList(string path)
        {
            if (File.Exists(path) == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Media>));
                FileStream stream = File.OpenRead(path);

                this.mediaList = (List<Media>)serializer.Deserialize(stream);
                stream.Close();
            }
            else
                MessageBox.Show("The file you specified does not exist.");
        }

        public void addMedia(string path)
        {
            Media file = new Media(path);

            this.mediaList.Add(file);
        }

        public void addMedia(Media file)
        {
            this.mediaList.Add(file);
        }

        public void removeMedia(int index)
        {
            this.mediaList.RemoveAt(index);
        }

        public void save(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Media>)); 
            FileStream stream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None);
            
            serializer.Serialize(stream, this.mediaList);
            stream.Close();
        }
    }
}
