using AutoMapper;
using Domain;

namespace API
{
  public class AutoMapperProfile : Profile
  {
    public AutoMapperProfile()
    {
      CreateMap<InquiryDto, Inquiry>().ReverseMap();
    }
  }
}
