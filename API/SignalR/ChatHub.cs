using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator mediator;

        public ChatHub(IMediator mediator)
        {
            this.mediator = mediator;
        }

        //public async Task SendComment(CreateComment.Command command)
        //{
        //    var comment = await mediator.Send(command);
        //    await Clients.Group(command.EventId?.ToString() ?? command.ClubId?.ToString() ?? command.GameId?.ToString() ?? command.ArticleId?.ToString())
        //            .SendAsync("ReceiveComment", comment.Value);
        //}

        //public async Task DeleteComment(DeleteComment.Command command)
        //{
        //    var comment = await mediator.Send(command);
        //    await Clients.Group(comment.Value.EventId?.ToString() ?? comment.Value.ClubId?.ToString() ?? comment.Value.GameId?.ToString() ?? comment.Value.ArticleId?.ToString())
        //        .SendAsync("DeleteComment", comment.Value.CommentId);
        //}

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var eventId = httpContext.Request.Query["eventId"];
            var clubId = httpContext.Request.Query["clubId"];
            var gameId = httpContext.Request.Query["gameId"];
            var articleId = httpContext.Request.Query["articleId"];
            if (eventId.Any())
                await Groups.AddToGroupAsync(Context.ConnectionId, eventId);
            else if (clubId.Any())
                await Groups.AddToGroupAsync(Context.ConnectionId, clubId);
            else if (gameId.Any())
                await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            else if (articleId.Any())
                await Groups.AddToGroupAsync(Context.ConnectionId, articleId);

            //var result = await mediator.Send(new ListComment.Query 
            //{ 
            //    EventId = eventId.Any() ? Guid.Parse(eventId) : null, 
            //    ArticleId = articleId.Any() ? Guid.Parse(articleId) : null, 
            //    ClubId = clubId.Any() ? Guid.Parse(clubId) : null, 
            //    GameId = gameId.Any() ? Guid.Parse(gameId) : null 
            //});

            //await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}
