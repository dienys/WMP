using System;
using Systeme.windows;
using Microsoft.Win32;
using System.Collections.Generic;

namespace WpfApplication1
{
    class Playlist
    {
        private
            string Name { get; set; }
        private MyMap Playlist;

        public
            Playlist(string PlaylistName)
        {
            Playlist = new MyMap();
            if (PlaylistName)
                Name = PlaylistName;
            else
            {
                Random rand = new Random();
                int randomNb = rand.Next(0, 200);
                Name = "Playlist" + randomNb;
            }
        }

        void PlaylistAdd(Uri NewItem)
        {
            Playlist.add(NewItem);
            return;
        }

        void PlaylistSupprElem(Uri Item)
        {
            Playlist.delete(Item);
        }
    }
}