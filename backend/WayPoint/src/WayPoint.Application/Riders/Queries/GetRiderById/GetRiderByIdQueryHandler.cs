using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Riders.Queries.GetRiderById;

public class GetRiderByIdQueryHandler
    : IRequestHandler<GetRiderByIdQuery, Result<RiderDto>>
{
    private readonly IRiderRepository _repo;
    private readonly RiderMapper _mapper;

    public GetRiderByIdQueryHandler(IRiderRepository repo, RiderMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<RiderDto>> Handle(
        GetRiderByIdQuery request, CancellationToken ct)
    {
        var rider = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new RiderNotFoundException(request.Id);

        return Result<RiderDto>.Success(_mapper.ToDto(rider));
    }
}
