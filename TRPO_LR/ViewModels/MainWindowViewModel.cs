using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TRPO_LR.Models;
using TRPO_LR.Utils;
using TRPO_LR.ViewModels.Framework;

namespace TRPO_LR.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly DialogManager _dialogManager;
    private readonly IViewModelFactory _viewModelFactory;

    public ObservableCollection<Cash> Cashes = new ObservableCollection<Cash>();

    public Nominals CurrentMinimalNominal { get; set; } = Enum.GetValues<Nominals>().First();
    
    public bool StartBillAnimation
    {
        get => _startBillAnimation;
        set
        {
            _startBillAnimation = value;
            OnPropertyChanged();
        }
    }

    private bool _startMoneyAnimation;
    
    public bool StartMoneyAnimation
    {
        get => _startMoneyAnimation;
        set
        {
            _startMoneyAnimation = value;
            OnPropertyChanged();
        }
    }

    private bool _startBillAnimation;
    
    public int CurrentBalance { get; set; }

    public MachineStates CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            MessageText = _currentState.GetDisplayName();
        }
    }

    private MachineStates _currentState;
    
    public string MessageText
    {
        get => _messageText;
        set
        {
            _messageText = value;
            OnPropertyChanged();
        }
    }

    private string _messageText = "Внесите деньги";

    public ImageSource BillImageSource
    {
        get => _billImageSource;
        private set
        {
            _billImageSource = value;
            OnPropertyChanged();
        }
    }
    
    private ImageSource _billImageSource =
        new BitmapImage(new Uri(@"pack://application:,,,/TRPO_LR;component/Resources/10r.jpg", UriKind.Absolute));
    
    public MainWindowViewModel(DialogManager dialogManager, IViewModelFactory viewModelFactory)
    {
        _dialogManager = dialogManager;
        _viewModelFactory = viewModelFactory;
    }

    public void SetMessageText(string value)
    {
        MessageText = value;
    }

    public async void TakeCash(bool isBill)
    {
        CurrentState = MachineStates.TakeMoney;

        SetCashDialogViewModel viewModel =
            isBill 
                ? _viewModelFactory.CreateSetBillViewModel() 
                : _viewModelFactory.CreateSetMoneyViewModel();
        
        var cash = await _dialogManager.ShowDialogAsync(viewModel);
        
        await Task.Delay(500);
        if (isBill)
        {
            BillImageSource = new BitmapImage(
                new Uri(@$"pack://application:,,,/TRPO_LR;component/Resources/{cash.value}r.jpg",
                    UriKind.Absolute));
            StartBillAnimation = true;
        }
        else
            StartMoneyAnimation = true;
        await Task.Delay(2000);
        
        StartBillAnimation = false;
        StartMoneyAnimation = false;
        
        if (cash is null or {isOriginal: false})
        {
            CurrentState = MachineStates.Unsuccessful;
            return;
        }
        
        Cashes.Add(cash);
        CurrentBalance += cash.value;
        CurrentState = MachineStates.Balance;
        MessageText += CurrentBalance;
    }

    public async void ChangeNominal()
    {
        var maxNominal = Enum.GetValues<Nominals>().Max();
        CurrentMinimalNominal = 
            (int) CurrentMinimalNominal == (int)maxNominal 
                ? Nominals.Rubles1 
                : Enum.GetValues<Nominals>()
                    .SkipWhile(n => n != CurrentMinimalNominal).Skip(1).First();
        CurrentState = MachineStates.MinimalNominal;
        MessageText += CurrentMinimalNominal.GetDisplayName();
    }
    
    public async void Process()
    {
        if (Cashes.Sum(c => c.value) == 0)
        {
            CurrentState = MachineStates.NeedMoney;
            return;
        }

        var cashesAfterExchange = new List<Cash>();
        
        foreach (var denomination in Enum.GetValues<Nominals>()
                     .OrderByDescending(n => n)
                     .SkipWhile(n => n != CurrentMinimalNominal)
                     .Cast<int>())
        {
            if (CurrentBalance >= denomination)
            {
                int count = CurrentBalance / denomination;
                for (int i = 0; i < count; i++)
                {
                    cashesAfterExchange.Add(new Cash()
                    {
                        value = denomination,
                        isOriginal = true
                    });
                }
                CurrentBalance %= denomination;
            }
        }
        Cashes.Clear();

        var msg = new StringBuilder();

        foreach (var nominal in Enum.GetValues<Nominals>())
        {
            var count = cashesAfterExchange.Count(c => c.value == (int) nominal);
            msg.AppendLine($"{nominal.GetDisplayName()} - {count} шт.");
        }
        
        CurrentBalance = 0;
        CurrentState = MachineStates.Output;
        
        var messageBoxDialog = _viewModelFactory.CreateMessageBoxViewModel(
            title: "Вам выданы разменянные деньги:",
            message: msg.ToString(),
            okButtonText: null,
            cancelButtonText: "Ок"
        );
        await _dialogManager.ShowDialogAsync(messageBoxDialog);
    }

    public async void Cancel()
    {
        if (Cashes.Sum(c => c.value) == 0)
            CurrentState = MachineStates.NeedMoney;

        var msg = new StringBuilder();

        foreach (var nominal in Enum.GetValues<Nominals>())
        {
            var count = Cashes.Count(c => c.value == (int) nominal);
            msg.AppendLine($"{nominal.GetDisplayName()} - {count} шт.");
        }

        CurrentBalance = 0;
        CurrentState = MachineStates.Output;
        
        var messageBoxDialog = _viewModelFactory.CreateMessageBoxViewModel(
            title: "Вам выданы все ваши деньги:",
            message: msg.ToString(),
            okButtonText: null,
            cancelButtonText: "Ок"
        );
        await _dialogManager.ShowDialogAsync(messageBoxDialog);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}