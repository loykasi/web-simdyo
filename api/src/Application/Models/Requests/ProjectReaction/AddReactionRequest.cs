using Domain.Enums;

namespace Application.Models.Requests.ProjectReaction
{
    public class AddReactionRequest
    {
        public ReactionTypes Type { get; set; }
    }
}
