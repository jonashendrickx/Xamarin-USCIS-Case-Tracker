using System;

namespace USCISCaseTracker.UWP.Background
{
    public sealed class BackgroundUpdateInterval
    {
        public BackgroundUpdateInterval(uint minutes)
        {
            Value = minutes;
        }

        public uint Value { get; set; }

        public override string ToString()
        {
            var date = new DateTime(1 ,1 ,1, (int)Value / 60, (int)Value % 60, 0);
            return $"Every {date:HH:mm}";
        }
    }
}
