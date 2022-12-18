using Application.Attachments;
using System;
using System.Collections.Generic;

namespace Application.Categories
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentName { get; set; }

        public IEnumerable<CategoryDto> Children { get; set; }
        public IEnumerable<AttachmentDto> Attachments { get; set; }
    }
}
