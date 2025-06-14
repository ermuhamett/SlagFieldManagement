﻿using Microsoft.EntityFrameworkCore;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Interfaces;
using SlagFieldManagement.Domain.Projection;

namespace SlagFieldManagement.Infrastructure.Repositories;

internal sealed class SlagFieldQueryRepository:ISlagFieldQueryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SlagFieldQueryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<SlagFieldStateProjection>> GetCurrentStatesAsync(CancellationToken ct)
    {
        var query = from place in _dbContext.Set<SlagFieldPlace>().Where(p => !p.IsDelete)
            from state in _dbContext.Set<SlagFieldState>()
                .Where(s => s.PlaceId == place.Id &&
                            (s.State == StateFieldType.BucketPlaced || s.State == StateFieldType.BucketEmptied) &&
                            s.StartDate == _dbContext.Set<SlagFieldState>()
                                .Where(s2 => s2.PlaceId == place.Id &&
                                             (s2.State == StateFieldType.BucketPlaced || s2.State == StateFieldType.BucketEmptied))
                                .Max(s2 => s2.StartDate))
                .DefaultIfEmpty()
            select new SlagFieldStateProjection
            {
                PlaceId = place.Id,
                Row = place.Row,
                Number = place.Number,
                IsEnable = place.IsEnable,
                StateId = state != null ? state.Id : null,
                State = place.IsEnable ? (state != null ? state.State.ToString() : "NotInUse") : "NotInUse",
                BucketId = state != null ? state.BucketId : null,
                MaterialId = state != null ? state.MaterialId : null,
                SlagWeight = state != null ? state.SlagWeight : null,
                StartDate = state != null ? state.StartDate : null,
                EndDate = state != null ? state.EndDate : null,
                Description = state != null ? state.Description : null
            };

        return await query.ToListAsync(ct);
    }
}