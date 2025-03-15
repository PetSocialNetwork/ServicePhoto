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
    public class PetPhotoController : ControllerBase
    {
        private readonly PetPhotoService _petPhotoService;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public PetPhotoController(PetPhotoService petPhotoService,
            IFileService fileService,
            IMapper mapper)
        {
            _petPhotoService = petPhotoService ?? throw new ArgumentException(nameof(petPhotoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<PetPhotoReponse>> AddPetPhotoAsync
            ([FromForm] Guid petId,
            [FromForm] Guid accountId,
            [Required] IFormFile file, CancellationToken cancellationToken)
        {
            if (file.Length == 0)
            {
                return BadRequest("Нет данных файла.");
            }

            var url = await _fileService.UploadPhotoAsync(file, cancellationToken);
            var photo = new PetPhoto(Guid.NewGuid(), url, petId, accountId);
            var response = await _petPhotoService.AddPetPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(response);
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<PetPhotoReponse>> AddAndSetPetPhotoAsync
            ([FromForm] Guid petId,
            [FromForm] Guid accountId,
            [Required] IFormFile file, CancellationToken cancellationToken)
        {
            if (file.Length == 0)
            {
                return BadRequest("Нет данных файла.");
            }

            var url = await _fileService.UploadPhotoAsync(file, cancellationToken);
            var photo = new PetPhoto(Guid.NewGuid(), url, petId, accountId);
            var response = await _petPhotoService.AddAndSetPetPhotoAsync(photo, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(response);
        }

        [HttpDelete("[action]")]
        public async Task DeletePetPhotoAsync(Guid photoId, CancellationToken cancellationToken)
        {
            var path = await _petPhotoService.DeletePetPhotoAsync(photoId, cancellationToken);
            _fileService.DeleteFile(path);
        }

        [HttpDelete("[action]")]
        public async Task DeleteAllPetPhotosAsync(Guid petId, Guid accountId, CancellationToken cancellationToken)
        {
            var paths = await _petPhotoService.DeleteAllPetPhotosAsync(petId, accountId, cancellationToken);
            _fileService.DeleteFiles(paths);
        }

        [HttpGet("[action]")]
        public async Task<PetPhotoReponse> GetPetPhotoByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var photo = await _petPhotoService.GetPetPhotoByIdAsync(id, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(photo);
        }

        [HttpGet("[action]")]
        public async Task<PetPhotoReponse?> GetMainPetPhotoAsync(Guid petId, Guid accountId, CancellationToken cancellationToken)
        {
            var photo = await _petPhotoService.FindMainPetPhotoAsync(petId, accountId, cancellationToken);
            return _mapper.Map<PetPhotoReponse>(photo);
        }

        [HttpGet("[action]")]
        public async IAsyncEnumerable<PetPhotoReponse> BySearchAsync(Guid petId, Guid accountId, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var photo in _petPhotoService.BySearchAsync(petId, accountId, cancellationToken))
            {
                var photoResponse = _mapper.Map<PetPhotoReponse>(photo);
                yield return photoResponse;
            }
        }

        [HttpPost("[action]")]
        public async Task<PetPhotoReponse> SetMainPetPhotoAsync([FromBody] PetMainPhotoRequest request, CancellationToken cancellationToken)
        {
            var (photo, oldFilePath) = await _petPhotoService
                .SetMainPetPhotoAsync(request.PetId, request.AccountId, request.PhotoId, cancellationToken);
            _fileService.DeleteFile(oldFilePath);
            return _mapper.Map<PetPhotoReponse>(photo);
        }
    }
}
