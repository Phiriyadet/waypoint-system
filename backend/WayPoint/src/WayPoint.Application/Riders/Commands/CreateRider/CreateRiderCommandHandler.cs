using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Entities;
using WayPoint.Domain.Enums;

namespace WayPoint.Application.Riders.Commands.CreateRider;

public class CreateRiderCommandHandler
    : IRequestHandler<CreateRiderCommand, Result<RiderDto>>
{
    private readonly IRiderRepository _repo;
    private readonly RiderMapper _mapper;

    public CreateRiderCommandHandler(IRiderRepository repo, RiderMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<RiderDto>> Handle(
        CreateRiderCommand request, CancellationToken ct)
    {
        var vehicleType = Enum.Parse<VehicleType>(request.VehicleType);
        var rider = Rider.Create(request.Name, request.Phone, vehicleType);

        await _repo.AddAsync(rider, ct);
        await _repo.SaveChangesAsync(ct);

        return Result<RiderDto>.Success(_mapper.ToDto(rider));
    }
}
