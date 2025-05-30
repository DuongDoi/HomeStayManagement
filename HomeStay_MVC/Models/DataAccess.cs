﻿using System;
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
        
        //USER
        public static DataSet LOGIN(string user,string pass)
        {
            
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
        public static DataSet USERS_UPDATE_PIN(string user, string _save_code)
        {
            
            string str;
            str = "";
            str = "API_USES_PKG.USERS_UPDATE_SAVE_CODE";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_save_code", OracleDbType.Varchar2),

                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = _save_code;


            return getDataFromProcedure(str, "", parms);
        }

        public static DataSet USERS_UPDATE_AVATAR(string user, string avatar_name)
        {

            string str;
            str = "";
            str = "API_USES_PKG.USERS_UPDATE_AVATAR";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_avatar_path", OracleDbType.Varchar2),

                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = avatar_name;


            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_INSERT(string user, string pass,string phone, string email,string name,string role,string ht_id,string create_by_user)
        {
            
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
                                new OracleParameter("v_role", OracleDbType.Varchar2),
                                new OracleParameter("v_homestay_id", OracleDbType.Varchar2),
                                new OracleParameter("v_create_by", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = pass;
            parms[2].Value = phone;
            parms[3].Value = email;
            parms[4].Value = name;
            parms[5].Value = role;
            parms[6].Value = ht_id;
            parms[7].Value = create_by_user;



            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_RESET_PASSWORD(string user, string email, string save_code,string pass)
        {
            
            string str;
            str = "";
            str = "API_USES_PKG.USERS_UPDATE_PASS";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_email", OracleDbType.Varchar2),
                                new OracleParameter("v_save_code", OracleDbType.Varchar2),
                                new OracleParameter("v_pass", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = email;
            parms[2].Value = save_code;
            parms[3].Value = pass;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_UPDATE_INFOR(string user,string phone,string email,string name, string card_number, string type)
        {
            
            string str;
            str = "";
            str = "API_USES_PKG.USERS_UPDATE_INFOR";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_phone", OracleDbType.Varchar2),
                                new OracleParameter("v_email", OracleDbType.Varchar2),
                                new OracleParameter("v_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_card_number", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;
            parms[1].Value = phone;
            parms[2].Value = email;
            parms[3].Value = name;
            parms[4].Value = card_number;
            parms[5].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_GET_LIST(string user)
        {
            
            string str;
            str = "";
            str = "API_USES_PKG.USERS_GET_LIST_MANAGER";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet USERS_GET_LIST_EMPLOYEE(string user_id,string ht_id,string create_by)
        {

            string str;
            str = "";
            str = "API_USES_PKG.USERS_GET_LIST_EMPLOYEE";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_homestay_id", OracleDbType.Varchar2),
                                new OracleParameter("v_create_by", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = user_id;
            parms[1].Value = ht_id;
            parms[2].Value = create_by;

            return getDataFromProcedure(str, "", parms);
        }

        public static DataSet USERS_UPDATE_PASS(string users, string email, string save_Code, string newPass1)
        {
            
            string str;
            str = "";
            str = "API_USES_PKG.USERS_UPDATE_PASS";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_users", OracleDbType.Varchar2),
                                new OracleParameter("v_email", OracleDbType.Varchar2),
                                new OracleParameter("v_save_code", OracleDbType.Varchar2),
                                new OracleParameter("v_pass", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = users;
            parms[1].Value = email;
            parms[2].Value = save_Code;
            parms[3].Value = newPass1;


            return getDataFromProcedure(str, "", parms);
        }

        //HOMESTAY
        public static DataSet HOMESTAYS_INSERT(string name, string card, string address, string description, string manager,string phone,string user_id)
        {
            
            string str;
            str = "";
            str = "homestays_pkg.HOMESTAYS_INSERT";
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
        public static DataSet HOMESTAYS_UPDATE(string id,string name, string card, string address, string description, string manager, string phone,string avatar_path, string type)
        {
            
            string str;
            str = "";
            str = "HOMESTAYS_PKG.HOMESTAYS_UPDATE";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_id", OracleDbType.Varchar2),
                                new OracleParameter("v_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_card", OracleDbType.Varchar2),
                                new OracleParameter("v_address", OracleDbType.NVarchar2),
                                new OracleParameter("v_description", OracleDbType.NVarchar2),
                                new OracleParameter("v_manager", OracleDbType.NVarchar2),
                                new OracleParameter("v_phone", OracleDbType.Varchar2),

                                new OracleParameter("v_avatar_path", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = name;
            parms[2].Value = card;
            parms[3].Value = address;
            parms[4].Value = description;
            parms[5].Value = manager;
            parms[6].Value = phone;
            parms[7].Value = avatar_path ?? (object)DBNull.Value;

            parms[8].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet HOMESTAYS_GET_LIST(string id,string user_id,string type)
        {
            
            string str;
            str = "";
            str = "HOMESTAYS_PKG.homestays_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_id", OracleDbType.Varchar2),
                                new OracleParameter("v_user_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = user_id;
            parms[2].Value = type;

            return getDataFromProcedure(str, "", parms);
        }


        //CUSTOMER
        public static DataSet CUSTOMERS_INSERT(string customer_card_id, string customer_name, string customer_phone, string customer_address, string users_id,string user_name)
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
                                new OracleParameter("v_create_by", OracleDbType.NVarchar2),
                                
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = customer_card_id;
            parms[1].Value = customer_name;
            parms[2].Value = customer_phone;
            parms[3].Value = customer_address;
            parms[4].Value = users_id;
            parms[5].Value = user_name;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet CUSTOMERS_UPDATE(string id,string customer_card_id, string customer_name, string customer_phone, string customer_address,string users_id, string type)
        {
            
            string str;
            str = "";
            str = "CUSTOMERS_PKG.CUSTOMERS_UPDATE";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_customer_id", OracleDbType.Varchar2),
                                new OracleParameter("v_customer_card_id", OracleDbType.Varchar2),
                                new OracleParameter("v_customer_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_customer_phone", OracleDbType.Varchar2),
                                new OracleParameter("v_customer_address", OracleDbType.NVarchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.NVarchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = customer_card_id;
            parms[2].Value = customer_name;
            parms[3].Value = customer_phone;
            parms[4].Value = customer_address;
            parms[5].Value = users_id;
            parms[6].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet CUSTOMERS_GET_LIST(string id, string users_id, string type)
        {
            
            string str;
            str = "";
            str = "customers_pkg.customers_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_customer_id", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = users_id;
            parms[2].Value = type;

            return getDataFromProcedure(str, "", parms);
        }

        //ROOM
        public static DataSet ROOMS_GET_LIST(string ht_id, string room_id,string userID, string type)
        {
            
            string str;
            str = "";
            str = "rooms_pkg.rooms_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_homestays_id", OracleDbType.Varchar2),
                                new OracleParameter("v_room_id", OracleDbType.Varchar2),
                                new OracleParameter("v_user_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = ht_id;
            parms[1].Value =room_id;
            parms[2].Value = userID;
            parms[3].Value = type;

            return getDataFromProcedure(str, "", parms);
        }

        public static DataSet ROOMS_UPDATE(string id, string name, string price, string status,string avatar_path,  string type)
        {

            string str;
            str = "";
            str = "rooms_pkg.rooms_update";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_rooms_id", OracleDbType.Varchar2),
                                new OracleParameter("v_room_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_price", OracleDbType.Varchar2),
                                new OracleParameter("v_status", OracleDbType.Varchar2),
                                new OracleParameter("v_avatar_path", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = name;
            parms[2].Value = price;
            parms[3].Value = status;
            parms[4].Value = avatar_path;

            parms[5].Value = type;

            return getDataFromProcedure(str, "", parms);
        }

        public static DataSet ROOMS_INSERT(string ht_id, string name, string price, string status)
        {

            string str;
            str = "";
            str = "rooms_pkg.rooms_insert";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_homestays_id", OracleDbType.Varchar2),
                                new OracleParameter("v_room_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_price", OracleDbType.Varchar2),
                                new OracleParameter("v_status", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = ht_id;
            parms[1].Value = name;
            parms[2].Value = price;
            parms[3].Value = status;

            return getDataFromProcedure(str, "", parms);
        }




        //FOOD
        public static DataSet FOODS_GET_LIST(string id, string users_id, string type)
        {
            
            string str;
            str = "";
            str = "foods_pkg.foods_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_foods_id", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = users_id;
            parms[2].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet FOODS_UPDATE(string id, string name, string price,string f_type, string userID,string avata_path, string type)
        {

            string str;
            str = "";
            str = "foods_pkg.foods_update";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_foods_id", OracleDbType.Varchar2),
                                new OracleParameter("v_foods_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_price", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_avatar_path", OracleDbType.Varchar2),
                                new OracleParameter("v_type_food", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = name;
            parms[2].Value = price;
            parms[3].Value = userID;
            parms[4].Value = avata_path;
            parms[5].Value = f_type;
            parms[6].Value = type;

            return getDataFromProcedure(str, "", parms);
        }

        public static DataSet FOODS_INSERT(string name, string price,string f_type, string userID)
        {

            string str;
            str = "";
            str = "foods_pkg.foods_insert";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_foods_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_price", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type_food", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = name;
            parms[1].Value = price;
            parms[2].Value = userID;
            parms[3].Value = f_type;


            return getDataFromProcedure(str, "", parms);
        }



        //SERVICE
        public static DataSet SERVICES_GET_LIST(string service_id, string users_id, string type)
        {
            
            string str;
            str = "";
            str = "services_pkg.services_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_services_id", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = service_id;
            parms[1].Value = users_id;
            parms[2].Value = type;

            return getDataFromProcedure(str, "", parms);
        }


        public static DataSet SERVICES_UPDATE(string id, string name, string price, string userID,string avatar_path, string type)
        {

            string str;
            str = "";
            str = "services_pkg.services_update";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_services_id", OracleDbType.Varchar2),
                                new OracleParameter("v_services_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_price", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_avatar_path", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = name;
            parms[2].Value = price;
            parms[3].Value = userID;
            parms[4].Value = avatar_path;

            parms[5].Value = type;


            return getDataFromProcedure(str, "", parms);
        }

        public static DataSet SERVICES_INSERT(string name, string price, string userID)
        {

            string str;
            str = "";
            str = "services_pkg.services_insert";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_services_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_price", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = name;
            parms[1].Value = price;
            parms[2].Value = userID;

            return getDataFromProcedure(str, "", parms);
        }

        //BILL
        public static DataSet BILLS_GET_LIST(string bills_id, string homestay_id, string type) 
        {

            string str;
            str = "";
            str = "bills_pkg.bills_get_list";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_bills_id", OracleDbType.Varchar2),
                                new OracleParameter("v_homestays_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = bills_id;
            parms[1].Value = homestay_id;
            parms[2].Value = type;

            return getDataFromProcedure(str, "", parms);
        }
        public static DataSet BILLS_UPDATE(string id, string name, string price, string userID, string type)
        {

            string str;
            str = "";
            str = "services_pkg.services_update";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_services_id", OracleDbType.Varchar2),
                                new OracleParameter("v_services_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_price", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("v_type", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = id;
            parms[1].Value = name;
            parms[2].Value = price;
            parms[3].Value = userID;
            parms[4].Value = type;

            return getDataFromProcedure(str, "", parms);
        }

        public static DataSet BILLS_INSERT(string name, string price, string userID)
        {

            string str;
            str = "";
            str = "services_pkg.services_insert";
            OracleParameter[] parms;
            parms = new OracleParameter[]
                            {
                                new OracleParameter("v_services_name", OracleDbType.NVarchar2),
                                new OracleParameter("v_price", OracleDbType.Varchar2),
                                new OracleParameter("v_users_id", OracleDbType.Varchar2),
                                new OracleParameter("P_RESULT",OracleDbType.RefCursor,ParameterDirection.Output),
            };
            parms[0].Value = name;
            parms[1].Value = price;
            parms[2].Value = userID;

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