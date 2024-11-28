using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NATSLoadTester.Control
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        private int _connectedClients = 0;
        private Timer _timer;

        public MainPageViewModel()
        {
            ClearConnectedClientsClickCommand = new Command(
                execute: () =>
                {
                    ConnectedClients = 0;
                },
                canExecute: () =>
                {
                    return true;
                });

            _timer = PrepareTimer();
        }

        ~MainPageViewModel()
        {
            // Clean Up
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ClearConnectedClientsClickCommand { get; private set; }

        public int ConnectedClients
        {
            get => _connectedClients;
            set
            {
                if (_connectedClients != value)
                {
                    _connectedClients = value;
                    OnPropertyChanged();
                }
            }
        }

        private Timer PrepareTimer()
        {
            return new Timer(new TimerCallback((s) => this.ConnectedClients++),
                               null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
