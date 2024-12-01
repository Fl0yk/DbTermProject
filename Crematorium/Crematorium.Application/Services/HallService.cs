using Crematorium.Application.Abstractions;
using Crematorium.Domain.Abstractions;
using Crematorium.Domain.Entities;

namespace Crematorium.Application.Services
{
    public class HallService : IHallService
    {
        private readonly IHallRepository _hallRepository;

        public HallService(IUnitOfWork unitOfWork) {
            _hallRepository = unitOfWork.HallRepository;
        }

        public async Task AddAsync(Hall hall, CancellationToken cancellationToken = default)
        {
            await _hallRepository.AddAsync(hall, cancellationToken);

            var id = (await _hallRepository.GetAllAsync(cancellationToken)).Where(h => h.Name == hall.Name).Last().Id;

            foreach(var date in hall.FreeDates)
            {
                await _hallRepository.AddFreeDate(id, DateTime.Parse(date.Data), cancellationToken);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await _hallRepository.DeleteAsync(id, cancellationToken);
        }

        public Task<IEnumerable<Hall>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _hallRepository.FindByNameAsync(name, cancellationToken);
        }

        public Task<IEnumerable<Hall>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _hallRepository.GetAllAsync(cancellationToken);
        }

        public Task<Hall?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _hallRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task UpdateAsync(Hall hall, CancellationToken cancellationToken = default)
        {
            await _hallRepository.UpdateAsync(hall, cancellationToken);

            var last = await _hallRepository.GetByIdAsync(hall.Id, cancellationToken);

            foreach (var date in hall.FreeDates.Except(last!.FreeDates))
            {
                await _hallRepository.AddFreeDate(hall.Id, DateTime.Parse(date.Data), cancellationToken);
            }
        }
    }
}
