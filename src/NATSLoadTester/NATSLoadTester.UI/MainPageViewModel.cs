using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NATSLoadTester.Application;
using NATSLoadTester.Infrastructure;
using NATSLoadTester.Infrastructure.Messaging;

namespace NATSLoadTester.UI
{
    public class MainPageViewModel : INotifyPropertyChanged, IDisposable
    {
        private int _connectedClients = 0;
        private Timer _timer;
        
        IMessageBusProvider _messageBusProvider;
        CancellationTokenSource _cts = new CancellationTokenSource();
        private object _myCollectionLock = new object();

        public MainPageViewModel()
        {
            _messageBusProvider = MessageBusProviderFactory.GetProvider();

            _messageBusProvider.Subscribe(
                "foo",
                new Action<IMessageBusMessage>((m) => { AddReceivedMessage(m); }),
                _cts.Token);

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> ReceivedMessages { get; } = new();
        
        public ICommand ClearConnectedClientsClickCommand { get; private set; }

        public int ConnectedClients
        {
            get => _connectedClients;
            set => SetConnectedClients(value);
        }

        private void SetConnectedClients(int value)
        {
            if (_connectedClients != value)
            {
                _connectedClients = value;
                OnPropertyChanged("ConnectedClients");
            }
        }

        private Timer PrepareTimer()
        {
            return new Timer(new TimerCallback(OnTimer), null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void AddReceivedMessage(IMessageBusMessage message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                lock (_myCollectionLock)
                {
                    ReceivedMessages.Add($"{message.Subject}, {message.Data}");
                    if (ReceivedMessages.Count > 5)
                    {
                        ReceivedMessages.RemoveAt(0);
                    }
                }
            });
        }

        private void OnTimer(object? stateInfo)
        {
            this.ConnectedClients++;
            _messageBusProvider.PublishString("foo", "Help me, I'm trapped in the magic school bus!");
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        ~MainPageViewModel()
        {
        }

        public void Dispose()
        {
            _cts.Cancel();
            _messageBusProvider.Dispose();
        }
    }
}
