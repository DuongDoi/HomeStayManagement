namespace HomeStay_MVC.Models
{
    public class UserInfor
    {
        public string Users { get; set; }
        public string Pass { get; set; }
        public string Email { get; set; }
        public string Save_Code { get; set; }
        public string Role { get; set; }

        public string IsLock { get; set; }
        public string Is_First_Login { get; set; }
        public string Homestays_Id { get; set; }
        public string Create_By { get; set; }
    }
}
