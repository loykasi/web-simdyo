using Scratch.Application.Interfaces.Repositories;
using Sqids;

namespace Scratch.Infrastructure.Services
{
    public class PublicIdService : IPublicIdService
    {
        private SqidsEncoder<int> _sqids;

        public PublicIdService()
        {
            _sqids = new SqidsEncoder<int>(new()
            {
                Alphabet = "YlouQaeEWTI0FMpKwUXJbCvr6O5kxRtDsgZVj8GLSqc2i7NnHmh4Bd9PzyAf13",
                MinLength = 5
            });
        }

        public string Encode(params int[] numbers)
        {
            return _sqids.Encode(numbers);
        }

        public IReadOnlyList<int> Decode(string id)
        {
            return _sqids.Decode(id);
        }
    }
}
