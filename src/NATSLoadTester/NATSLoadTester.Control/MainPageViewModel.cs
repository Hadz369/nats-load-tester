using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NATSLoadTester.Infrastructure;

namespace NATSLoadTester.Control
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        private int _connectedClients = 0;
        private Timer _timer;
        private IMessageBusProvider _messageBusProvider;

        public MainPageViewModel()
        {
            _messageBusProvider = new NATSProvider();

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
            _messageBusProvider.Dispose();
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
            return new Timer(new TimerCallback(OnTimer), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void OnTimer(object? stateInfo)
        {
            this.ConnectedClients++;
            _messageBusProvider.PublishString("foo", "Help me, I'm trapped in the magic school bus!");
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
