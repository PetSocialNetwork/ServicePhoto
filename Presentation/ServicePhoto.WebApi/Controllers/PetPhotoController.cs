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
    public class PetPhotoController : ControllerBase
    {
        private readonly PetPhotoService _petPhotoService;
        private readonly IMapper _mapper;
        public PetPhotoController(PetPhotoService petPhotoService,
            IMapper mapper)
        {
            _petPhotoService = petPhotoService 
                ?? throw new ArgumentException(nameof(petPhotoService));
            _mapper = mapper 
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Удаляет фотографию питомца по идентификатору
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task DeletePetPhotoAsync
            (Guid photoId, CancellationToken cancellationToken)
        {
           await _petPhotoService.DeletePetPhotoAsync(photoId, cancellationToken);
        }

        /// <summary>
        ///  Удаляет все фотографии питомца
        /// </summary>
        /// <param name="petId">Идентификатор питомца</param>
        /// <param name="profileId">Идентификатор профиля пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpDelete("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task DeleteAllPetPhotosAsync
            (Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            await _petPhotoService.DeleteAllPetPhotosAsync(petId, profileId, cancellationToken);
        }

        /// <summary>
        /// Возвращает фотографию питомца по идентификатору
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PetPhotoReponse> GetPetPhotoByIdAsync
            (Guid photoId, CancellationToken cancellationToken)
        {
            var photo = await _petPhotoService.GetPetPhotoByIdAsync(photoId, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(photo);
        }

        /// <summary>
        ///  Возвращает главную фотографию питомца
        /// </summary>
        /// <param name="petId">Идентификатор питомца</param>
        /// <param name="profileId">Идентификатор профиля пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<PetPhotoReponse?> GetMainPetPhotoAsync
            (Guid petId, Guid profileId, CancellationToken cancellationToken)
        {
            var photo = await _petPhotoService.FindMainPetPhotoAsync(petId, profileId, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(photo);
        }

        /// <summary>
        /// Добавляет и устанавливает главную фотографию питомца
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PetPhotoReponse>> AddAndSetPetPhotoAsync
            ([FromBody] AddPetPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = _mapper.Map<PetPhoto>(request);
            var response = await _petPhotoService.AddAndSetPetPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(response);
        }

        /// <summary>
        /// Добавляет фотографию питомца
        /// </summary>
        /// <param name="request">Модлеь запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PetPhotoReponse>> AddPetPhotoAsync
            ([FromBody] AddPetPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = _mapper.Map<PetPhoto>(request);
            var response = await _petPhotoService.AddPetPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(response);
        }

        /// <summary>
        /// Устанавливает главную фотографию питомца
        /// </summary>
        /// <param name="request">Модель запроса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<PetPhotoReponse> SetMainPetPhotoAsync
            ([FromBody] PetMainPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = await _petPhotoService
                .SetMainPetPhotoAsync(request.PetId, request.ProfileId, request.PhotoId, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(photo);
        }

        /// <summary>
        /// Возвращает все фотографии питомца
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpPost("[action]")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<List<PetPhotoReponse>> BySearchAsync
            ([FromBody] PetPhotoBySearchRequest request, CancellationToken cancellationToken)
        {
            var options = _mapper.Map<PaginationOptions>(request.Pagination);
            var photos = await _petPhotoService.BySearchAsync
                (request.PetId, request.ProfileId, options, cancellationToken);
            return _mapper.Map<List<PetPhotoReponse>>(photos);
        }
    }
}
