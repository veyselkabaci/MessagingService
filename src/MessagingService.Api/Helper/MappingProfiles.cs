using AutoMapper;
using MessagingService.Api.Models;

namespace MessagingService.Api.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<MessageRequest, Message>();
    }
}