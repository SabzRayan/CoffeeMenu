using Application.Core;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System;

namespace Application.Attachments
{
    public class AddAttachment
    {
        public class Command : IRequest<Result<string>>
        {
            public IFormFile File { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.File).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, Result<string>>
        {
            private readonly IWebHostEnvironment webHostEnvironment;

            public Handler(IWebHostEnvironment webHostEnvironment)
            {
                this.webHostEnvironment = webHostEnvironment;
            }

            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.File.ContentType.ToLowerInvariant() != "image/*")
                    return Result<string>.Failure("You can only send image files");

                if (request.File.Length > (2*1024*1024))
                    return Result<string>.Failure("File size limit is 2MB");

                string directoryPath = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot", "attachments", Path.GetRandomFileName());
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, request.File.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                SaveReducedImage(100, stream, filePath);
                await request.File.CopyToAsync(stream, cancellationToken);
                return Result<string>.Success(filePath[(filePath.IndexOf("wwwroot") + 7)..].Replace("\\", "/"));
            }

            public static void SaveReducedImage(int maxWidthHeight, Stream resourceImage, string filePath)
            {
                try
                {
                    //int width = maxWidthHeight;
                    //int height = maxWidthHeight;
#pragma warning disable CA1416 // Validate platform compatibility
                    var image = Image.FromStream(resourceImage);
                    //if (image.Width / image.Height > 1)
                    //    height = width * image.Height / image.Width;
                    //else
                    //    width = height * image.Width / image.Height;
                    var thumb = image.GetThumbnailImage(maxWidthHeight, maxWidthHeight, null, new IntPtr());
                    //var thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                    var thumbFilePath = filePath[..filePath.LastIndexOf('.')] + "thumb.png";
                    thumb.Save(thumbFilePath);
#pragma warning restore CA1416 // Validate platform compatibility
                }
                catch //(Exception e)
                {
                    // Console.WriteLine(e);
                }
            }
        }
    }
}
