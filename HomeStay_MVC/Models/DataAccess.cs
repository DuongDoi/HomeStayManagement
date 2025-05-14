using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Configuration;
using HomeStay_MVC.Model;
namespace ResfullApi.Models
{
    public class DataAccess
    {
       
        public DataAccess()
        {

        }

        //coi nhu update cac reqeust tu MPS vao DB

        
        //USER
        public static DataSet LOGIN(string user,string pass)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "API_USES_PKG.USERS_LOGIN";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_pass", OracleDbType.Varchar2),
                                
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = pass;
           

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_INSERT(string user, string pass,string phone, string email,string name)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "API_USES_PKG.USERS_INSERT_MANAGER";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_pass", OracleDbType.Varchar2),
                                new OracleParameter("v_phone", OracleDbType.Varchar2),
                                new OracleParameter("v_email", OracleDbType.Varchar2),
                                new OracleParameter("v_name", OracleDbType.NVarchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = pass;
            parms[2].Value = phone;
            parms[3].Value = email;
            parms[4].Value = name;


            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_RESET_PASSWORD(string user, string email, string save_code,string pass)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "API_USES_PKG.USERS_UPDATE_PASS";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_email", OracleDbType.Varchar2),
                                new OracleParameter("v_save_code", OracleDbType.NVarchar2),
                                new OracleParameter("v_pass", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = email;
            parms[2].Value = save_code;
            parms[3].Value = pass;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_UPDATE(string user, string pass,string phone,string email,string name, string type)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "API_PKG.USERS_UPDATE";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_pass", OracleDbType.Varchar2),
                                new OracleParameter("v_phone", OracleDbType.Varchar2),
                                new OracleParameter("v_email", OracleDbType.Varchar2),
                                new OracleParameter("v_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_type", OracleDbType.NVarchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = pass;
            parms[2].Value = phone;
            parms[3].Value = email;
            parms[4].Value = name;
            parms[5].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_GET_LIST(string user)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "API_USES_PKG.USERS_GET_LIST_MANAGER";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.NVarchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;

            return getDataFromProcedure(str, "", parms);
        }
        //HOMESTAY
        public static DataSet HOMESTAYS_INSERT(string name, string card, string address, string description, string manager,string phone,string user_id)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "HOMESTAYS_PKG.HOMESTAYS_INSERT";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_card", OracleDbType.Varchar2),
                                new OracleParameter("v_address", OracleDbType.NVarchar2),
                                new OracleParameter("v_description", OracleDbType.NVarchar2),
                                new OracleParameter("v_manager", OracleDbType.NVarchar2),
                                new OracleParameter("v_phone", OracleDbType.Varchar2),
                                new OracleParameter("v_user_id", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = name;
            parms[1].Value = card;
            parms[2].Value = address;
            parms[3].Value = description;
            parms[4].Value = manager;
            parms[5].Value = phone;
            parms[6].Value = user_id;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet HOMESTAYS_UPDATE(string name, string card, string address, string description, string manager, string phone, string user_id, string type)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "HOMESTAYS_PKG.HOMESTAYS_UPDATE";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_card", OracleDbType.Varchar2),
                                new OracleParameter("v_address", OracleDbType.NVarchar2),
                                new OracleParameter("v_description", OracleDbType.NVarchar2),
                                new OracleParameter("v_manager", OracleDbType.NVarchar2),
                                new OracleParameter("v_phone", OracleDbType.Varchar2),
                                new OracleParameter("v_user_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.NVarchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = name;
            parms[1].Value = card;
            parms[2].Value = address;
            parms[3].Value = description;
            parms[4].Value = manager;
            parms[5].Value = phone;
            parms[6].Value = user_id;
            parms[7].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet HOMESTAYS_GET_LIST(string name,string user_id,string type)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "HOMESTAYS_PKG.homestays_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_user_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = name;
            parms[1].Value = user_id;
            parms[2].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        //CUSTOMER
        public static DataSet CUSTOMERS_INSERT(string customer_card_id, string customer_name, string customer_phone, string customer_address, string users_id)
        {
            string str;
            str = "";
            str = "CUSTOMERS_PKG.CUSTOMERS_INSERT";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_customer_card_id", OracleDbType.Varchar2),
                                new OracleParameter("v_customer_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_customer_phone", OracleDbType.Varchar2),
                                new OracleParameter("v_customer_address", OracleDbType.NVarchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = customer_card_id;
            parms[1].Value = customer_name;
            parms[2].Value = customer_phone;
            parms[3].Value = customer_address;
            parms[4].Value = users_id;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet CUSTOMERS_UPDATE(string customer_card_id, string customer_name, string customer_phone, string customer_address,string users_id, string type)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "CUSTOMERS_PKG.CUSTOMERS_UPDATE";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_customer_card_id", OracleDbType.Varchar2),
                                new OracleParameter("v_customer_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_customer_phone", OracleDbType.Varchar2),
                                new OracleParameter("v_customer_address", OracleDbType.NVarchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.NVarchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = customer_card_id;
            parms[1].Value = customer_name;
            parms[2].Value = customer_phone;
            parms[3].Value = customer_address;
            parms[4].Value = users_id;
            parms[5].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet CUSTOMERS_GET_LIST(string customer_card_id, string users_id, string type)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "customers_pkg.customers_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_customer_card_id", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = customer_card_id;
            parms[1].Value = users_id;
            parms[2].Value = type;

            return getDataFromProcedure(str, "", parms);
        }

        //ROOM
        public static DataSet ROOMS_GET_LIST(string ht_id, string room_name, string type)
        {
            // {"title":"xxxx","serviceId":"30","content":"cong hoa xa hoi chu nghia","dateSend":"01/12/2020 01:01:10"}
            string str;
            str = "";
            str = "rooms_pkg.rooms_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_homestays_id", OracleDbType.Varchar2),
                                new OracleParameter("v_room_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = ht_id;
            parms[1].Value =room_name;
            parms[2].Value = type;

            return getDataFromProcedure(str, "", parms);
        }


























































        public static DataSet getDataFromProcedure(string sSQL, string sTableName, params OracleParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            OracleConnection dbConnection = getPoolingConnection();
            OracleDataAdapter dataAdapter;
            dataAdapter = new OracleDataAdapter();
            try
            {
                dataAdapter.SelectCommand = new OracleCommand(sSQL, dbConnection);
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < cmdParms.Length; i++)
                    dataAdapter.SelectCommand.Parameters.Add(cmdParms[i]);
                if (sTableName != "")
                {
                    dataAdapter.Fill(ds, sTableName);
                }
                else
                    dataAdapter.Fill(ds);
            }
            catch (OracleException loadException)
            {
                throw loadException;
            }
            catch (Exception unException)
            {
                throw unException;
            }
            finally
            {
                dbConnection.Close();
            }
            return ds;
        }




        public static OracleConnection getPoolingConnection()
        {
            //string conn = ConfigurationSettings.AppSettings["Connection"];
            string conn = CommonFunction.GetValuesAppSetting("webConfig", "Connection");
            OracleConnection dbConn = new OracleConnection(conn);
            return dbConn;
        }





    }

}