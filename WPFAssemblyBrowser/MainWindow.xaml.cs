using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.IO;
using AsmBrowser;

namespace WPFAssemblyBrowser
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void btnMainClick(object sender, RoutedEventArgs e)
        {
            string AssemblyPath = Path.Combine(Directory.GetCurrentDirectory(), "Assemblies", "FakerDTO.dll");
            AssemblyBrowser browser = new AssemblyBrowser();
            BrowserResult result = browser.Browse(AssemblyPath);
        }
    }
}
