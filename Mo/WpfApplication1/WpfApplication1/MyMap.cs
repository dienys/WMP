using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication1
{
    class MyMap
    {
        private Dictionary<string, Uri> myMap { get; set; }

        public MyMap() {
            myMap = new Dictionary<string, Uri>();
        }

        public void add(Uri reference)
        {
            string filename = System.IO.Path.GetFileName(reference.LocalPath);
            myMap.Add(filename, reference);
        }

        public void delete(string name)
        {
            myMap.Remove(name);
        }
    }
}
