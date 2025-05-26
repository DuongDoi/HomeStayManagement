using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeStay_MVC.Models
{
    public class RegisterModel
    {
        public string Users { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? role { get; set; }
        public IEnumerable<SelectListItem>? RoleOptions { get; set; }
        public List<SelectListItem> HomeStayOptions { get; set; } = new List<SelectListItem>();
        public string HOMESTAYS_ID { get; set; }
    }
}
