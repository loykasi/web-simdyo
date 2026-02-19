namespace Domain.Entities
{
    public class Pagination<TItem>
    {
        public int Total { get; set; }
        public int Size { get; set; }
        public int? LastId { get; set; }
        public required List<TItem> Items { get; set; }
    }
}
