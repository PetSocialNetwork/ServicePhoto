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
    public class PetPhotoController : ControllerBase
    {
        private readonly PetPhotoService _petPhotoService;
        private readonly IHostEnvironment _environment; 
        private readonly IMapper _mapper;
        public PetPhotoController(PetPhotoService petProfileService, IMapper mapper, IHostEnvironment environment)
        {
            _petPhotoService = petProfileService ?? throw new ArgumentException(nameof(petProfileService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _environment = environment;
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<PetPhotoReponse>> AddPetPhotoAsync
            ([FromForm] Guid petId,
            [FromForm] Guid accountId,
            IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            var baseUrl = $"https://localhost:7216";
            var relativePath = $"/images/{fileName}";
            var fullUrl = $"{baseUrl}{relativePath}";

            var photo = new PetPhoto(Guid.NewGuid(), fullUrl, petId, accountId);
            var response = await _petPhotoService.AddPetPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(response);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<ActionResult<PetPhotoReponse>> AddAndSetPetPhotoAsync
            ([FromForm] Guid petId,
            [FromForm] Guid accountId,
            IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            var baseUrl = $"https://localhost:7216";
            var relativePath = $"images/{fileName}"; 
            var fullUrl = $"{baseUrl}/{relativePath}"; 
            var photo = new PetPhoto(Guid.NewGuid(), fullUrl, petId, accountId);
            var response = await _petPhotoService.AddAndSetPetPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(response);

        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            await _petPhotoService.DeletePetPhotoAsync(photoId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("[action]")]
        public async Task DeleteAllPetPhotosAsync(Guid petId, Guid accountId, CancellationToken cancellationToken)
        {
            await _petPhotoService.DeleteAllPetPhotosAsync(petId, accountId, cancellationToken);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PetPhotoReponse> GetPetPhotoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var photo = await _petPhotoService.GetPetPhotoByIdAsync(id, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(photo);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async Task<PetPhotoReponse?> GetMainPetPhotoAsync(Guid petId, Guid accountId, CancellationToken cancellationToken)
        {
            var photo = await _petPhotoService.FindMainPetPhotoAsync(petId, accountId, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(photo);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("[action]")]
        public async IAsyncEnumerable<PetPhotoReponse>? BySearchPetPhotosAsync(Guid petId, Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _petPhotoService.BySearchPetPhotosAsync(petId, accountId, cancellationToken))
            {
                var photoResponse = _mapper.Map<PetPhotoReponse>(photo);
                yield return photoResponse;
            }
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PhotoNotFoundException))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<PetPhotoReponse> SetMainPetPhotoAsync([FromBody] PetMainPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = await _petPhotoService
                .SetMainPetPhotoAsync(request.PetId, request.AccountId, request.PhotoId, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(photo);
        }
    }
}
