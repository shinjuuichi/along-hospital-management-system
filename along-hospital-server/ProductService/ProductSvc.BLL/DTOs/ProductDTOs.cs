using AutoMapper;
using Microsoft.AspNetCore.Http;
using ProductSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.DTOs.ImageDTOs.BaseDTOs;
using SharedLibrary.Enums;

namespace ProductSvc.BLL.DTOs
{
    #region ProductDetail DTOs (nested for Product)

    public class CreateProductDetailDTO : MapTo<ProductDetail>
    {
        public string Specification { get; set; } = null!;
        public string? Ingredients { get; set; }
        public double Weight { get; set; }
        public string? Manufacturer { get; set; }
        public string? CountryOfOrigin { get; set; }
    }

    public class UpdateProductDetailDTO : MapTo<ProductDetail>
    {
        public string? Specification { get; set; }
        public string? Ingredients { get; set; }
        public double? Weight { get; set; }
        public string? Manufacturer { get; set; }
        public string? CountryOfOrigin { get; set; }
    }

    public class GetProductDetailDTO : MapFrom<ProductDetail>
    {
        public int ProductId { get; set; }
        public string Specification { get; set; } = null!;
        public string? Ingredients { get; set; }
        public double Weight { get; set; }
        public string? Manufacturer { get; set; }
        public string? CountryOfOrigin { get; set; }
    }

    #endregion

    #region Product DTOs

    public class CreateProductDTO : MapTo<Product>, ICreateMultiImagesDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFileCollection? NewImages { get; set; }

        public CreateProductDetailDTO? ProductDetail { get; set; }
    }

    public class UpdateProductDTO : MapTo<Product>, IUpdateMultiImagesDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        [AllowFileType(FileType.Image)]
        public IFormFileCollection? NewImages { get; set; }
        public string[]? RemainImages { get; set; }
        public string[]? RemoveImages { get; set; }

        public UpdateProductDetailDTO? ProductDetail { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateProductDTO, Product>()
                .ForMember(d => d.ProductDetail, opt => opt.Ignore())
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping);
        }
    }

    public class GetProductDTO : MapFrom<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string[] Images { get; set; } = Array.Empty<string>();

        public GetProductDetailDTO? ProductDetail { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Product, GetProductDTO>()
              .ForMember(d => d.CategoryName,
                  opt => opt.MapFrom(s => s.Category != null ? s.Category.Name : null))
              .ForMember(d => d.ProductDetail,
                  opt => opt.MapFrom(s => s.ProductDetail));
        }
    }

    #endregion
}

