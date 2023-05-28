using System.ComponentModel.DataAnnotations;

namespace CRMCQRS.Domain.Common.Enums;

public enum MissionPriority
{
    [Display(Name = "Низкий")]
    Low = 4,
    [Display(Name = "Средний")]
    Medium = 6,
    [Display(Name = "Высокий")]
    High = 7,
}