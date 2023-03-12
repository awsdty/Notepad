using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Notepad.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notepad.Models
{
    public abstract class Dir 
    {

        public Dir(string name) 
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Image { get; set; }
        public string Path { get; set; }
    }
}
