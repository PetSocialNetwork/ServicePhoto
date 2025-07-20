using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Services;
using ServicePhoto.Domain.Shared;
using ServicePhoto.WebApi.Models.Requests;
using ServicePhoto.WebApi.Models.Responses;

namespace ServicePhoto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalPhotoController : ControllerBase
    {
        private readonly PersonalPhotoService _personalPhotoService;
        private readonly IMapper _mapper;
        public PersonalPhotoController(PersonalPhotoService personalPhotoService,
            IMapper mapper)
        {
            _personalPhotoService = personalPhotoService 
                ?? throw new ArgumentException(nameof(personalPhotoService));
            _mapper = mapper 
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Удаляет фотографию пользователя по идентификатору
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeletePersonalPhotoAsync
            ([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            await _personalPhotoService.DeletePersonalPhotoAsync(photoId, cancellationToken);
        }

        /// <summary>
        /// Удаляет все фотографии пользователя
        /// </summary>
        /// <param name="profileId">Идентификатор профиля</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task DeleteAllPersonalPhotosAsync
            ([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            await _personalPhotoService.DeleteAllPersonalPhotosAsync(profileId, cancellationToken);
        }

        /// <summary>
        /// Возвращает фотографию пользователя по идентификатору
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PersonalPhotoResponse> GetPersonalPhotoByIdAsync
            ([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService.GetPersonalPhotoByIdAsync(photoId, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }

        /// <summary>
        /// Возвращает главную фотографию пользователя
        /// </summary>
        /// <param name="profileId">Идентификатор профиля</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<PersonalPhotoResponse?> GetMainPersonalPhotoAsync
            ([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService
                .FindMainPersonalPhotoAsync(profileId, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }

        /// <summary>
        /// Устанавливает главную фотографию пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PersonalPhotoResponse> SetMainPersonalPhotoAsync
            ([FromBody] PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService
                .SetMainPersonalPhotoAsync(request.ProfileId, request.PhotoId, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }

        /// <summary>
        /// Возвращает все фотографии пользователя
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<List<PersonalPhotoResponse>> BySearchAsync
            ([FromBody] PersonalPhotoBySearchRequest request, CancellationToken cancellationToken)
        {
            var options = _mapper.Map<PaginationOptions>(request.Pagination);
            var photos = await _personalPhotoService.BySearchPhotosAsync
                (request.ProfileId, options, cancellationToken);
            return _mapper.Map<List<PersonalPhotoResponse>>(photos);
        }

        /// <summary>
        /// Получить главные фотографии пользователей
        /// </summary>
        /// <param name="profileIds">Идентификаторы пользователей</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<List<PersonalPhotoResponse>> GetMainPersonalPhotoByIdsAsync
            ([FromBody] Guid[] profileIds, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService.FindMainPersonalPhotosByIdsAsync(profileIds, cancellationToken);
            return _mapper.Map<List<PersonalPhotoResponse>>(photo);
        }

        /// <summary>
        /// Добавляет и устанавливает главную фотографию пользователя 
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PersonalPhotoResponse>> AddAndSetPersonalPhotoAsync
            ([FromBody] AddPersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = _mapper.Map<PersonalPhoto>(request);
            var response = await _personalPhotoService.AddAndSetPersonalPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(response);
        }

        /// <summary>
        ///  Добавляет фотографию пользователю
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PersonalPhotoResponse>> AddPersonalPhotoAsync
            ([FromBody] AddPersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = _mapper.Map<PersonalPhoto>(request);
            var response = await _personalPhotoService.AddPersonalPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(response);
        }
    }
}
