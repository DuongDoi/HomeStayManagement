using System.ComponentModel.DataAnnotations;

namespace HomeStay_MVC.Models
{
    public class CreatePIN
    {
        public string Users { get; set; }
        public string PIN1 { get; set; }
        public string PIN2 { get; set; }
    }
}
