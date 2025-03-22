using AutoMapper;
using Final4.DTO.Flower;
using Final4.Model.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Ánh xạ từ DTO -> Entity
        CreateMap<AddFlowerDTO, Flower>().ReverseMap();
        CreateMap<UpdateFlowerDTO, Flower>().ReverseMap();
    }
}
