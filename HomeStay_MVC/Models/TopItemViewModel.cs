namespace HomeStay_MVC.Models
{
    public class TopItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Avatar { get; set; }
        public int UsedCount { get; set; }
    }
    public class DataReportModel
    {
        public string Date_value { get; set; }
        public int Total_in { get; set; }
        public int Total_out { get; set; }
    }
}
