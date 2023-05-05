namespace POGOMVC.Models
{
    public class GraphColumns
    {
        public int Key { get; set; }
        public decimal FloatValue { get; set; }
        public DateTime TimeStampValue { get; set; }
        public decimal IgnoreColumn { get; set; }
    }
    public class BarChart
    {
        public List<GraphColumns> Data { get; set; }
        public List<GraphColumns> Key2 { get; set; }
        public List<GraphColumns> Key3 { get; set; }
        public List<GraphColumns> Key4 { get; set; }
        public List<GraphColumns> Key5 { get; set; }
        public List<GraphColumns> Key6 { get; set; }
        public DateTime[] XAxis { get; set; }
        //public string[] XAxis { get; set; }
    }
}
