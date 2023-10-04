using API.Contracts;
using API.DTO.AccountRoles;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
//using API.Utilities.Handlers.Exceptions;
namespace API.Controllers;

[ApiController] 
[Route("api/[controller]")] 

public class AccountRoleController : ControllerBase
{
    
    private readonly IAccountRoleRepository _accountRoleRepository;
    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    [HttpGet] 
    public IActionResult GetAll()
    {
        
        var result = _accountRoleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        var data = result.Select(x => (AccountRoleDto)x);
        return Ok(new ResponseOkHandler<IEnumerable<AccountRoleDto>>(data));
    }

    [HttpGet("{guid}")] 
    public IActionResult GetByGuid(Guid guid)
    {
        
        var result = _accountRoleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
   
        return Ok(new ResponseOkHandler<AccountRoleDto>((AccountRoleDto)result));
    }

    [HttpPost] 
    public IActionResult Create(CreateAccountRoleDto accountRoleDto)
    {
        try
        {
            
            var result = _accountRoleRepository.Create(accountRoleDto);
            
            return Ok(new ResponseOkHandler<AccountRoleDto>((AccountRoleDto)result));
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
    public IActionResult Update(AccountRoleDto accountRoleDto)
    {
        try
        {
            
            var entity = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);
            if (entity is null)
            {
                
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            
            AccountRole toUpdate = accountRoleDto;
            toUpdate.CreatedDate = entity.CreatedDate;
            _accountRoleRepository.Update(toUpdate);
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
            var entity = _accountRoleRepository.GetByGuid(guid);
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }            
            _accountRoleRepository.Delete(entity);
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
