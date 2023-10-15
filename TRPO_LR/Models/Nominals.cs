using System.ComponentModel.DataAnnotations;

namespace TRPO_LR.Models;

public enum Nominals
{
    [Display(Name = "1₽")]
    Rubles1 = 1,
    [Display(Name = "2₽")]
    Rubles2 = 2,
    [Display(Name = "5₽")]
    Rubles5 = 5,
    [Display(Name = "10₽")]
    Rubles10 = 10,
    [Display(Name = "50₽")]
    Rubles50 = 50,
    [Display(Name = "100₽")]
    Rubles100 = 100,
    [Display(Name = "200₽")]
    Rubles200 = 200,
    [Display(Name = "500₽")]
    Rubles500 = 500,
    [Display(Name = "1000₽")]
    Rubles1000 = 1000,
    [Display(Name = "2000₽")]
    Rubles2000 = 2000,
    [Display(Name = "5000₽")]
    Rubles5000 = 5000,
}