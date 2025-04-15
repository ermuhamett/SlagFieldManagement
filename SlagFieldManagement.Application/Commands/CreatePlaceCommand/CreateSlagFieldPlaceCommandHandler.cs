using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

namespace SlagFieldManagement.Application.Commands.CreatePlaceCommand;

public sealed class CreateSlagFieldPlaceCommandHandler:ICommandHandler<CreateSlagFieldPlaceCommand, SlagFieldPlace>
{
    private readonly ISlagFieldPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSlagFieldPlaceCommandHandler(
        ISlagFieldPlaceRepository placeRepository,
        IUnitOfWork unitOfWork)
    {
        _placeRepository = placeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<SlagFieldPlace>> Handle(
        CreateSlagFieldPlaceCommand request, 
        CancellationToken ct)
    {
        // Создаём место с помощью фабричного метода
        var placeResult = SlagFieldPlace.Create(request.Row, request.Number);
        if (placeResult.IsFailure)
            return Result.Failure<SlagFieldPlace>(placeResult.Error);

        var place = placeResult.Value;
        
        // Сохраняем в репозитории с использованием транзакции
        await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            await _placeRepository.AddAsync(place, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            await _unitOfWork.CommitTransactionAsync(ct);
            return Result.Success(place);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(ct);
            throw;
        }
    }
}