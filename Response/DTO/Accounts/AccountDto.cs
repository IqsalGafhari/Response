using API.Models;

namespace API.DTO.Accounts;


public class AccountDto
{
    public Guid Guid { get; set; } 
    public int Otp { get; set; } 
    public bool IsUsed { get; set; } 
    public DateTime ExpiredTime { get; set; }     
    public static explicit operator AccountDto(Account account)
    {
        return new AccountDto
        {
            Guid = account.Guid,
            Otp = account.Otp,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime,
        };
    }
}
