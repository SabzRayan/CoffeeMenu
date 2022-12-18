using Application.Attachments;
using Application.Branches;
using Application.Categories;
using Application.Cities;
using Application.Feedbacks;
using Application.Modules;
using Application.Orders;
using Application.Products;
using Application.Provinces;
using Application.Restaurants;
using Application.Users;
using AutoMapper;
using Domain;
using Domain.Enum;
using System.Linq;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            //string currentUsername = null;
            CreateMap<Attachment, AttachmentDto>();
            CreateMap<Branch, BranchDto>()
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.City.Name))
                .ForMember(d => d.ProvinceName, o => o.MapFrom(s => s.Province.Name));
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.ParentName, o => o.MapFrom(s => s.Parent.Name));
            CreateMap<City, CityDto>()
                .ForMember(d => d.RestaurantCount, o => o.MapFrom(s => s.Branches.Count))
                .ForMember(d => d.ProvinceName, o => o.MapFrom(s => s.Province.Name));
            CreateMap<Feedback, FeedbackDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.OrderDetail.Product.Title));
            CreateMap<Module, ModuleDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.LikeCount, o => o.MapFrom(s => s.OrderDetails.Count(a => a.Feedbacks.Any(b => b.Happiness == HappinessEnum.Good))));
            CreateMap<Province, ProvinceDto>()
                .ForMember(d => d.RestaurantCount, o => o.MapFrom(s => s.Branches.Count));
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.Modules, o => o.MapFrom(s => s.Modules.Select(a => a.Module)));
            CreateMap<User, UserDto>();
        }
    }
}
