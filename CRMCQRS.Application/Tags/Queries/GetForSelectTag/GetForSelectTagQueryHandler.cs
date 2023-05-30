using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Tags.Queries.GetForSelectTag;

public class GetForSelectTagQueryHandler : IRequestHandler<GetForSelectTagQuery, List<TagViewModel>>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IMapper _mapper;


    public GetForSelectTagQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException("unitOfWork", "Parameter unitOfWork is null");
        _mapper = mapper ?? throw new ArgumentNullException("mapper", "Parameter mapper is null");
        _tagRepository = _unitOfWork.GetRepository<Tag>();
    }

    public async Task<List<TagViewModel>> Handle(GetForSelectTagQuery request,
        CancellationToken cancellationToken)
    {
        var tags =
            await _tagRepository.GetAllAsync(
                disableTracking: true, selector: item => _mapper.Map<TagViewModel>(item));

        return tags.ToList();
    }
}