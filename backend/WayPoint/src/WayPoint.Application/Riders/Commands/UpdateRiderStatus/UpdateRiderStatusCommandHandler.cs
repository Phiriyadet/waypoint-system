using MediatR;
using WayPoint.Application.Common.Interfaces.Repositories;
using WayPoint.Application.Common.Mappings;
using WayPoint.Application.Riders.DTOs;
using WayPoint.Domain.Common;
using WayPoint.Domain.Enums;
using WayPoint.Domain.Exceptions;

namespace WayPoint.Application.Riders.Commands.UpdateRiderStatus;

public class UpdateRiderStatusCommandHandler
    : IRequestHandler<UpdateRiderStatusCommand, Result<RiderDto>>
{
    private readonly IRiderRepository _repo;
    private readonly RiderMapper _mapper;

    public UpdateRiderStatusCommandHandler(IRiderRepository repo, RiderMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Result<RiderDto>> Handle(
        UpdateRiderStatusCommand request, CancellationToken ct)
    {
        var rider = await _repo.GetByIdAsync(request.RiderId, ct)
            ?? throw new RiderNotFoundException(request.RiderId);

        var status = Enum.Parse<RiderStatus>(request.Status);
        rider.ChangeStatus(status);

        await _repo.SaveChangesAsync(ct);
        return Result<RiderDto>.Success(_mapper.ToDto(rider));
    }
}
