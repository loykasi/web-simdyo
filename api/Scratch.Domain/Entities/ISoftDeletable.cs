namespace Scratch.Domain.Entities
{
    public interface ISoftDeletable
    {
        DateTime? DeletedAt { get; set; }
    }
}
