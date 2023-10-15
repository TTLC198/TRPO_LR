using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TRPO_LR.Models;

public enum MachineStates
{
    [Display(Name = "Внесите деньги")]
    NeedMoney = 0,
    
    [Display(Name = "Недостаточно средств")]
    BadBalance = 1,
    
    [Display(Name = "Внесение средств")]
    TakeMoney = 2,
    
    [Display(Name = "Повторите попытку")]
    Unsuccessful = 3,
    
    [Display(Name = "Ваш баланс: ")]
    Balance = 4,
    
    [Display(Name = "Минимальный номинал: ")]
    MinimalNominal = 5,
    
    [Display(Name = "Выдача денег")]
    Output = 6,
    
    [Display(Name = "Неверный номинал")]
    IncorrectNominal = 7,
}