using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SocialAndReviews.Application.UserProfiles.DTOs.Requests;
using SocialAndReviews.Application.UserProfiles.DTOs.Responces;
using SocialAndReviews.Application.UserProfiles.UseCases.Commands.Create;
using SocialAndReviews.Application.UserProfiles.UseCases.Commands.Delete;
using SocialAndReviews.Application.UserProfiles.UseCases.Commands.Update;
using SocialAndReviews.Application.UserProfiles.UseCases.Queries.Get;
using SocialAndReviews.Application.UserProfiles.UseCases.Queries.GetById;

namespace SocialAndReviews.API.Controllers
{
    [Route("api/[controller]")]
    public class UserProfilesController : BaseApiController
    {
        private readonly ISender _sender;

        public UserProfilesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(CreateUserProfileRequest request, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new CreateUserProfileCommand(request), cancellationToken)).ToApiResponse();
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new GetUserProfileByIdQuery(id), cancellationToken)).ToApiResponse();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResult<UserProfileDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationRequest request, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new GetUserProfilesQuery(request), cancellationToken)).ToApiResponse();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync(Guid id, UpdateUserProfileRequest request, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new UpdateUserProfileCommand(id, request), cancellationToken)).ToApiResponse();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            return (await _sender.Send(new DeleteUserProfileCommand(id), cancellationToken)).ToApiResponse();
        }
    }
}