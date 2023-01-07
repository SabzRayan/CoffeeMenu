using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Attachments
{
    public class DeleteAttachment
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;
            private readonly IWebHostEnvironment webHostEnvironment;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IWebHostEnvironment webHostEnvironment, IUserAccessor userAccessor)
            {
                this.context = context;
                this.webHostEnvironment = webHostEnvironment;
                this.userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var attachment = await context.Attachments.FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken: cancellationToken);
                if (attachment == null) return null;
                var me = await context.Users.FindAsync(new object[] { userAccessor.GetUserId() }, cancellationToken);
                if ((attachment.ProductId != null) ? 
                    (me.RestaurantId != attachment.Product.Category.RestaurantId) : 
                    (me.RestaurantId != attachment.Category.RestaurantId)) 
                        return Result<Unit>.Failure("You can't delete attachments from a restaurant made by someone else");

                if (attachment.IsMain) return Result<Unit>.Failure("You can't delete main attachment");

                string directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot");
                string filePath = Path.Combine(directoryPath, attachment.Url);
                if (File.Exists(filePath)) File.Delete(filePath);

                filePath = filePath[..filePath.LastIndexOf('.')] + "thumb.png";
                if (File.Exists(filePath)) File.Delete(filePath);

                context.Attachments.Remove(attachment);

                var success = await context.SaveChangesAsync(cancellationToken) > 0;
                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Problem deleting attachment from API");
            }
        }
    }
}
