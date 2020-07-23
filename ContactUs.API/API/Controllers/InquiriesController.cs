using AutoMapper;
using Domain;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace API
{
  public class InquiriesController : CRUDController<Inquiry, long, InquiryDto>
  {
    private readonly IRepository _repository;
    public InquiriesController(IRepository repository, IMapper mapper, IConfiguration config)
      : base(repository, mapper, config)
    {
      _repository = repository;
    }

  }
}
