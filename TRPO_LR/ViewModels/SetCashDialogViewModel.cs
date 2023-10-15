using System;
using System.Windows;
using System.Windows.Controls;
using TRPO_LR.Models;
using TRPO_LR.ViewModels.Framework;

namespace TRPO_LR.ViewModels;

public class SetCashDialogViewModel : DialogScreen<Cash>
{
    public void CheckMoney(string value)
    {
        if (!int.TryParse(value, out var moneyValue)) return;
        var money = new Cash()
        {
            value = moneyValue
        };
        
        var rnd = new Random();
        money.isOriginal = rnd.Next(0, 10) < 9;
        
        if (moneyValue > 10)
            money.isBill = true;
        
        Close(money);
    }
}