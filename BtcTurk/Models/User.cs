namespace BtcTurk.Models
{
    public sealed class User : BaseEntity
    {
        public int PhoneNumber { get; set; } //sms notification 
        public string Email { get; set; } //email notification 
        public string Phone { get; set; } //push notification 
        //public List<Instruction> Instructions { get; set; }
    }
}
