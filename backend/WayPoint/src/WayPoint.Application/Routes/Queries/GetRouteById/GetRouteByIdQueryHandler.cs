using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Routes.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Routes.Queries.GetRouteById;

public class GetRouteByIdQueryHandler
    : IRequestHandler<GetRouteByIdQuery, Result<RouteDto>>
{
    private readonly IRouteRepository _repo;
    private readonly RouteMapper _mapper;

    public GetRouteByIdQueryHandler(IRouteRepository repo, RouteMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<RouteDto>> Handle(
        GetRouteByIdQuery request, CancellationToken ct)
    {
        var route = await _repo.GetByIdAsync(request.Id, ct)
            ?? throw new RouteNotFoundException(request.Id);

        return Result<RouteDto>.Success(_mapper.ToDto(route));
    }
}
