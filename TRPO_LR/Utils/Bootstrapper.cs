using System.Windows;
using Stylet;
using StyletIoC;
using TRPO_LR.ViewModels;
using TRPO_LR.ViewModels.Framework;

namespace TRPO_LR.Utils;

public class Bootstrapper : Bootstrapper<MainWindowViewModel>
{
    private T GetInstance<T>() => (T) base.GetInstance(typeof(T));

    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        base.ConfigureIoC(builder);
        
        builder.Bind<DialogManager>().ToSelf().InSingletonScope();
        
        builder.Bind<IViewModelFactory>().ToAbstractFactory();
        builder.Bind<MainWindowViewModel>().ToSelf().InSingletonScope();
        builder.Bind<SetBillDialogViewModel>().ToSelf().InSingletonScope();
    }
    
    protected override void Launch()
    {
        base.Launch();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
    }
}