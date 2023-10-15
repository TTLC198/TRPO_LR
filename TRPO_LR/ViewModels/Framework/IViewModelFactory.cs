namespace TRPO_LR.ViewModels.Framework;

public interface IViewModelFactory
{
    SetBillDialogViewModel CreateSetBillViewModel();
    SetMoneyDialogViewModel CreateSetMoneyViewModel();
    MessageBoxViewModel CreateMessageBoxViewModel();
}