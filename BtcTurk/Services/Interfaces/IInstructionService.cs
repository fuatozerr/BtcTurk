using BtcTurk.Models;

namespace BtcTurk.Services.Interfaces
{
    public interface IInstructionService
    {
        public void Create();
        public List<Instruction> GetInstructions();

    }
}
