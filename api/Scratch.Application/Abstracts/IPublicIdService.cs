namespace Scratch.Application.Abstracts
{
    public interface IPublicIdService
    {
        string Encode(params int[] numbers);
        IReadOnlyList<int> Decode(string id);
    }
}
