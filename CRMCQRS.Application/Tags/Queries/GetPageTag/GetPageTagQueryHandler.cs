using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Tags.Queries.GetPageTag;

public class GetPageTagQueryHandler : IRequestHandler<GetPageTagQuery, IPagedList<TagViewModel>>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IMapper _mapper;


    public GetPageTagQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _tagRepository = _unitOfWork.GetRepository<Tag>();
    }

    public async Task<IPagedList<TagViewModel>> Handle(GetPageTagQuery request,
        CancellationToken cancellationToken)
    {
        var tags =
            await _tagRepository.GetPagedListAsync(predicate: request.GetExpression(request), pageIndex: request.Page,
                disableTracking: true, cancellationToken: cancellationToken, 
                selector: item => _mapper.Map<TagViewModel>(item));

        return tags;
    }
}