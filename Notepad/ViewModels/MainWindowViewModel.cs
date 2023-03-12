using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Notepad.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Notepad.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool visual;
        private bool visual1;
        private bool openButton;
        private bool saveButton;
        private string text_box, text_folder;
        private string button_save;
        private int index;
        private ObservableCollection<Dir> Directories;
        private string filepath = Directory.GetCurrentDirectory();


        public ObservableCollection<Dir> DirectoriesProp { get => Directories; private set => this.RaiseAndSetIfChanged(ref Directories, value); }

        public bool Visual { get => visual; set => this.RaiseAndSetIfChanged(ref visual, value); }
        public bool Visual1 { get => visual1; set => this.RaiseAndSetIfChanged(ref visual1, value); }

        public int Index { get => index; private set => this.RaiseAndSetIfChanged(ref index, value); }

        public string Text_box { get => text_box; set => this.RaiseAndSetIfChanged(ref text_box, value); }

        public string Button_Save { get => button_save; set => this.RaiseAndSetIfChanged(ref button_save, value); }

        public bool OpenButton { get => openButton; set => this.RaiseAndSetIfChanged(ref openButton, value); }

        public bool SaveButton { get => saveButton; set => this.RaiseAndSetIfChanged(ref saveButton, value); }


        public string Text_Folder 
        {   get => text_folder; 
            set 
            { 
                this.RaiseAndSetIfChanged(ref text_folder, value);
                if (text_folder != "") Button_Save = "Сохранить";
            } 
        
        }

        public int Cur_Index
        {
            get => index;
            set
            {
                this.RaiseAndSetIfChanged(ref index, value);
                if(openButton == true && saveButton == false)
                {
                    if (Directories[index] is Files) Text_Folder = Directories[index].Name;
                    else Text_Folder = "";
                    Button_Save = "Открыть";
                }
                else if(saveButton == true && openButton == false)
                {
                    if (Directories[index] is Files)
                    {
                        Button_Save = "Сохранить";
                        Text_Folder = Directories[index].Name;
                    }
                    else
                    {
                        Button_Save = "Открыть";
                        Text_Folder = "";
                    }
                }
            }
        }

        public MainWindowViewModel()
        {
            visual = true;
            visual1 = false;
            openButton = false;
            saveButton = false;
            text_box = string.Empty;
            text_folder = string.Empty;
            button_save = "Открыть";

            Directories = new ObservableCollection<Dir>();

            GoPath(filepath);
        }

        public void Open()
        {
            Visual = false;
            Visual1 = true;
            text_folder = string.Empty;
            saveButton = false;
            openButton = true;
        }

        public void Save()
        {
            Visual = false;
            Visual1 = true;
            text_folder = "";
            saveButton = true;
            openButton = false;
            index = 0;
        }
        public void Back()
        {
            Visual = true;
            Visual1 = false;
            text_folder = string.Empty;
            saveButton = false;
            openButton = false;
        }

        public void ButtonClick()
        {
            if (OpenButton == true) ButtonOpenO();
            else if (SaveButton == true) ButtonOpenS();
        }

        public void ButtonOpenO()
        {
            if (Directories[Cur_Index] is Folder)
            {
                if (Directories[Cur_Index].Name == "..")
                {
                    var pathh = Directory.GetParent(filepath);
                    if(pathh != null)
                    {
                        GoPath(pathh.FullName);
                        filepath = pathh.FullName;
                    }
                    else if(pathh == null)
                    {
                        GoPath("");
                    }
                }
                else
                {
                    var temp = Directories[index].Path;
                    GoPath(Directories[Cur_Index].Path);
                    filepath = temp;
                }
            }
            else
            {
                Load(Directories[index].Path);
                Back();
            }
        }

        public void ButtonOpenS()
        {
            if (Directories[Cur_Index] is Dir && Text_Folder == "")
            {
                Button_Save = "Открыть";
                if (Directories[Cur_Index].Name == "..")
                {
                    var patt = Directory.GetParent(filepath);
                    if (patt != null) GoPath(patt.FullName);
                    else if (patt == null) GoPath("");
                    filepath = patt.FullName;
                }
                else
                {
                    var temp_path = Directories[index].Path;
                    GoPath(Directories[Cur_Index].Path);
                    filepath = temp_path;
                }
            }
            else if (Directories[Cur_Index] is Files || Text_Folder != "")
            {
                if (Text_Folder == Directories[index].Name) Save_file(Directories[Cur_Index].Path, 0);
                else
                {
                    var temp_pat = filepath;
                    temp_pat += "\\" + Text_Folder;
                    Save_file(temp_pat, 1);
                }
                Back();
            }
        }

        public void GoPath(string Path1)
        {
            Directories.Clear();
            if (Path1 != "")
            {
                var dirinfo = new DirectoryInfo(Path1);
                Directories.Add(new Folder(".."));
                foreach (var directory in dirinfo.GetDirectories())
                {
                    Directories.Add(new Folder(directory));
                }
                foreach (var fileinfo in dirinfo.GetFiles())
                {
                    Directories.Add(new Files(fileinfo));
                }
            }
            else if (Path1 == "")
            {
                foreach (var disk in Directory.GetLogicalDrives())
                {
                    Directories.Add(new Folder(disk));
                }
            }
        }

        public void Load(string ppath)
        {
            string new_text = String.Empty;
            StreamReader sr = new StreamReader(Directories[index].Path);
            while (sr.EndOfStream != true)
            {
                new_text += sr.ReadLine() + "\n";
            }
            Text_box = new_text;
        }

        public async void Save_file(string ppath, int flag)
        {
            if (flag == 0)
            {
                using (StreamWriter writer = new StreamWriter(ppath))
                {
                    writer.Write(Text_box);
                }
            }
            else
            {
                using (FileStream fstream = new FileStream(ppath, FileMode.OpenOrCreate))
                {
                    byte[] buffer = Encoding.Default.GetBytes(Text_box);
                    await fstream.WriteAsync(buffer, 0, buffer.Length);
                }
            }
        }
        public void DoubleTap()
        {
            if (OpenButton == true) ButtonOpenO();
            else if (SaveButton == true) ButtonOpenS();
        }

    }
}
