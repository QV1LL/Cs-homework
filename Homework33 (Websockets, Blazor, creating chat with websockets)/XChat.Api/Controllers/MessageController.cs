using System.Net;
using System.Web;
using XChat.Api.Helpers.Dto;
using XChat.Api.Helpers.Dto.Message;
using XChat.Api.Helpers.Http;
using XChat.Api.Models;
using XChat.Api.Services.Http;
using XChat.Api.Services.Message;
using XChat.Api.Services.User;

namespace XChat.Api.Controllers;

internal class MessageController
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;
    private readonly WebSocketService _webSocketService;
    private readonly WordsService _wordsService;

    public MessageController(IMessageService messageService,
                             IUserService userService,
                             WebSocketService webSocketService,
                             WordsService wordsService)
    {
        _messageService = messageService;
        _userService = userService;
        _webSocketService = webSocketService;
        _wordsService = wordsService;
    }

    public async Task<Response> CreateMessageAsync(Request request)
    {
        var uri = new Uri("http://dummy" + request.Route);
        var query = HttpUtility.ParseQueryString(uri.Query);

        if (!Guid.TryParse(query["chatId"], out var parsedId))
            return new Response<string>(HttpStatusCode.BadRequest, "Failed to parse chat id");

        var createMessageRequestResult = DtoHelper.DeserializeRequest<CreateMessageRequest>(request.Body);

        if (createMessageRequestResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", createMessageRequestResult.Errors));

        var createMessageRequest = createMessageRequestResult.Value;
        var userResult = await _userService.GetByUsernameAsync(createMessageRequest.UserName);

        if (userResult.IsFailed)
            return new Response<string>(HttpStatusCode.NotFound, $"Owner '{createMessageRequest.UserName}' of message not found");

        var allowedMessageText = _wordsService.ValidateText(createMessageRequest.Text);
        var message = new Message(userResult.Value!, allowedMessageText);

        var createResult = await _messageService.CreateAsync(message, parsedId);

        if (createResult.IsFailed)
            return new Response<string>(HttpStatusCode.BadRequest, string.Join("; ", createResult.Errors));

        var dto = new MessageDto
        (
            message.User.Name,
            message.Text,
            message.CreatedAt
        );

        _ = _webSocketService.BroadcastAsync(parsedId, dto);

        return new Response<string>(HttpStatusCode.OK);
    }

    public async Task<Response> GetRecentMessagesAsync(Request request)
    {
        int count = 20;
        var uri = new Uri("http://dummy" + request.Route);
        var query = HttpUtility.ParseQueryString(uri.Query);

        if (int.TryParse(query["count"], out var parsedCount) && parsedCount > 0)
            count = parsedCount;

        if (!Guid.TryParse(query["chatId"], out var parsedId))
            return new Response<string>(HttpStatusCode.BadRequest, "Cannot map chat id from request");

        var result = await _messageService.GetRecentAsync(count, parsedId);
        
        if (result.IsFailed)
            return new Response<string>(HttpStatusCode.InternalServerError, string.Join("; ", result.Errors));

        var messages = result.Value!
            .Select(m => new MessageDto(m.User.Name, m.Text, m.CreatedAt))
            .ToList();

        return new Response<List<MessageDto>>(HttpStatusCode.OK, messages);
    }

    public async Task<Response> GetOlderMessagesAsync(Request request)
    {
        int count = 20;
        DateTimeOffset before = DateTimeOffset.Now;

        var uri = new Uri("http://dummy" + request.Route);
        var query = HttpUtility.ParseQueryString(uri.Query);

        if (int.TryParse(query["count"], out var parsedCount) && parsedCount > 0)
            count = parsedCount;

        if (long.TryParse(query["before"], out var ms))
            before = DateTimeOffset.FromUnixTimeMilliseconds(ms);

        if (!Guid.TryParse(query["chatId"], out var parsedId))
            return new Response<string>(HttpStatusCode.BadRequest, "Cannot map chat id from request");

        var result = await _messageService.GetOlderAsync(before, count, parsedId);
        if (result.IsFailed)
            return new Response<string>(HttpStatusCode.InternalServerError, string.Join("; ", result.Errors));

        var messages = result.Value!
            .Select(m => new MessageDto(m.User.Name, m.Text, m.CreatedAt))
            .ToList();

        return new Response<List<MessageDto>>(HttpStatusCode.OK, messages);
    }
}
