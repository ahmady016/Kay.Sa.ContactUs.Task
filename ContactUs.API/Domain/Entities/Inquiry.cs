using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
  [Table("Inquiries")]
  public class Inquiry : Entity<long>
  {
    [Column("Email", TypeName = "varchar(150)")]
    public string Email { get; set; }

    [Column("Phone", TypeName = "char(11)")]
    public string Phone { get; set; }

    [Column("Message", TypeName="nvarchar(1000)")]
    public string Message { get; set; }
  }
}
