using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Models
{
    public class Folder : Dir
    {
        public Folder(string name) : base(name)
        {
            Path = name;
            Image = "Assets/folder.png";
        }

        public Folder(DirectoryInfo foldername) : base(foldername.Name)
        {
            Path = foldername.FullName;
            Image = "Assets/folder.png";
        }
    }
}
