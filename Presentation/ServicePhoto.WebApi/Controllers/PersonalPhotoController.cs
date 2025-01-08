using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Services;
using ServicePhoto.WebApi.Models.Requests;
using ServicePhoto.WebApi.Models.Responses;
using System.Runtime.CompilerServices;

namespace ServicePhoto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalPhotoController : ControllerBase
    {
        private readonly PersonalPhotoService _personalPhotoService;
        private readonly IMapper _mapper;
        public PersonalPhotoController(PersonalPhotoService personalPhotoService, IMapper mapper)
        {
            _personalPhotoService = personalPhotoService ?? throw new ArgumentException(nameof(personalPhotoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<PersonalPhotoResponse> AddAndSetPersonalPhotoAsync(PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = _mapper.Map<PersonalPhoto>(request);

            var addPhoto = await _personalPhotoService.AddAndSetPersonalPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(addPhoto);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<PersonalPhotoResponse> AddPersonalPhotoAsync(PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = _mapper.Map<PersonalPhoto>(request);

            var addPhoto = await _personalPhotoService.AddPersonalPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(addPhoto);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<PersonalPhotoResponse>? BySearchPersonalPhotosAsync(Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _personalPhotoService.BySearchPersonalPhotosAsync(accountId, cancellationToken))
            {
                var photoResponse = _mapper.Map<PersonalPhotoResponse>(photo);
                yield return photoResponse;
            }
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse> GetPersonalPhotoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService.GetPersonalPhotoByIdAsync(id, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse> GetMainPersonalPhotoAsync(CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService.GetMainPersonalPhotoAsync(cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeletePersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            await _personalPhotoService.DeletePersonalPhotoAsync(photoId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse> SetMainPersonalPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService.SetMainPersonalPhotoAsync(photoId, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }
    }
}
