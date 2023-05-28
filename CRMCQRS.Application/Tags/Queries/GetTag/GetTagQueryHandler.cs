using AutoMapper;
using CRMCQRS.Application.Common.Mappings;
using CRMCQRS.Application.Tags.Commands.UpdateTag;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Tags.Queries.GetTag;

public class GetTagQueryHandler :IRequestHandler<GetTagQuery, TagViewModel>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IMapper _mapper;


    public GetTagQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _tagRepository = _unitOfWork.GetRepository<Tag>();
    }

    public async Task<TagViewModel> Handle(GetTagQuery request,
        CancellationToken cancellationToken)
    {
        var dbTag = await _tagRepository.GetFirstOrDefaultAsync(predicate:item => item.Id == request.Id);
        if (dbTag is null)
        {
            throw new ArgumentException($"Tag with ID {request.Id} not found");
        }

        return _mapper.Map<TagViewModel>(dbTag);
    }
}