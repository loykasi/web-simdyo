using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace Scratch.Domain.Requests
{
    public class PaginationQuery
    {
        const int _defaultLimit = 20;
        
        public int Limit
        {
            get => _limit;
            set
            {
                _limit = value > _defaultLimit ? value : _defaultLimit;
            }
        }
        private int _limit = 10;

        public int? Cursor { get; set; }
        public int? Page { get; set; }

        [JsonIgnore]
        public bool IsCursorPagination => Cursor.HasValue;
        [JsonIgnore]
        public int Offset => Page.HasValue ? ((Page.Value - 1) * Limit) : 0;
    }
}
