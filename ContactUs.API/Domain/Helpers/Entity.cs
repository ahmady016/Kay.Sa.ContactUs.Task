using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
  public class Entity<T>
  {
    [Key]
    public T Id { get; set; }

    [Column("Name", TypeName="nvarchar(150)")]
    public string Name { get; set; } = null;

    [Column("Notes", TypeName="nvarchar(1000)")]
    public string Notes { get; set; } = null;

    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(2);
    public DateTime? ModifiedAt { get; set; } = null;

    [Column("CreatedBy", TypeName="varchar(100)")]
    public string CreatedBy { get; set; } = "App_Dev";

    [Column("ModifiedBy", TypeName="varchar(100)")]
    public string ModifiedBy { get; set; } = null;

  }
}
