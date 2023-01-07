using System;

namespace Application.Branches
{
    public class BranchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int TableCount { get; set; }
        public Double Lat { get; set; }
        public Double Lng { get; set; }
        public Guid CityId { get; set; }
        public Guid ProvinceId { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public string Logo { get; set; }
    }
}
