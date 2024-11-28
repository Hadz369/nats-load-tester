using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace NATSLoadTester.Control
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        int _connectedClients = 0;

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

        }

        ~MainPageViewModel() 
        {
            // Clean Up
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
