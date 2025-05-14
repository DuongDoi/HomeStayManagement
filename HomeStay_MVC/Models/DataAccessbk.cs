using System.Data;

namespace HomeStay_MVC.Model
{
    public class DataAccessbk
    {
        public  DataAccessbk()
        { }


        public static DataSet Login(string user,string pass)
        {
            DataSet ds = new DataSet();

            //ket noi vao firebase

            string url_db = CommonFunction.GetValuesAppSetting("config", "firebaseURL");





            return ds;
        }

    }




}
