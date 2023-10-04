using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingManagementApp.Models
{
    //membuat anotasi tabel dengan nama custom
    [Table("tb_m_roles")]
    public class Role : BaseEntity
    {
        //tidak boleh null, menamai kolom dan tipe data spesifik
        [Required, Column("name", TypeName = "nvarchar(100)")]
        public string Name { get; set; }
        public ICollection<AccountRole>? AccountRoles { get; set;}
    }
}
