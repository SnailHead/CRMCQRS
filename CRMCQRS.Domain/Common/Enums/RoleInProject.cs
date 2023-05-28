using System.ComponentModel.DataAnnotations;

namespace CRMCQRS.Domain.Common.Enums;

public enum RoleInProject
{
    [Display(Name = "Продукт менеджер")]
    ProjectManager = 0,
    [Display(Name = "Участник")]
    Participant = 1,
    
}