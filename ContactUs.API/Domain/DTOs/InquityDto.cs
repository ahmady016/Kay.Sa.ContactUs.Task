using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
  public class InquiryDto
  {
    public long? Id { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MinLength(10)]
    [MaxLength(150)]
    public string Email { get; set; }

    [Required]
    [MinLength(11)]
    [MaxLength(11)]
    public string Phone { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(1000)]
    public string Message { get; set; }
  }
}
