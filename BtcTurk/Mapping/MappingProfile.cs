using AutoMapper;
using BtcTurk.Dto;
using BtcTurk.Models;

namespace BtcTurk.Mapping

{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InstructionDto, Instruction>()
                        .ReverseMap();

            CreateMap<NotificationDto, Instruction>()
                        .ReverseMap();

        }
    }
}
