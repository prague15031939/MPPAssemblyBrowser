using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AsmBrowser;

namespace WPFAssemblyBrowser
{
    class ViewModel : INotifyPropertyChanged
    {
        private RelayCommand openFileCommand;
        public RelayCommand OpenFileCommand
        {
            get
            {
                return openFileCommand ?? (openFileCommand = new RelayCommand(obj =>
                {
                    IDialogService dialogService = new DefaultDialogService();
                    if (dialogService.OpenFile())
                    {
                        AssemblyPath = dialogService.FilePath;
                        BrowseAssembly();
                    }
                }));
            }
        }

        private RelayCommand closeWindowCommand;
        public RelayCommand CloseWindowCommand
        {
            get
            {
                return closeWindowCommand ??
                    (closeWindowCommand = new RelayCommand(obj =>
                    {
                        Window wnd = obj as Window;
                        if (wnd != null)
                        {
                            wnd.Close();
                        }
                    }));
            }
        }

        private string asmFullName;
        public string AssemblyFullName
        {
            get { return asmFullName; }
            set
            {
                asmFullName = value;
                OnPropertyChanged("AssemblyFullName");
            }
        }

        private List<Namespace> asmInfo;
        public List<Namespace> AssemblyInfo
        {
            get { return asmInfo; }
            set
            {
                asmInfo = value;
                OnPropertyChanged("AssemblyInfo");
            }
        }

        private string asmPath;
        public string AssemblyPath
        {
            get { return asmPath; }
            set
            {
                asmPath = value;
                OnPropertyChanged("AssemblyPath");
            }
        }

        private void BrowseAssembly()
        {
            AssemblyBrowser browser = new AssemblyBrowser();
            BrowserResult result = browser.Browse(AssemblyPath);
            AssemblyFullName = result.FullName;
            AssemblyInfo = result.Namespaces;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
