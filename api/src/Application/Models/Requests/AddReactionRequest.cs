using Domain.Enums;

namespace Application.Models.Requests
{
    public class AddReactionRequest
    {
        public ReactionTypes Type { get; set; }
    }
}
