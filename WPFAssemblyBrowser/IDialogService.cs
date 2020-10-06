namespace WPFAssemblyBrowser
{
    interface IDialogService
    {
        string FilePath { get; set; }
        bool OpenFile();
    }
}
