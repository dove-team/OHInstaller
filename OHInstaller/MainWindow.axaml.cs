using Avalonia.Controls;
using Avalonia.Extensions.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData;
using OHInstaller.Libs;
using OHInstaller.Libs.Models;

namespace OHInstaller
{
    public class MainWindow : AeroWindow
    {
        private HapFile hapFile;
        private readonly TextBox pathBox;
        private readonly MainWindowViewModel viewModel;
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            viewModel = new MainWindowViewModel();
            pathBox = this.FindControl<TextBox>("pathBox");
            InitializeComponent();
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems[0] is HapFile file)
            {
                hapFile = file;
                viewModel.CanInstall = true;
            }
            else
            {
                hapFile = null;
                viewModel.CanInstall = false;
            }
        }
        private void InitializeComponent()
        {
            Width = 800;
            Height = 600;
            DataContext = viewModel;
            var list = Hdc.Instance.GetDevices();
            viewModel.Devices.Add(list);
            this.FindControl<ComboBox>("deviceBox").SelectedIndex = 0;
        }
        private void InstallHap_Click(object sender, RoutedEventArgs e)
        {
            if (hapFile != null)
            {
                if (Hdc.Instance.InstallHap(hapFile.Path))
                    MessageBox.Show("提示", "安装成功！", MessageBoxButtons.Ok);
                else
                    MessageBox.Show("提示", "安装失败...", MessageBoxButtons.Ok);
            }
        }
        private void ReplaceHap_Click(object sender, RoutedEventArgs e)
        {
            if (hapFile != null)
            {
                if (Hdc.Instance.ReplaceHap(hapFile.Path))
                    MessageBox.Show("提示", "安装成功！", MessageBoxButtons.Ok);
                else
                    MessageBox.Show("提示", "安装失败...", MessageBoxButtons.Ok);
            }
        }
        private async void SelectHap_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Files.Clear();
            var folderDialog = new OpenFolderDialog();
            var path = await folderDialog.ShowAsync(this);
            pathBox.Text = path;
            if (!string.IsNullOrEmpty(path))
            {
                var files = Directory.GetFiles(path, "*.hap", SearchOption.AllDirectories);
                if (files != null && files.Length > 0)
                {
                    var list = new List<HapFile>();
                    foreach (var file in files)
                        list.Add(new HapFile(file));
                    viewModel.Files.AddRange(list);
                }
            }
        }
    }
}