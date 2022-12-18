using Application.Branches;
using Application.Categories;
using Application.Modules;
using Application.Users;
using Domain;
using System;
using System.Collections.Generic;

namespace Application.Restaurants
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Logo { get; set; }
        public string HeaderPic { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Address { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Whatsapp { get; set; }
        public string Telegram { get; set; }
        public string Tweeter { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<ModuleDto> Modules { get; set; }
        public IEnumerable<BranchDto> Branches { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}
