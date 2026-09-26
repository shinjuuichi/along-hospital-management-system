using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.DTOs.ImageDTOs.BaseDTOs;
using SharedLibrary.Services.Interfaces;

namespace SharedLibrary.Base.Services
{
    public class BaseService<TModel, TCreateDTO, TUpdateDTO, TGetDTO>(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUploadFileService? uploadFileService = null,
        string[]? includes = null)
            : IBaseCrudService<TCreateDTO, TUpdateDTO, TGetDTO>
                where TModel : BaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly IMapper _mapper = mapper;
        protected readonly string[]? _includes = includes;
        protected readonly IUploadFileService? _uploadFileService = uploadFileService;

        protected readonly IGenericRepository<TModel> _repository = unitOfWork.Repository<TModel>();

        #region Get All
        public virtual async Task<List<TGetDTO>> GetAllAsync()
        {
            var entites = await _repository.GetAllAsync(includes: _includes);
            return _mapper.Map<List<TGetDTO>>(entites);
        }

        public virtual async Task<PaginationResult<TGetDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var (total, entities) = await _repository.GetAllPaginatedAsync(filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                _includes);

            var entitiesDTO = _mapper.Map<List<TGetDTO>>(entities);
            return new PaginationResult<TGetDTO>(total, filterDTO.PageSize, entitiesDTO);
        }
        #endregion

        #region Get With Id
        public virtual async Task<List<TGetDTO>> GetAllByIdsAsync(List<int> ids)
        {
            var entities = await _repository.GetAllByIdsAsync(ids, _includes);
            return _mapper.Map<List<TGetDTO>>(entities);
        }

        public virtual async Task<TGetDTO> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id, _includes)
                ?? throw new DataNotFoundException(typeof(TModel), id);
            return _mapper.Map<TGetDTO>(entity);
        }
        #endregion

        #region Write
        public virtual async Task<TGetDTO> CreateAsync(TCreateDTO createDTO)
        {
            var entity = _mapper.Map<TModel>(createDTO);

            if (_uploadFileService != null)
            {
                if (entity is EntityWithImage entityWithImage && createDTO is IUploadImageDTO createImageDto)
                {
                    var file = createImageDto.Image;
                    if (file is { Length: > 0 })
                    {
                        entityWithImage.Image = await _uploadFileService.UploadAsync(file, typeof(TModel).Name);
                    }
                }
                else if (entity is EntityWithMultiImages entityWithMultiImages && createDTO is ICreateMultiImagesDTO createMultiImagesDto)
                {
                    var files = createMultiImagesDto.NewImages;
                    if (files is { Count: > 0 })
                    {
                        entityWithMultiImages.Images = await _uploadFileService.UploadManyAsync(files, typeof(TModel).Name);
                    }
                }
            }

            var resultEntity = await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(resultEntity.Id);
        }

        public virtual async Task<TGetDTO> UpdateAsync(int id, TUpdateDTO updateDTO)
        {
            var entity = await _repository.GetByIdAsync(id)
                 ?? throw new DataNotFoundException(typeof(TModel), id);

            _mapper.Map(updateDTO, entity);

            if (_uploadFileService != null)
            {
                if (entity is EntityWithImage entityWithImage && updateDTO is IUploadImageDTO updateImageDto)
                {
                    var newImage = updateImageDto.Image;
                    if (newImage is { Length: > 0 })
                    {
                        if (!string.IsNullOrWhiteSpace(entityWithImage.Image))
                        {
                            await _uploadFileService.DeleteAsync(entityWithImage.Image);
                        }

                        entityWithImage.Image = await _uploadFileService.UploadAsync(newImage, typeof(TModel).Name);
                    }
                }
                else if (entity is EntityWithMultiImages entityWithMultiImages && updateDTO is IUpdateMultiImagesDTO updateMultiImagesDto)
                {
                    var remainImages = updateMultiImagesDto.RemainImages ?? [];
                    var removeImages = updateMultiImagesDto.RemoveImages ?? [];

                    if (removeImages.Length > 0)
                    {
                        await _uploadFileService.DeleteManyAsync(removeImages);
                    }

                    var uploadedNewImages = new List<string>();
                    if (updateMultiImagesDto.NewImages is { Count: > 0 })
                    {
                        var uploadedImages = await _uploadFileService.UploadManyAsync(updateMultiImagesDto.NewImages, typeof(TModel).Name);
                        uploadedNewImages.AddRange(uploadedImages);
                    }

                    entityWithMultiImages.Images = remainImages.Concat(uploadedNewImages).ToArray();
                }
            }

            var resultEntity = _repository.Update(entity);
            await _unitOfWork.SaveChangeAsync();

            return await this.GetByIdAsync(resultEntity.Id);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(TModel), id);

            if (_uploadFileService != null)
            {
                if (entity is EntityWithImage entityWithImage
                    && !string.IsNullOrWhiteSpace(entityWithImage.Image))
                {
                    await _uploadFileService.DeleteAsync(entityWithImage.Image);
                }
                else if (entity is EntityWithMultiImages entityWithMultiImages
                         && entityWithMultiImages.Images is { Length: > 0 })
                {
                    await _uploadFileService.DeleteManyAsync(entityWithMultiImages.Images);
                }
            }

            _repository.Remove(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public virtual async Task DeleteSelectedIdsAsync(List<int> ids)
        {
            var entities = await _repository.GetAllByIdsAsync(ids);
            if (entities.Count != ids.Count)
            {
                var foundIds = entities.Select(e => e.Id).ToHashSet();
                var firstNotFoundId = ids.FirstOrDefault(id => !foundIds.Contains(id));
                throw new DataNotFoundException(typeof(TModel), firstNotFoundId);
            }

            if (_uploadFileService != null)
            {
                foreach (var entity in entities)
                {
                    if (entity is EntityWithImage entityWithImage
                        && !string.IsNullOrWhiteSpace(entityWithImage.Image))
                    {
                        await _uploadFileService.DeleteAsync(entityWithImage.Image);
                    }
                    else if (entity is EntityWithMultiImages entityWithMultiImages
                             && entityWithMultiImages.Images is { Length: > 0 })
                    {
                        await _uploadFileService.DeleteManyAsync(entityWithMultiImages.Images);
                    }
                }
            }

            _repository.RemoveRange(entities);
            await _unitOfWork.SaveChangeAsync();
        }
        #endregion
    }
}
