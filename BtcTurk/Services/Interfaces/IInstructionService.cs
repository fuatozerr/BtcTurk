using BtcTurk.Dto;
using BtcTurk.Models;

namespace BtcTurk.Services.Interfaces
{
    public interface IInstructionService
    {
        public Task<Response<bool>> Create(InstructionDto request);
        public List<Instruction> GetInstructions();

    }
}
