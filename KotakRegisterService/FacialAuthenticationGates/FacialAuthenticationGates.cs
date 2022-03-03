using Oracle.ManagedDataAccess.Client;
using KMBL.StepupAuthentication.CoreComponents.DataAccessHandler;
using System.Data;
using System.Collections.Generic;
using System;
using System.Configuration;

namespace KMBL.StepupAuthentication.CoreComponents
{

    public class get_registered_user_data
    {
        public int customer_registration_checker_id { get; set; } 
        public string CRN { get; set; }
        public string customer_image_id { get; set; }
        public int customer_image2_id { get; set; }
        public int customer_image3_id { get; set; }
        public string person_id { get; set; }
    }

    public class survey_question_and_answer
    {
        public int Responsecode { get; set; }
        public List<survey_question_data> questions { get; set; }
    }

    public class survey_question_data
    {
        public int question_id { get; set; }
        public string question_text { get; set; }
        public List<survey_answer_data> answer { get; set; } 
    }

    public class survey_answer_data
    {
        public int question_id { get; set; }
        public int answer_id { get; set; }
        public string answer_text { get; set; }
    }

    public class post_survey_user_answer
    {
        public string crn { get; set; }
        public int customer_registration_checker_id { get; set; }
        public string rate_experience { get; set; }
        public string suggestion_text { get; set; }
        public int question_id { get; set; }
        public int answer_id { get; set; }
    }

    public class ExecutingSP
    {

        public static string MongoDB_Registration_Databse = ConfigurationManager.AppSettings["MongoDB_Registration_Database"];
        //private static string MongoDB_Verification_Databse = System.Configuration.ConfigurationManager.AppSettings["MongoDB_Verification_Databse"];
       

        public static string MongoDBConnectionstring = ConfigurationManager.AppSettings["MongoDBConnectionstring"];
        public string SP_GET_PERSON_ID_DE_REGISTER(string CRN)
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_crn_value", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_DeviceType", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_GET_PERSON_ID_DE_REGISTER", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            connection.Open();
            OracleDataReader reader = objCmd.ExecuteReader();

            string PersonID = "";

            while (reader.Read())
            {
                PersonID = reader.GetString(0);
            }

            dbManager.CloseConnection(connection);

            return PersonID;
        }

        public void SP_DE_REGISTER_ON_END(string CRN, Int32 SessionID, int EventID, int ChannelID, int GateID, int StatusID, string DeviceDetails, string JsonResponse,string Version)
        {
            var dbManager = new DBManager("DBConnection");

            var parameters = new List<OracleParameter>();  
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_SessionID", SessionID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GateID", GateID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_EventID", EventID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_channel_id", ChannelID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_StatusID", StatusID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_DeviceDetails", DeviceDetails, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_JsonResponse", JsonResponse, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_Version", Version, OracleDbType.Varchar2));
            dbManager.Insert("SP_DE_REGISTER_ON_END", CommandType.StoredProcedure, parameters.ToArray());
        }

        public void SP_BENCHMARK(int PID,string CRN, int EventID, string Phase, string StartTime, string EndTime)
        {
            var dbManager = new DBManager("DBConnection");

            var parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_PID", PID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_EVENTID", EventID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_PHASE", Phase, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_STARTTIME", StartTime, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_ENDTIME", EndTime, OracleDbType.NVarchar2));

            dbManager.Insert("SP_BENCHMARK", CommandType.StoredProcedure, parameters.ToArray());

        }

        public List<string> SP_GateExecuteOnStart(string ObjectID, string TokenID, string SessionID, string CRN, int GateID, string EventID, string ChannelID, string DeviceDetails, Int32 PTID)
        {
            var start_list = new List<string>();

            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_ObjectID", ObjectID, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_TokenID", TokenID, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_SessionID", SessionID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_GateID", GateID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_EventID", EventID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ChannelID", ChannelID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_DeviceDetails", DeviceDetails, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_PTID_Input", PTID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GatePass", OracleDbType.RefCursor, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_ParentTransactionID", OracleDbType.Int32, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_CustomerImageID", OracleDbType.Int32, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_Gate_Execute_On_Start", CommandType.StoredProcedure, parameters.ToArray(), out connection);


            connection.Open();
            OracleDataReader reader = objCmd.ExecuteReader();

            while (reader.Read())
            {
                start_list.Add(reader.GetString(0));
            }

            start_list.Add(objCmd.Parameters["v_ParentTransactionID"].Value.ToString());
            start_list.Add(objCmd.Parameters["v_CustomerImageID"].Value.ToString());

            dbManager.CloseConnection(connection);
            return start_list;


        }


        public void SP_GateExecuteOnEnd(int ImageStatus, int CustomerID, string CRN, int SessionID, int GateID, string GatePass, string JsonResponse, int StatusID, int PTID, string EventID, string ChannelID, string ObjectID)
        {
            var dbManager = new DBManager("DBConnection");

            var parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_Status", ImageStatus, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_CustomerID", CustomerID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_SessionID", SessionID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GateID", GateID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GatePass", GatePass, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_JsonResponse", JsonResponse, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_StatusID", StatusID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ParentTransactionID", PTID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_EventID", EventID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ChannelID", ChannelID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_object_id", ObjectID, OracleDbType.NVarchar2));
            dbManager.Update("SP_Gate_Execute_On_End", CommandType.StoredProcedure, parameters.ToArray());
        }

        public int SP_GetStatusID(string Status)
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_StatusValue", Status, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_StatusID", OracleDbType.Int32, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_GET_STATUS_ID", CommandType.StoredProcedure, parameters.ToArray(), out connection);


            connection.Open();
            objCmd.ExecuteNonQuery();

            int StatusID = int.Parse(objCmd.Parameters["v_StatusID"].Value.ToString());

            dbManager.CloseConnection(connection);


            return StatusID;
        }

        public string SP_GetDeviceType(int ChannelID)
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_ChannelID", ChannelID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_DeviceType", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_GET_DEVICE_TYPE", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            connection.Open();
            OracleDataReader reader = objCmd.ExecuteReader();

            string DeviceType = "";

            while (reader.Read())
            {
                DeviceType = reader.GetString(0);
            }

            dbManager.CloseConnection(connection);

            return DeviceType;
        }

        public List<int> SP_GetChannelIDEventID(string ChannelName, string EventName)
        {
            var evntchnl_list = new List<int>();

            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_ChannelName", ChannelName, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_EventName", EventName, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_ChannelID", OracleDbType.Int32, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_EventID", OracleDbType.Int32, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_GET_CHANNEL_EVENT_ID", CommandType.StoredProcedure, parameters.ToArray(), out connection);


            connection.Open();
            objCmd.ExecuteNonQuery();

            evntchnl_list.Add(int.Parse(objCmd.Parameters["v_ChannelID"].Value.ToString()));
            evntchnl_list.Add(int.Parse(objCmd.Parameters["v_EventID"].Value.ToString()));

            dbManager.CloseConnection(connection);

            return evntchnl_list;

        }


        public string SP_GetKey()
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_key", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_GET_KEY_KEY_VAULT", CommandType.StoredProcedure, parameters.ToArray(), out connection);


            connection.Open();
            OracleDataReader reader = objCmd.ExecuteReader();

            string key = "";

            while (reader.Read())
            {
                key = reader.GetString(0);
            }


            dbManager.CloseConnection(connection);

            return key;
        }


        public List<int> SP_GetGate(int event_id, int channel_id)
        {
            var gate_list = new List<int>();

            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_EventID", event_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ChannelID", channel_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_Gate_No", OracleDbType.RefCursor, ParameterDirection.Output));

            OracleCommand objCmd = dbManager.GetCommand("SP_GET_GATE_GATE_CONFIG", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            connection.Open();

            OracleDataReader reader = objCmd.ExecuteReader();

            while (reader.Read())
            {
                gate_list.Add(reader.GetInt32(0));
            }

            dbManager.CloseConnection(connection);

            return gate_list;

        }

        public List<string> SP_AzureExecuteOnStart(string ObjectID, string TokenID, string SessionID, string CRN, int GateID, string EventID, string ChannelID, string DeviceDetails, int PTID,string Version)
        {
            var start_list = new List<string>();

            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_TokenID", TokenID, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_SessionID", SessionID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_EventID", EventID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ChannelID", ChannelID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GateID", GateID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ObjectID", ObjectID, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_DeviceDetails", DeviceDetails, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_ParentTransactionID", PTID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_Version", Version, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_gatepass_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_objectID_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_AZURE_CLOUD_ON_START", CommandType.StoredProcedure, parameters.ToArray(), out connection);


            connection.Open();
            OracleDataReader reader = objCmd.ExecuteReader();

            while (reader.Read())
            {
                start_list.Add(reader.GetString(0));
            }

            reader.NextResult();

            while (reader.Read())
            {
                start_list.Add(reader.GetString(0));
            }


            dbManager.CloseConnection(connection);
            return start_list;

        }

        public int SP_ValidateGatePass(string CRN, string SessionID, int GateID, string GatePass, string EventID, string ChannelID)
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_SessionID", SessionID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GateID", GateID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GatePass", GatePass, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_EventID", EventID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ChannelID", ChannelID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_Result_Output", OracleDbType.Int32, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_Gate_Execute", CommandType.StoredProcedure, parameters.ToArray(), out connection);


            connection.Open();
            objCmd.ExecuteNonQuery();

            int Status = int.Parse(objCmd.Parameters["v_Result_Output"].Value.ToString());

            dbManager.CloseConnection(connection);


            return Status;
        }

        public void SP_AzureOnEnd(string CRN, string SessionID, int GateID, string GatePass, string JsonResponse, int StatusID, int PTID, string PersonID, string RequestNo, string BranchID, string ChannelID, string Gate1Image, string Gate2Image, string Gate3Image, string AuthorisedBy, string CreatedBy)
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_SessionID", int.Parse(SessionID), OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GateID", GateID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GatePass", GatePass, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_JsonResponse", JsonResponse, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_StatusID", StatusID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ParentTransactionID", PTID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_Person_ID", PersonID, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_request_no", RequestNo, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_branch_id", BranchID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_channel_id", int.Parse(ChannelID), OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_customer_image_id", Gate1Image, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_customer_image2_id", Gate2Image, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_customer_image3_id", Gate3Image, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_authorized_by", AuthorisedBy, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_created_by", CreatedBy, OracleDbType.Int32));
            //parameters.Add(dbManager.CreateParameter("v_registration_timestamp", OracleDbType.NVarchar2, ParameterDirection.Output));
            dbManager.Update("SP_AZURE_CLOUD_ON_END", CommandType.StoredProcedure, parameters.ToArray());
            //OracleCommand objCmd = dbManager.GetCommand("SP_AZURE_CLOUD_ON_END", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            //connection.Open();
            //objCmd.ExecuteNonQuery();

            //string registration_timestamp = objCmd.Parameters["v_registration_timestamp"].Value.ToString();

           // dbManager.CloseConnection(connection);

            //return registration_timestamp;


        }

        public int SP_AzureVerifyOnEnd(string CRN, string SessionID, int GateID, string GatePass, string JsonResponse, int StatusID, int Score, int PTID, string PersonID, string RequestNo, string GestureID, string BranchID, string ChannelID, string ObjectID, string AppID, string CreatedBy)
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_SessionID", int.Parse(SessionID), OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GateID", GateID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_GatePass", GatePass, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_JsonResponse", JsonResponse, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_StatusID", StatusID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_Score", Score, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ParentTransactionID", PTID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_Person_ID", PersonID, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_request_no", RequestNo, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_GestureID", GestureID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_branch_id", BranchID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_channel_id", int.Parse(ChannelID), OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_ObjectID", ObjectID, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_ApplicationID", AppID, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_created_by", CreatedBy, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_status_result", OracleDbType.Int32, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_AZURE_VERIFICATION_ON_END", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            connection.Open();
            objCmd.ExecuteNonQuery();

            int Status = int.Parse(objCmd.Parameters["v_status_result"].Value.ToString());

            dbManager.CloseConnection(connection);

            return Status;

        }

        public void SP_Exception(string crn, int event_id, int channel_id, string device_details, string message, string helplink, string exception_source, string stack_trace, string target_site)
        {
            var dbManager = new DBManager("DBConnection");

            var parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_crn", crn, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_channel_id", channel_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_device_details", device_details, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_message", message, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_helplink", helplink, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_exception_source", exception_source, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_stack_trace", stack_trace, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_target_site", target_site, OracleDbType.NVarchar2));

            dbManager.Insert("SP_INSERT_EXCEPTION_LOGS", CommandType.StoredProcedure, parameters.ToArray());

        }

        public List<get_registered_user_data> GetRegisteredUserData(int offset_value, int record_limit, string resource_group_id)
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            //List<OracleParameter> parameters = new List<OracleParameter>();
            var parameters = new List<OracleParameter>();
            List<get_registered_user_data> grud = new List<get_registered_user_data>();
            parameters.Add(dbManager.CreateParameter("v_offset_value", offset_value, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("v_no_of_records", record_limit, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("v_resource_group_id", resource_group_id, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("result_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var reader = dbManager.GetDataReader("SP_GET_REGISTERED_USER_DATA", CommandType.StoredProcedure, parameters.ToArray(), out connection);

  
            //var ImageGallery = new ImageGallery(MongoDBConnectionstring, (event_id == 1) ? MongoDB_Registration_Databse : MongoDB_Verification_Databse);
            var ImageGallery = new ImageGallery(MongoDBConnectionstring, MongoDB_Registration_Databse);
            while (reader.Read())
            {
                get_registered_user_data grudi = new get_registered_user_data();

                grudi.customer_registration_checker_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                grudi.CRN = reader.IsDBNull(1) ? null : reader.GetString(1);
                grudi.customer_image_id = reader.IsDBNull(2) ? null : Convert.ToBase64String(ImageGallery.Get(reader.GetString(2)));
                grudi.customer_image2_id = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                grudi.customer_image3_id = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                grudi.person_id = reader.IsDBNull(5) ? null : reader.GetString(5);
                grud.Add(grudi);
               
            }
            reader.Close();
            dbManager.CloseConnection(connection);
            return grud;
        }

        public int update_resource_group_id(string resource_group_id, string new_person_id, int customer_registration_checker_id)
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();

            parameters.Add(dbManager.CreateParameter("v_resource_group_id", resource_group_id, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_new_person_id", new_person_id, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_customer_registration_checker_id", customer_registration_checker_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("result_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var reader = dbManager.GetDataReader("SP_UPDATE_RESOURCE_GROUP_ID", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            int status = 0;
            while (reader.Read())
            {
                status = reader.GetInt32(0);
            }
            reader.Close();
            dbManager.CloseConnection(connection);
            return status;

        }

        public survey_question_and_answer survey_question_and_answer()
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            //List<OracleParameter> parameters = new List<OracleParameter>();
            var parameters = new List<OracleParameter>();
            survey_question_and_answer sqaa = new survey_question_and_answer();
            List<survey_question_data> sqd = new List<survey_question_data>();
            parameters.Add(dbManager.CreateParameter("r_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var reader = dbManager.GetDataReader("GET_SURVEY_QUESTIONS", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            while (reader.Read())
            {

                survey_question_data sqdi = new survey_question_data();
                sqdi.question_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                sqdi.question_text = reader.IsDBNull(1) ? null : reader.GetString(1);
                sqd.Add(sqdi);

            }
            reader.Close();
            dbManager.CloseConnection(connection);

            List<survey_answer_data> ad = survey_answer_data();
            for (var i = 0; i < sqd.Count; i++)
            {
                var get_question_id = sqd[i].question_id;
                List<survey_answer_data> adii = new List<survey_answer_data>();
                for (var j=0; j< ad.Count; j++)
                {
                    if(get_question_id == ad[j].question_id)
                    {
                        adii.Add(ad[j]);
                    }
                }
                sqd[i].answer=adii;
            }
            sqaa.Responsecode = 200;
            sqaa.questions = sqd;

            return sqaa;
        }

        public List<survey_answer_data> survey_answer_data()
        {
            var dbManager = new DBManager("DBConnection");

            OracleConnection connection = null;
            //List<OracleParameter> parameters = new List<OracleParameter>();
            var parameters = new List<OracleParameter>();
            List<survey_answer_data> ad = new List<survey_answer_data>();
            parameters.Add(dbManager.CreateParameter("r_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var reader = dbManager.GetDataReader("GET_SURVEY_ANSWERS", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            while (reader.Read())
            {

                survey_answer_data adi = new survey_answer_data();
                adi.answer_id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                adi.question_id = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                adi.answer_text = reader.IsDBNull(2) ? null : reader.GetString(2);
                ad.Add(adi);

            }
            reader.Close();
            dbManager.CloseConnection(connection);
            
            return ad;
        }

    }

}
