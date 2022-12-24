using OHInstaller.Libs.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace OHInstaller
{
    public sealed class MainWindowViewModel : ReactiveObject
    {
        private bool canInstall = false;
        public bool CanInstall
        {
            get => canInstall;
            set => this.RaiseAndSetIfChanged(ref canInstall, value);
        }
        private ObservableCollection<Device> devices = new ObservableCollection<Device>();
        public ObservableCollection<Device> Devices
        {
            get => devices;
            private set => this.RaiseAndSetIfChanged(ref devices, value);
        }
        private ObservableCollection<HapFile> files = new ObservableCollection<HapFile>();
        public ObservableCollection<HapFile> Files
        {
            get => files;
            private set => this.RaiseAndSetIfChanged(ref files, value);
        }
    }
}