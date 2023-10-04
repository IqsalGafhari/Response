using API.Repositories;
using BookingManagementApp.Contracts;
using BookingManagementApp.Data;
using BookingManagementApp.Models;

namespace BookingManagementApp.Repositories
{
    public class AccountRepository : GeneralRepository<Account> , IAccountRepository
    {
            public AccountRepository(BookingManagementDbContext context) : base(context) { }
            
    }
}
