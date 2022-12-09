using Avalonia.Controls;
using Avalonia.Extensions.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using OHInstaller.Libs;
using System.IO;

namespace OHInstaller
{
    public class MainWindow : AeroWindow
    {
        private Label driveTips;
        private Label statusTips;
        private HyperlinkButton installHdc;
        private HyperlinkButton installManager;
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
            InitializeComponent();
        }
        private void InitializeComponent()
        {
            Width = 800;
            Height = 600;
            driveTips = this.FindControl<Label>("driveTips");
            statusTips = this.FindControl<Label>("statusTips");
            installHdc = this.FindControl<HyperlinkButton>("installHdc");
            installManager = this.FindControl<HyperlinkButton>("installManager");
            if (Hdc.Instance.HasManager)
            {
                driveTips.Content = "已安装";
                driveTips.Foreground = Brushes.Green;
                installManager.IsVisible = false;
            }
            else
            {
                driveTips.Content = "未安装";
                driveTips.Foreground = Brushes.Red;
                installManager.IsVisible = true;
            }
            if (Hdc.Instance.HasHdc)
            {
                statusTips.Content = "已安装";
                statusTips.Foreground = Brushes.Green;
                installHdc.IsVisible = false;
            }
            else
            {
                statusTips.Content = "未安装";
                statusTips.Foreground = Brushes.Red;
                installHdc.IsVisible = true;
            }
            DownloadManager.Instance.ProcessChange += Downloader_ProcessChange;
        }
        private void Downloader_ProcessChange(string progressPercentage)
        {

        }
        private async void InstallHdc_Click(object sender, RoutedEventArgs e)
        {
            if (await UrlTools.Instance.GetToolsUrl())
                Hdc.Instance.InstallHdc();
            else
                await MessageBox.Show("提示", "获取失败，请稍候重试！", MessageBoxButtons.Ok);
        }
        private async void SelectHap_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Filters = new List<FileDialogFilter> { new FileDialogFilter() { Name = "hap安装包", Extensions = new List<string> { "hap" } } }
            };
            var path = await dialog.ShowAsync(this);
            if (path != null && path.Length > 0)
                Hdc.Instance.InstallHap(path.FirstOrDefault());
        }
    }
}