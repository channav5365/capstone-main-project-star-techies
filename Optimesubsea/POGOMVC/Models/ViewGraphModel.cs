namespace POGOMVC.Models
{
    public class ViewGraphModel
    {
        public int Id { get; set; }
        public GraphColumns GraphColumns { get; set; }
        public BarChart BarChart { get; set; }
        public string ProjectName { get; set; }
        public bool IsUploadAvailable { get; set; }
        public bool IsDownloadAvailable { get; set; }
        public ViewGraphModel()
        {
            IsUploadAvailable = false;
            IsDownloadAvailable = false;
        }
    }
}
