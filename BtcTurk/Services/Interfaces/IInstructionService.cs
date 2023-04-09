using BtcTurk.Dto;

namespace BtcTurk.Services.Interfaces
{
    public interface IInstructionService
    {
        Task<Response<bool>> Create(InstructionDto request);
        Task<Response<InstructionDto>> GetInstructionById(int userId, int instructionId);
        Task<Response<bool>> CancelInstruction(int userId, CancelInstructionDto request);
        Task<Response<NotificationDto>> GetNotificationById(int userId, int instructionId);

    }
}
