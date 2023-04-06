using BtcTurk.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BtcTurk.EntityConfiguration
{
    public class InstructionEntityConfiguration : BaseEntityConfiguration<Instruction>
    {
        public override void Configure(EntityTypeBuilder<Instruction> builder)
        {
            base.Configure(builder);
        }
    }
}
