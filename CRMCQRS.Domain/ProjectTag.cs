using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CRMCQRS.Domain;

public class ProjectTag
{
    public Guid ProjectsId { get; set; }
    public Guid TagsId { get; set; }
    public Tag Tag { get; set; }
    public Project Project { get; set; }
}