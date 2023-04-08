using BtcTurk.Dto;
using BtcTurk.Models;
using BtcTurk.Models.Response;

namespace BtcTurk.Services.Interfaces
{
    public interface IInstructionService
    {
        public BaseResponse Create(InstructionDto request);
        public List<Instruction> GetInstructions();

    }
}
