using API.Contracts;
using API.DTO.Accounts;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
//using API.Utilities.Handlers.Exceptions;
namespace API.Controllers;

[ApiController] 
[Route("api/[controller]")] 

public class AccountController : ControllerBase
{
    
    private readonly IAccountRepository _accountRepository;
    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet] 
    public IActionResult GetAll()
    {
        
        var result = _accountRepository.GetAll();
        if (!result.Any())
        {
           
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        
        var data = result.Select(x => (AccountDto)x);
        
        return Ok(new ResponseOkHandler<IEnumerable<AccountDto>>(data));
    }

    [HttpGet("{guid}")] 
    public IActionResult GetByGuid(Guid guid)
    {
       
        var result = _accountRepository.GetByGuid(guid);
        if (result is null)
        {
            
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }
        
        return Ok(new ResponseOkHandler<AccountDto>((AccountDto)result));
    }

    [HttpPost] 
    public IActionResult Create(CreateAccountDto accountDto)
    {
        try
        {
            Account toCreate = accountDto;            
            toCreate.Password = HashHandler.HashPassword(accountDto.Password);            
            var result = _accountRepository.Create(toCreate);           
            return Ok(new ResponseOkHandler<AccountDto>((AccountDto)result));
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
    public IActionResult Update(AccountDto accountDto)
    {
        try
        {
            
            var entity = _accountRepository.GetByGuid(accountDto.Guid);
            if (entity is null)
            {
                
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            
            Account toUpdate = accountDto;
            toUpdate.CreatedDate = entity.CreatedDate;
            toUpdate.Password = HashHandler.HashPassword(accountDto.Password);
            _accountRepository.Update(toUpdate);
            return Ok(new ResponseOkHandler<string>("Data Updated"));
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

    [HttpDelete] 
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var entity = _accountRepository.GetByGuid(guid);
            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            _accountRepository.Delete(entity);
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
