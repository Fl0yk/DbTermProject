﻿using Crematorium.Domain.Entities;

namespace Crematorium.Domain.Abstractions;

public interface IHallRepository
{
    public Task<IEnumerable<Hall>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<Hall?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task AddAsync(Hall hall, CancellationToken cancellationToken = default);

    public Task UpdateAsync(Hall hall, CancellationToken cancellationToken = default);

    public Task AddFreeDate(int id, DateTime date, CancellationToken cancellationToken = default);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<Hall>> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
