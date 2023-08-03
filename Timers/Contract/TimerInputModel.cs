namespace Timers.Contract
{
    public class TimerInputModel
    {
        public int Id { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public string WebUrl { get; set; }
    }
}
