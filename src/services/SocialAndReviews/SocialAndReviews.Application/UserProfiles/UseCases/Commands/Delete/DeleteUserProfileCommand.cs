using Shared.CQRS.Commands;
using Shared.ErrorHandling;

namespace SocialAndReviews.Application.UserProfiles.UseCases.Commands.Delete
{
    public record DeleteUserProfileCommand(Guid Id) : ICommand<Result<bool>>;
}