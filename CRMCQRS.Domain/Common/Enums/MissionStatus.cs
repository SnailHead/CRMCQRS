using System.ComponentModel.DataAnnotations;

namespace CRMCQRS.Domain.Common.Enums;

public enum MissionStatus
{
    [Display(Name = "Требуется предварительное действие")]
    PriorActionRequired = 0,
    [Display(Name = "На оценке")]
    InEvaluation = 1,
    [Display(Name = "В работе")]
    InProgress = 2,
    [Display(Name = "На проверке")]
    InReview = 3,
    [Display(Name = "Завершена")]
    Complete = 10
}