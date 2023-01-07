using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Attachments
{
    public class SetMainAttachment
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext context;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IUserAccessor userAccessor)
            {
                this.context = context;
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
                    return Result<Unit>.Failure("You can't set attachment as main in a restaurant made by someone else");

                var currentMain = await context.Attachments.FirstOrDefaultAsync(a =>
                                                                    a.CategoryId == attachment.CategoryId &&
                                                                    a.ProductId == attachment.ProductId &&
                                                                    a.IsMain,
                                                                    cancellationToken: cancellationToken);

                if (currentMain != null) currentMain.IsMain = false;
                attachment.IsMain = true;
                var success = await context.SaveChangesAsync(cancellationToken) > 0;
                if (success) return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Problem setting main attachment");
            }
        }
    }
}
