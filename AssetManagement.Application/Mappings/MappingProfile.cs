using AssetManagement.Application.DTOs;
using AssetManagement.Application.DTOs.Auth;
using AssetManagement.Domain.Entities;
using AssetManagement.Domain.ValueObjects;
using AutoMapper;

namespace AssetManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<decimal, Money>().ConvertUsing(src => Money.Create(src, "USD"));
            CreateMap<Money, decimal>().ConvertUsing(src => src.Amount);

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Vendor, VendorDto>().ReverseMap();
            CreateMap<PurchaseOrder, PurchaseOrderDto>().ReverseMap();
            CreateMap<Asset, AssetDto>().ReverseMap();
            CreateMap<AssetItem, AssetItemDto>()
                .ForMember(dest => dest.AssignedToUserId, opt => opt.MapFrom(src => src.Assignments.Where(a => a.ReturnDate == null).Select(a => (int?)a.EmployeeId).FirstOrDefault()))
                .ForMember(dest => dest.AssignedOn, opt => opt.MapFrom(src => src.Assignments.Where(a => a.ReturnDate == null).Select(a => (DateTime?)a.AssignmentDate).FirstOrDefault()))
                .ReverseMap();
            // Mapping for RefreshToken DTO
            CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();
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