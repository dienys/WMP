using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication1
{
    public class Playlist
    {
        private
            string Name { get; set; }

        private MyMap items;

        public Playlist()
        {
            items = new MyMap();

            Random rand = new Random();
            int randomNb = rand.Next(0, 200);
            Name = "Playlist" + randomNb;
        }

        public Playlist(string PlaylistName)
        {
            items = new MyMap();
            Name = PlaylistName;
        }

        public void addItem(Uri item)
        {
            items.add(item);
        }

        public void supprItem(string name)
        {
            items.delete(name);
        }
    }
}
