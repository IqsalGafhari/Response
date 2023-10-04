using API.Models;

namespace API.Utilities.Handlers;

public class GenerateHandler
{
    public static string GenerateNik(Employee employee)
    {
        
        string Nik = "111111";

        
        if (employee is not null)
        {
            
            int generateNik = int.Parse(employee.Nik);
            Nik = (generateNik + 1).ToString();
        }
        return Nik;
    }
}
