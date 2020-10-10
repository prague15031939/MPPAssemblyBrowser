using Microsoft.Win32;

namespace WPFAssemblyBrowser
{
    class DefaultDialogService : IDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "dll files (*.dll) | *.dll";
            if (openDialog.ShowDialog() == true)
            {
                FilePath = openDialog.FileName;
                return true;
            }
            return false;
        }
    }
}
