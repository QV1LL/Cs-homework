using System.Net;
using XChat.Api.Helpers.Dto;
using XChat.Api.Helpers.Dto.Message;
using XChat.Api.Helpers.Http;
using XChat.Api.Models;
using XChat.Api.Services.Message;
using XChat.Api.Services.User;

namespace XChat.Api.Controllers;

internal class MessageController
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public MessageController(IMessageService messageService, IUserService userService)
    {
        _messageService = messageService;
        _userService = userService;
    }

    public Response CreateMessage(string body)
    {
        var result = DtoHelper.DeserializeRequest<CreateMessageRequest>(body);

        if (result.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", result.Errors));

        var createMessageRequest = result.Value;
        var userResult = _userService.GetByUsernameAsync(createMessageRequest.UserName).Result;
        
        if (userResult.IsFailed)
            return new Response<string>(HttpStatusCode.NotFound, $"Owner '{createMessageRequest.UserName}' of message not found");

        var message = new Message(userResult.Value!, createMessageRequest.Text);

        var createResult = _messageService.CreateAsync(message).Result;

        if (createResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", createResult.Errors));

        return new Response<string>(HttpStatusCode.OK);
    }
}
