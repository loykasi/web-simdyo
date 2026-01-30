using Scratch.Application.Abstracts;
using Scratch.Application.Interfaces.Services;
using Scratch.Domain.Entities;
using Sqids;

namespace Scratch.Infrastructure.Services
{
    public class PublicIdService : IPublicIdService
    {
        private SqidsEncoder<int> _sqids;
        private readonly IUnitOfWork _unitOfWork;

        public PublicIdService(IUnitOfWork unitOfWork)
        {
            _sqids = new SqidsEncoder<int>(new()
            {
                Alphabet = "YlouQaeEWTI0FMpKwUXJbCvr6O5kxRtDsgZVj8GLSqc2i7NnHmh4Bd9PzyAf13",
                MinLength = 5
            });

            _unitOfWork = unitOfWork;
        }

        public string Encode(params int[] numbers)
        {
            return _sqids.Encode(numbers);
        }

        public IReadOnlyList<int> Decode(string id)
        {
            return _sqids.Decode(id);
        }

        public bool TryDecodeId(string publicId, out int id)
        {
            id = 0;
            if (Decode(publicId) is [int decodedId] &&
                publicId == Encode(decodedId))
            {
                id = decodedId;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Project?> GetProject(string publicId)
        {
            if (TryDecodeId(publicId, out int id))
            {
                return await _unitOfWork.ProjectRepository.GetById(id);
            }

            return null;
        }
    }
}
