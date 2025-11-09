namespace Scratch.Application.Interfaces.Repositories
{
    public interface IPublicIdService
    {
        string Encode(params int[] numbers);
        IReadOnlyList<int> Decode(string id);
    }
}
