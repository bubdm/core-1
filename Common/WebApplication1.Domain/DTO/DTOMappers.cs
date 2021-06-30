using AutoMapper;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.DTO
{
    public static class DTOMappers
    {
        public static readonly Mapper MapperSectionToDTO;
        public static readonly Mapper MapperSectionFromDTO;
        public static readonly Mapper MapperBrandToDTO;
        public static readonly Mapper MapperBrandFromDTO;
        public static readonly Mapper MapperProductToDTO;
        public static readonly Mapper MapperProductFromDTO;

        static DTOMappers()
        {
            MapperSectionToDTO = new Mapper(new MapperConfiguration(c => c.CreateMap<Section, SectionDTO>()));
            MapperSectionFromDTO = new Mapper(new MapperConfiguration(c => c.CreateMap<SectionDTO, Section>()));
            MapperBrandToDTO = new Mapper(new MapperConfiguration(c => c.CreateMap<Brand, BrandDTO>()));
            MapperBrandFromDTO = new Mapper(new MapperConfiguration(c => c.CreateMap<BrandDTO, Brand>()));
            MapperProductToDTO = new Mapper(new MapperConfiguration(c => c.CreateMap<Product, ProductDTO>()
                .ForMember("Section", o => o.MapFrom(p => MapperSectionToDTO.Map<SectionDTO>(p.Section)))
                .ForMember("Brand", o=> o.MapFrom(p => MapperBrandToDTO.Map<BrandDTO>(p.Brand)))));
            MapperSectionToDTO = new Mapper(new MapperConfiguration(c => c.CreateMap<ProductDTO, Product>()
                .ForMember("Section", o => o.MapFrom(p => MapperSectionFromDTO.Map<Section>(p.Section)))
                .ForMember("Brand", o => o.MapFrom(p => MapperBrandFromDTO.Map<Brand>(p.Brand)))));
        }
    }
}
