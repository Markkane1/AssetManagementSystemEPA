using AssetManagement.Application.DTOs;
using AssetManagement.Domain.Entities;
using AutoMapper;

namespace AssetManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Vendor, VendorDto>().ReverseMap();
            CreateMap<PurchaseOrder, PurchaseOrderDto>().ReverseMap();
            CreateMap<Asset, AssetDto>().ReverseMap();
            CreateMap<AssetItem, AssetItemDto>().ReverseMap();
            CreateMap<Directorate, DirectorateDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();
            CreateMap<Assignment, AssignmentDto>().ReverseMap();
            CreateMap<MaintenanceRecord, MaintenanceRecordDto>().ReverseMap();
            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<TransferHistory, TransferHistoryDto>().ReverseMap();

            CreateMap<AssetAssignmentReportDto, AssetItem>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ItemStatus))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.ItemSource))
                .ReverseMap();

            CreateMap<NonRepairableAssetReportDto, AssetItem>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ItemStatus))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.ItemSource))
                .ReverseMap();
        }
    }
}