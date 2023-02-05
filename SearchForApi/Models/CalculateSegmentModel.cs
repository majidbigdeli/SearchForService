using System;
namespace SearchForApi.Models
{
    public class CalculateSegmentModel
    {
        public int StartSegment { get; set; }
        public int EndSegment { get; set; }
        public int SegmentCount { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int DurationTime { get; set; }
        public int StartBreathTime { get; set; }
        public int EndBreathTime { get; set; }
        public int OriginSurationTime { get; set; }
        public int ActualStartSegmentTime { get; set; }
        public int ActualEndSegmentTime { get; set; }
    }
}

