using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServicePhoto.Domain.Entities;
using ServicePhoto.Domain.Services;
using ServicePhoto.WebApi.Models.Requests;
using ServicePhoto.WebApi.Models.Responses;
using ServicePhoto.WebApi.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServicePhoto.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalPhotoController : ControllerBase
    {
        private readonly PersonalPhotoService _personalPhotoService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public PersonalPhotoController(PersonalPhotoService personalPhotoService,
            IFileService fileService,
            IMapper mapper)
        {
            _personalPhotoService = personalPhotoService ?? throw new ArgumentException(nameof(personalPhotoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<PersonalPhotoResponse>> AddPersonalPhotoAsync
            ([FromForm] Guid profileId,
            [Required] IFormFile file, CancellationToken cancellationToken)
        {
            if (file.Length == 0)
            {
                return BadRequest("Нет данных файла.");
            }

            var url = await _fileService.UploadPhotoAsync(file, cancellationToken);
            var photo = new PersonalPhoto(Guid.NewGuid(), profileId, url);
            var response = await _personalPhotoService.AddPersonalPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(response);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<PersonalPhotoResponse>> AddAndSetPersonalPhotoAsync
            ([FromForm] Guid profileId,
            [Required] IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Нет данных файла.");
            }

            var url = await _fileService.UploadPhotoAsync(file, cancellationToken);
            var photo = new PersonalPhoto(Guid.NewGuid(), profileId, url);
            var response = await _personalPhotoService.AddAndSetPersonalPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(response);

        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeletePersonalPhotoAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            var path = await _personalPhotoService.DeletePersonalPhotoAsync(photoId, cancellationToken);
            _fileService.DeleteFile(path);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeleteAllPersonalPhotosAsync([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            var paths = await _personalPhotoService.DeleteAllPersonalPhotosAsync(profileId, cancellationToken);
            _fileService.DeleteFiles(paths);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse> GetPersonalPhotoByIdAsync([FromQuery] Guid photoId, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService.GetPersonalPhotoByIdAsync(photoId, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PersonalPhotoResponse?> GetMainPersonalPhotoAsync([FromQuery] Guid profileId, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService.FindMainPersonalPhotoAsync(profileId, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<PersonalPhotoResponse>? BySearchPersonalPhotosAsync([FromQuery] Guid profileId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _personalPhotoService.BySearchPersonalPhotosAsync(profileId, cancellationToken))
            {
                var photoResponse = _mapper.Map<PersonalPhotoResponse>(photo);
                yield return photoResponse;
            }
        }





        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<PersonalPhotoResponse> SetMainPersonalPhotoAsync([FromBody] PersonalPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = await _personalPhotoService
                .SetMainPersonalPhotoAsync(request.ProfileId, request.PhotoId, cancellationToken);
            return _mapper.Map<PersonalPhotoResponse>(photo);
        }


    }
}
