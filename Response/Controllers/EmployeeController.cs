using API.Contracts;
using API.DTO.Employees;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
//using API.Utilities.Handlers.Exceptions;
namespace API.Controllers;

[ApiController] 
[Route("api/[controller]")] 
public class EmployeeController : ControllerBase
{
    
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _employeeRepository.GetAll();
        if (!result.Any())
        {
            
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = result.Select(x => (EmployeeDto)x);
        return Ok(new ResponseOkHandler<IEnumerable<EmployeeDto>>(data));
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _employeeRepository.GetByGuid(guid);
        if (result is null)
        {
            
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
       
        return Ok(new ResponseOkHandler<EmployeeDto>((EmployeeDto)result));
    }

    [HttpPost]
    public IActionResult Create(CreateEmployeeDto employeeDto)
    {
        try
        {
            Employee toCreate = employeeDto;
            toCreate.Nik = GenerateHandler.GenerateNik(_employeeRepository.GetLastNik());
            var result = _employeeRepository.Create(toCreate);
            return Ok(new ResponseOkHandler<EmployeeDto>((EmployeeDto)result));
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
    public IActionResult Update(EmployeeDto employeeDto)
    {
        try
        {
            
            var entity = _employeeRepository.GetByGuid(employeeDto.Guid);
            if (entity is null)
            {
                
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            Employee toUpdate = employeeDto; 
            toUpdate.Nik = entity.Nik; 
            toUpdate.CreatedDate = entity.CreatedDate; 
            _employeeRepository.Update(toUpdate); 
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
            
            var entity = _employeeRepository.GetByGuid(guid);
            if (entity is null)
            {
                
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            
            _employeeRepository.Delete(entity);
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
