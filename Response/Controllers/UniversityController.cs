using API.Contracts;
using API.DTO.Universities;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
//using API.Utilities.Handlers.Exceptions;
namespace API.Controllers;

[ApiController] 
[Route("api/[controller]")] 
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;
    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet] 
    public IActionResult GetAll()
    {
        var result = _universityRepository.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = result.Select(x => (UniversityDto)x);
        return Ok(new ResponseOkHandler<IEnumerable<UniversityDto>>(data));
    }

    [HttpGet("{guid}")] 
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _universityRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        return Ok(new ResponseOkHandler<UniversityDto>((UniversityDto)result));
    }

    [HttpPost]
    public IActionResult Create(CreateUniversityDto universityDto)
    {
        try
        {
            var result = _universityRepository.Create(universityDto);
            return Ok(new ResponseOkHandler<UniversityDto>((UniversityDto)result));
        }
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }

    [HttpPut] 
    public IActionResult Update(UniversityDto universityDto)
    {
        try
        {
            
            var entity = _universityRepository.GetByGuid(universityDto.Guid);
            if (entity is null)
            {
                
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            University toUpdate = universityDto;
            toUpdate.CreatedDate = entity.CreatedDate;
            _universityRepository.Update(toUpdate);
            return Ok(new ResponseOkHandler<string>("Data Updated"));
        }
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to update data",
                Error = ex.Message
            });
        }
    }

    [HttpDelete] 
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var entity = _universityRepository.GetByGuid(guid);
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            _universityRepository.Delete(entity);
            return Ok(new ResponseOkHandler<string>("Data Deleted"));
        }
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to delete data",
                Error = ex.Message
            });
        }
    }
}
