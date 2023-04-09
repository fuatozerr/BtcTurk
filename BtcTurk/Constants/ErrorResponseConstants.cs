namespace BtcTurk.Constants
{
    public class ErrorResponseConstants
    {
        public const string ActiveInstruction = "A user can have only 1 active instruction.";
        public const string InstructionNotFound = "Instruction Not Found";
        public const string InstructionNotFoundById = "The instruction of the sent Id could not be found";
        public const string DontMatchUserAndInstruction = " Instruction information and User information do not match.";
        public const string DatabaseError = "There was a problem on the database side.";
        public const string ValidationError = "Validation Error";

    }
}
