using System;

namespace TeachingPlatformApp.WswPlatform
{
    public class TeachingCommandBuilder
    {

        TeachingCommand command;

        public TeachingCommandBuilder()
        {
            command = new TeachingCommand
            {
                HeaderOne = 0xAA,
                HeaderTwo = 0xBB,
                ExperimentIndex = 0,
                IsStart = 0
            };
        }

        public TeachingCommandBuilder(int index, bool isStart)
        {
            command = new TeachingCommand
            {
                HeaderOne = 0xAA,
                HeaderTwo = 0xBB
            };
            SetExperimentIndex(index);
            SetIsStart(isStart);
        }

        public TeachingCommandBuilder SetExperimentIndex(int index)
        {
            command.ExperimentIndex = Convert.ToByte(index);
            return this;
        }

        public TeachingCommandBuilder SetIsStart(bool isStart)
        {
            command.IsStart = (byte)(isStart == true ? 0x01 : 0x00);
            return this;
        }

        public TeachingCommand Build() => command;

        public byte[] BuildCommandBytes() => Utils.StructHelper.StructToBytes(command);

    }

}
