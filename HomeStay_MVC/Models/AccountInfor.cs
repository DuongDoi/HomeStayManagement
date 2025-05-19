using System.ComponentModel.DataAnnotations;

namespace HomeStay_MVC.Models
{
    public class AccountInfor
    {
        
         public string Id { get; set; }

        public string Users { get; set; }

         public string Pass { get; set; }
         public string Phone { get; set; }
         public string Email { get; set; }
         public string Name { get; set; }
         public string Card_Number { get; set; }
         public string Save_Code { get; set; }
         public string Role { get; set; }

         public string IsLock { get; set; }

         public string Avatar_Path { get; set; }
         public DateTime Create_At { get; set; }
         public DateTime Update_At { get; set; }

         public string Is_First_Login { get; set; }
         public string Homestays_Id { get; set; }
         public string Create_By { get; set; }
    }


}
