using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

namespace SlagFieldManagement.Application.Commands.CreatePlaceCommand;

public sealed class CreateSlagFieldPlaceCommandHandler:ICommandHandler<CreateSlagFieldPlaceCommand, Guid>
{
    private readonly IRepository<SlagFieldPlace> _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateSlagFieldPlaceCommandHandler(
        IRepository<SlagFieldPlace> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid>> Handle(
        CreateSlagFieldPlaceCommand request, 
        CancellationToken cancellationToken)
    {
        var place = SlagFieldPlace.Create(request.Row, request.Number);
        await _repository.AddAsync(place);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(place.Id);
    }
}