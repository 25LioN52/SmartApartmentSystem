namespace SmartApartmentSystem.Data.Models
{
    public class Schedule
    {
        public byte ModuleId { get; set; }
        public int Day { get; set; }
        public byte Hour { get; set; }
        public byte Minutes { get; set; }
        public byte Status { get; set; }

        public virtual Module Module { get; set; }
    }
}
