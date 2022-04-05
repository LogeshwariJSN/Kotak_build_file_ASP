using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using KMBL.StepupAuthentication.CoreComponents.DataAccessHandler;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using KMBLNetNbanking;
using Newtonsoft.Json.Linq;

namespace KotakAdmin_Iteration_1
{
    public class DashboardSPecificResultData
    {
        public string CRN { get; set; }
        public string ResulstReason { get; set; }
        public string CreatedOn { get; set; }
        public string DeviceDetails { get; set; }
        public string ObjectId { get; set; }
        public string Version { get; set; }
        public string ThumbnailImage { get; set; }
    }

    public class DashboardSPecificResultTotalData
    {
        public string total_records { get; set; }
        public List<DashboardSPecificResultData> DashboardSPecificResultData { get; set; }
    }

    public class DashboardSuccessImageTotalData
    {
        public string total_records { get; set; }
        public List<DashboardSuccessImageData> DashboardSuccessImageData { get; set; }

    }

    public class DashboardSuccessImageData
    {
        public string CRN { get; set; }
        public string ResulstReason { get; set; }
        public string CreatedOn { get; set; }
        public string DeviceDetails { get; set; }
        public int AuditId { get; set; }
        public int ParentTransaction { get; set; }
        public string Version { get; set; }
        public List<string> ThumbnailImage { get; set; }
    }

    public class DashboardThumbnailImageData
    {
        public string ObjectId { get; set; }
        public int AuditId { get; set; }
        public int ParentTrasactionId { get; set; }

    }

    public class DataMigrationData
    {
        public string CRN { get; set; }
        public int customer_registration_checker_id { get; set; }
        public string created_on { get; set; }
    }

    public class GetCustomerImageData
    {
        public int customer_registration_checker_id { get; set; }
        public string CRN { get; set; }
        public string customer_image_id { get; set; }
        public int customer_image2_id { get; set; }
        public int customer_image3_id { get; set; }
    }

    public class DataAccessGallery
    {
        public string total_records = "";
        public string LoginValid = "", total_pending = "", total_approved = "", total_rejected = "", mobile_count = "", netbanking_count = "", branch_count = "", mobile_graph_count = "", netbanking_graph_count = "", branch_graph_count = "", total_count = "", total_records_faceauth = "";
        public dynamic GestureImage;
        private static string MongoDB_Registration_Databse = System.Configuration.ConfigurationManager.AppSettings["MongoDB_Registration_Databse"];
        private static string MongoDB_Verification_Databse = System.Configuration.ConfigurationManager.AppSettings["MongoDB_Verification_Databse"];
        private static string MongoDBConnectionstring = System.Configuration.ConfigurationManager.AppSettings["MongoDBConnectionstring"];
        public string PasswordKey = "";


        public string SP_GET_PERSON_ID(string CRN)
        {
            var dbManager = new DBManager("DBConnectionweb");

            OracleConnection connection = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_crn_value", CRN, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_DeviceType", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_GET_PERSON_ID_DE_REGISTER", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            connection.Open();
            OracleDataReader reader = objCmd.ExecuteReader();

            string PersonID = null;

            while (reader.Read())
            {
                PersonID = reader.GetString(0);
            }

            dbManager.CloseConnection(connection);

            return PersonID;
        }



        // Admin Dashboard user count Function
        public List<string> DashboardCountV1(string f, string t)
        {

            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            if (f == null || f == "")
            {
                parameters.Add(dbManager.CreateParameter("v_from_date", null, OracleDbType.Varchar2));
                f = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
                parameters.Add(dbManager.CreateParameter("v_from_date", f + " 00:00:00", OracleDbType.Varchar2));

            if (t == null || t == "")
                t = DateTime.Now.ToString("yyyy-MM-dd");

            parameters.Add(dbManager.CreateParameter("v_to_date", t + " 23:59:59", OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("total_reg", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("mobile_reg_Android", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("mobile_reg_IOS", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("netbanking_reg", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("branch_reg", OracleDbType.Int64, ParameterDirection.Output));

            parameters.Add(dbManager.CreateParameter("total_ver", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("mobile_ver_Android", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("mobile_ver_IOS", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("netbanking_ver", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("branch_ver", OracleDbType.Int64, ParameterDirection.Output));

            parameters.Add(dbManager.CreateParameter("latest_update_hour", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("latest_update_minute", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("from_date", OracleDbType.Date, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_DASHBOARD_COUNT_V1", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            dataReader.Close();
            dbManager.CloseConnection(connection);

            List<string> dcv1 = new List<string>();
            dcv1.Add(objCmd.Parameters["total_reg"].Value.ToString());
            dcv1.Add(objCmd.Parameters["mobile_reg_Android"].Value.ToString());
            dcv1.Add(objCmd.Parameters["mobile_reg_IOS"].Value.ToString());
            dcv1.Add(objCmd.Parameters["netbanking_reg"].Value.ToString());
            dcv1.Add(objCmd.Parameters["branch_reg"].Value.ToString());

            dcv1.Add(objCmd.Parameters["total_ver"].Value.ToString());
            dcv1.Add(objCmd.Parameters["mobile_ver_Android"].Value.ToString());
            dcv1.Add(objCmd.Parameters["mobile_ver_IOS"].Value.ToString());
            dcv1.Add(objCmd.Parameters["netbanking_ver"].Value.ToString());
            dcv1.Add(objCmd.Parameters["branch_ver"].Value.ToString());
            //dcv1.Add((objCmd.Parameters["latest_update_hour"].Value.ToString() == "0") ? objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minutes Ago": objCmd.Parameters["latest_update_hour"].Value.ToString()+" Hours "+objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minutes Ago");
            if (objCmd.Parameters["latest_update_hour"].Value.ToString() == null)
                dcv1.Add("No data found");
            else
                dcv1.Add((objCmd.Parameters["latest_update_hour"].Value.ToString() == "0") ? (objCmd.Parameters["latest_update_minute"].Value.ToString() == "0" || objCmd.Parameters["latest_update_minute"].Value.ToString() == "1") ? objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minute Ago" : objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minutes Ago" :
                ((objCmd.Parameters["latest_update_hour"].Value.ToString() == "1") ? objCmd.Parameters["latest_update_hour"].Value.ToString() + " Hour " : objCmd.Parameters["latest_update_hour"].Value.ToString() + " Hours ") + ((objCmd.Parameters["latest_update_minute"].Value.ToString() == "1" || objCmd.Parameters["latest_update_minute"].Value.ToString() == "0") ? objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minute Ago" : objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minutes Ago"));
            dcv1.Add(objCmd.Parameters["from_date"].Value.ToString());
            return dcv1;
        }



        // Admin Dashboard user count Function
        public List<string> DashboardFailureCount(string f, string t)
        {
            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            if (f == null || f == "")
            {
                parameters.Add(dbManager.CreateParameter("v_from_date", null, OracleDbType.Varchar2));
                f = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
                parameters.Add(dbManager.CreateParameter("v_from_date", f + " 00:00:00", OracleDbType.Varchar2));

            if (t == null || t == "")
                t = DateTime.Now.ToString("yyyy-MM-dd");

            parameters.Add(dbManager.CreateParameter("v_to_date", t + " 23:59:59", OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("total_reg", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("mobile_reg_Android", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("mobile_reg_IOS", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("netbanking_reg", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("branch_reg", OracleDbType.Int64, ParameterDirection.Output));

            parameters.Add(dbManager.CreateParameter("total_ver", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("mobile_ver_Android", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("mobile_ver_IOS", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("netbanking_ver", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("branch_ver", OracleDbType.Int64, ParameterDirection.Output));

            parameters.Add(dbManager.CreateParameter("latest_update_hour", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("latest_update_minute", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("from_date", OracleDbType.Date, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_DASHBOARD_FAILURE_COUNT", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            dataReader.Close();
            dbManager.CloseConnection(connection);

            List<string> dcv1 = new List<string>();
            dcv1.Add(objCmd.Parameters["total_reg"].Value.ToString());
            dcv1.Add(objCmd.Parameters["mobile_reg_Android"].Value.ToString());
            dcv1.Add(objCmd.Parameters["mobile_reg_IOS"].Value.ToString());
            dcv1.Add(objCmd.Parameters["netbanking_reg"].Value.ToString());
            dcv1.Add(objCmd.Parameters["branch_reg"].Value.ToString());

            dcv1.Add(objCmd.Parameters["total_ver"].Value.ToString());
            dcv1.Add(objCmd.Parameters["mobile_ver_Android"].Value.ToString());
            dcv1.Add(objCmd.Parameters["mobile_ver_IOS"].Value.ToString());
            dcv1.Add(objCmd.Parameters["netbanking_ver"].Value.ToString());
            dcv1.Add(objCmd.Parameters["branch_ver"].Value.ToString());
            //dcv1.Add((objCmd.Parameters["latest_update_hour"].Value.ToString() == "0") ? objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minutes Ago": objCmd.Parameters["latest_update_hour"].Value.ToString()+" Hours "+objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minutes Ago");
            if (objCmd.Parameters["latest_update_hour"].Value.ToString() == null)
                dcv1.Add("No data found");
            else
                dcv1.Add((objCmd.Parameters["latest_update_hour"].Value.ToString() == "0") ? (objCmd.Parameters["latest_update_minute"].Value.ToString() == "0" || objCmd.Parameters["latest_update_minute"].Value.ToString() == "1") ? objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minute Ago" : objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minutes Ago" :
                ((objCmd.Parameters["latest_update_hour"].Value.ToString() == "1") ? objCmd.Parameters["latest_update_hour"].Value.ToString() + " Hour " : objCmd.Parameters["latest_update_hour"].Value.ToString() + " Hours ") + ((objCmd.Parameters["latest_update_minute"].Value.ToString() == "1" || objCmd.Parameters["latest_update_minute"].Value.ToString() == "0") ? objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minute Ago" : objCmd.Parameters["latest_update_minute"].Value.ToString() + " Minutes Ago"));
            dcv1.Add(objCmd.Parameters["from_date"].Value.ToString());
            return dcv1;
        }


        // Admin Dashboard user count Function
        public List<string> DashboardGateDropOffsFailure(string f, string t)
        {
            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            if (f == null || f == "")
            {
                parameters.Add(dbManager.CreateParameter("v_from_date", null, OracleDbType.Varchar2));
                f = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
                parameters.Add(dbManager.CreateParameter("v_from_date", f + " 00:00:00", OracleDbType.Varchar2));

            if (t == null || t == "")
                t = DateTime.Now.ToString("yyyy-MM-dd");

            parameters.Add(dbManager.CreateParameter("v_to_date", t + " 23:59:59", OracleDbType.Varchar2));

            parameters.Add(dbManager.CreateParameter("v_registration_drop_offs", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_verification_drop_offs", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_gate1_failures", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_gate2_failures", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_gate3_failures", OracleDbType.Int64, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_DASHBOARD_GATE_FAILURE_DROP_OFFS", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            dataReader.Close();
            dbManager.CloseConnection(connection);
            List<string> dcv1 = new List<string>();
            dcv1.Add(objCmd.Parameters["v_registration_drop_offs"].Value.ToString());
            dcv1.Add(objCmd.Parameters["v_verification_drop_offs"].Value.ToString());
            dcv1.Add(objCmd.Parameters["v_gate1_failures"].Value.ToString());
            dcv1.Add(objCmd.Parameters["v_gate2_failures"].Value.ToString());
            dcv1.Add(objCmd.Parameters["v_gate3_failures"].Value.ToString());
            return dcv1;
        }

        //Dashboard Failure Report
        public static DashboardSPecificResultTotalData DashboardSPecificResult(int event_id, int resultsid, int channelid, string f, string t, int offset_value, int record_limit)
        {

            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            if (f == null || f == "")
            {
                parameters.Add(dbManager.CreateParameter("v_from_date", null, OracleDbType.Varchar2));
                f = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
                parameters.Add(dbManager.CreateParameter("v_from_date", f + " 00:00:00", OracleDbType.Varchar2));

            if (t == null || t == "")
                t = DateTime.Now.ToString("yyyy-MM-dd");

            parameters.Add(dbManager.CreateParameter("v_to_date", t + " 23:59:59", OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("v_resultsid", resultsid, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("v_channelid", channelid, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("v_offset_value", offset_value, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("v_record_limit", record_limit, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("r_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("v_total_records", OracleDbType.Int32, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_GET_DASHBOARD_SPECIFIC_RESULT", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            List<DashboardSPecificResultData> dsrd = new List<DashboardSPecificResultData>();
            var ImageGallery = new ImageGallery(MongoDBConnectionstring, (event_id == 1) ? MongoDB_Registration_Databse : MongoDB_Verification_Databse);
            while (dataReader.Read())
            {
                dsrd.Add(new DashboardSPecificResultData
                {
                    CRN = dataReader.IsDBNull(0) ? null : dataReader.GetString(0),
                    //CreatedOn = dataReader.IsDBNull(1) ?  new DateTime() : dataReader.GetDateTime(1),
                    CreatedOn = dataReader.IsDBNull(1) ? null : dataReader.GetString(1),
                    ResulstReason = dataReader.IsDBNull(2) ? null : dataReader.GetString(2),
                    DeviceDetails = dataReader.IsDBNull(3) ? null : dataReader.GetString(3),
                    ObjectId = dataReader.IsDBNull(4) ? null : dataReader.GetString(4),
                    ThumbnailImage = dataReader.IsDBNull(4) ? null : Convert.ToBase64String(ImageGallery.Get(dataReader.GetString(4))),
                    Version = dataReader.IsDBNull(5) ? "1" : dataReader.GetString(5)
                });
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            //RestOfRecords = objCmd.Parameters["v_total_records"].Value.ToString();

            DashboardSPecificResultTotalData DashboardSPecificResultTotalData = new DashboardSPecificResultTotalData();

            DashboardSPecificResultTotalData.total_records = objCmd.Parameters["v_total_records"].Value.ToString();
            DashboardSPecificResultTotalData.DashboardSPecificResultData = dsrd;

            return DashboardSPecificResultTotalData;
        }

        //Dashboard Success Report
        public static DashboardSuccessImageTotalData DashboardSuccessImage(int event_id, int resultsid, int channelid, string f, string t, int offset_value, int record_limit)
        {
            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            var parameters1 = new List<OracleParameter>();
            if (f == null || f == "")
            {
                parameters.Add(dbManager.CreateParameter("v_from_date", null, OracleDbType.Varchar2));
                parameters1.Add(dbManager.CreateParameter("v_from_date", null, OracleDbType.Varchar2));
                f = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                parameters.Add(dbManager.CreateParameter("v_from_date", f + " 00:00:00", OracleDbType.Varchar2));
                parameters1.Add(dbManager.CreateParameter("v_from_date", f + " 00:00:00", OracleDbType.Varchar2));
            }

            if (t == null || t == "")
                t = DateTime.Now.ToString("yyyy-MM-dd");

            parameters.Add(dbManager.CreateParameter("v_to_date", t + " 23:59:59", OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("v_resultsid", resultsid, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("v_channelid", channelid, OracleDbType.Int64));
            //parameters.Add(dbManager.CreateParameter("v_offset_value", offset_value, OracleDbType.Int64));
            //parameters.Add(dbManager.CreateParameter("v_record_limit", record_limit, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("r_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_GET_DASHBOARD_SUCCESS_IMAGE", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            List<DashboardThumbnailImageData> imageData = new List<DashboardThumbnailImageData>();
            var ImageGallery = new ImageGallery(MongoDBConnectionstring, (event_id == 1) ? MongoDB_Registration_Databse : MongoDB_Verification_Databse);
            while (dataReader.Read())
            {
                imageData.Add(new DashboardThumbnailImageData
                {
                    AuditId = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0),
                    ObjectId = dataReader.IsDBNull(1) ? null : dataReader.GetString(1),
                    ParentTrasactionId = dataReader.IsDBNull(2) ? 0 : dataReader.GetInt32(2),
                });
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);

            parameters1.Add(dbManager.CreateParameter("v_to_date", t + " 23:59:59", OracleDbType.Varchar2));
            parameters1.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int64));
            parameters1.Add(dbManager.CreateParameter("v_resultsid", resultsid, OracleDbType.Int64));
            parameters1.Add(dbManager.CreateParameter("v_channelid", channelid, OracleDbType.Int64));
            parameters1.Add(dbManager.CreateParameter("v_offset_value", offset_value, OracleDbType.Int64));
            parameters1.Add(dbManager.CreateParameter("v_record_limit", record_limit, OracleDbType.Int64));
            parameters1.Add(dbManager.CreateParameter("r_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            parameters1.Add(dbManager.CreateParameter("v_total_records", OracleDbType.Int32, ParameterDirection.Output));

            objCmd = dbManager.GetCommand("SP_GET_DASHBOARD_SPECIFIC_RESULT_SUCCESS", CommandType.StoredProcedure, parameters1.ToArray(), out connection);
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            List<DashboardSuccessImageData> specificsuccessresult = new List<DashboardSuccessImageData>();
            while (dataReader.Read())
            {
                specificsuccessresult.Add(new DashboardSuccessImageData
                {
                    CRN = dataReader.IsDBNull(0) ? null : dataReader.GetString(0),
                    //CreatedOn = dataReader.IsDBNull(1) ? new DateTime() : dataReader.GetDateTime(1),
                    CreatedOn = dataReader.IsDBNull(1) ? null : dataReader.GetString(1),
                    ResulstReason = dataReader.IsDBNull(2) ? null : dataReader.GetString(2),
                    DeviceDetails = dataReader.IsDBNull(3) ? null : dataReader.GetString(3),
                    AuditId = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4),
                    ParentTransaction = dataReader.IsDBNull(5) ? 0 : dataReader.GetInt32(5),
                    Version = dataReader.IsDBNull(6) ? "1" : dataReader.GetString(6),
                    ThumbnailImage = new List<string>()
                });
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);


            foreach (DashboardThumbnailImageData thumbnailId in imageData)
            {
                foreach (DashboardSuccessImageData successId in specificsuccessresult)
                {
                    try
                    {
                        if (thumbnailId.ParentTrasactionId == successId.ParentTransaction)
                        {
                            successId.ThumbnailImage.Add(Convert.ToBase64String(ImageGallery.Get(thumbnailId.ObjectId)));

                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            DashboardSuccessImageTotalData DashboardSuccessImageTotalData = new DashboardSuccessImageTotalData();

            DashboardSuccessImageTotalData.total_records = objCmd.Parameters["v_total_records"].Value.ToString();
            DashboardSuccessImageTotalData.DashboardSuccessImageData = specificsuccessresult;
            return DashboardSuccessImageTotalData;
        }

        public void SP_ClearCRN(string CRN)
        {
            var dbManager = new DBManager("DBConnectionweb");

            var parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_CRN", CRN, OracleDbType.NVarchar2));

            dbManager.Delete("SP_CLEAR_CRN", CommandType.StoredProcedure, parameters.ToArray());
        }




        // Master Gate Function
        public List<KMBLNetNbanking.MasterGateFetch> MasterGateFetchlog()
        {
            var dbManager = new DBManager("DBConnectionweb");

            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            List<KMBLNetNbanking.MasterGateFetch> mgf = new List<KMBLNetNbanking.MasterGateFetch>();
            parameters.Add(dbManager.CreateParameter("gate_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var dataReader = dbManager.GetDataReader("SP_GET_GATE_MASTER_DETAILS", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            while (dataReader.Read())
            {
                mgf.Add(new KMBLNetNbanking.MasterGateFetch
                {
                    GATE_NUMBER = dataReader.GetInt32(0)
                });
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return mgf;
        }

        // Master Result Reason Function
        public List<KMBLNetNbanking.MasterResultReasonFetch> MasterResultReasonFetchLog()
        {
            var dbManager = new DBManager("DBConnectionweb");

            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            List<KMBLNetNbanking.MasterResultReasonFetch> mrrf = new List<KMBLNetNbanking.MasterResultReasonFetch>();
            parameters.Add(dbManager.CreateParameter("result_reason_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var dataReader = dbManager.GetDataReader("SP_GET_RESULT_REASON_MASTER_DETAILS", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            while (dataReader.Read())
            {
                mrrf.Add(new KMBLNetNbanking.MasterResultReasonFetch
                {
                    Result_Reason = dataReader.GetString(0)
                });
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return mrrf;
        }

        public List<KMBLNetNbanking.MasterStatusFetch> MasterStatusFetchlog()
        {

            var dbManager = new DBManager("DBConnectionweb");

            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            List<KMBLNetNbanking.MasterStatusFetch> msd = new List<KMBLNetNbanking.MasterStatusFetch>();
            parameters.Add(dbManager.CreateParameter("status_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var dataReader = dbManager.GetDataReader("SP_GET_STATUS_MASTER_DETAILS", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            while (dataReader.Read())
            {
                msd.Add(new KMBLNetNbanking.MasterStatusFetch
                {
                    status_id = dataReader.GetInt32(0),
                    status_name = dataReader.GetString(1)
                });
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return msd;
        }


        public List<string> GetVersions()
        {
            var dbManager = new DBManager("DBConnectionweb");
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            List<string> med = new List<string>();
            parameters.Add(dbManager.CreateParameter("version_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var dataReader = dbManager.GetDataReader("SP_GET_VERSIONS", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            while (dataReader.Read())
            {
                med.Add(dataReader.GetString(0));
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return med;
        }


        // Log Function
        public static JsonResult AuditLogFetch(string crn, string event_id, string result_reason, string gate_number, string from_date, string to_date, string version, int offset_value, int no_of_records)
        {
            string total_records = "0";
            List<KMBLNetNbanking.AuditLogFetch> ad = new List<KMBLNetNbanking.AuditLogFetch>();
            if (from_date == null && to_date == null || from_date == "" && to_date == "")
            {
                from_date = DateTime.Now.ToString("yyyy-MM-dd");

                to_date = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (crn == "")
            {
                crn = null;
            }
            if (version == "")
            {
                version = null;
            }
            if (result_reason == "")
            {
                result_reason = null;
            }
            if (event_id == "0")
            {
                event_id = null;
            }
            if (gate_number == "0")
            {
                gate_number = null;
            }
            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader reader = null;
            OracleConnection connection = null;

            var parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_offset", offset_value, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_no_of_records", no_of_records, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_crn", crn, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_gate_number", gate_number, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_from_date", from_date + " 00:00:00", OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_to_date", to_date + " 23:59:59", OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_version", version, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_results_reason", result_reason, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("l_cursor", OracleDbType.RefCursor, ParameterDirection.Output)); 
            parameters.Add(dbManager.CreateParameter("v_total_records", OracleDbType.Int32, ParameterDirection.Output));

            OracleCommand objCmd = dbManager.GetCommand("SP_GET_AUDIT_LOG_DETAILS", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            connection.Open();
            reader = objCmd.ExecuteReader();
            while (reader.Read())
            {
                KMBLNetNbanking.AuditLogFetch adi = new KMBLNetNbanking.AuditLogFetch();
                adi.AuditID = reader.GetInt32(0);
                adi.CRN = reader.GetString(1);
                //adi.CreatedOn = reader.GetDateTime(2);
                adi.CreatedOn = reader.IsDBNull(2) ? null : reader.GetString(2);
                adi.EventName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                adi.EventID = reader.GetInt32(4);
                adi.SessionID = reader.GetInt32(5);
                adi.StatusID = reader.GetInt32(6);
                adi.StatusName = reader.IsDBNull(7) ? "" : reader.GetString(7);
                adi.GateFailedAt = reader.IsDBNull(14) ? "" : reader.GetString(14);
                adi.ParentTransactionID = reader.GetInt32(9);
                adi.ChannelName = reader.IsDBNull(10) ? "" : reader.GetString(10);
                adi.Score = reader.IsDBNull(12) ? 0 : reader.GetInt32(12);
                adi.IsCompleted = reader.GetInt32(13);
                adi.Version = reader.IsDBNull(15) ? "1" : reader.GetString(15);
                adi.json_response = reader.IsDBNull(16) ? "" : reader.GetString(16);
                adi.ResultReason = reader.IsDBNull(17) ? "" : reader.GetString(17);
                adi.AuditLogExpandFetch = AuditLogExpandFetch(reader.GetInt32(0), reader.GetString(3));
                ad.Add(adi);

            }
            reader.Close();
            dbManager.CloseConnection(connection);
            total_records = Convert.ToString(objCmd.Parameters["v_total_records"].Value);
            return new JsonResult()
            {
                Data = new { StatusCode = 200, Data = ad, total_records = total_records },
                MaxJsonLength = 86753090
            };
        }

        // MasterEvents Function
        public List<KMBLNetNbanking.MasterEventsFetch> MasterEventsFetchlog()
        {
            var dbManager = new DBManager("DBConnectionweb");

            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            List<KMBLNetNbanking.MasterEventsFetch> med = new List<KMBLNetNbanking.MasterEventsFetch>();
            parameters.Add(dbManager.CreateParameter("event_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var dataReader = dbManager.GetDataReader("SP_GET_EVENTS_MASTER_DETAILS", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            while (dataReader.Read())
            {
                med.Add(new KMBLNetNbanking.MasterEventsFetch
                {
                    EVENT_ID = dataReader.GetInt32(0),
                    EVENT_NAME = dataReader.GetString(1)
                });
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return med;
        }


        public static List<KMBLNetNbanking.AuditLogExpandFetch> AuditLogExpandFetch(int audit_id, string event_name)
        {

            var dbManager = new DBManager("DBConnectionweb");
            ImageGallery img;
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            List<KMBLNetNbanking.AuditLogExpandFetch> aed = new List<KMBLNetNbanking.AuditLogExpandFetch>();
            parameters.Add(dbManager.CreateParameter("v_parent_transaction_id", audit_id, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("g_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var reader = dbManager.GetDataReader("SP_GET_EVENT_DETAILS", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            while (reader.Read())
            {

                if (event_name == "Registration")
                {
                    img = new ImageGallery(MongoDBConnectionstring, MongoDB_Registration_Databse);
                }
                else
                {
                    img = new ImageGallery(MongoDBConnectionstring, MongoDB_Verification_Databse);
                }

                aed.Add(new KMBLNetNbanking.AuditLogExpandFetch
                {
                    AUDIT_ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    Image_get = reader.IsDBNull(1) ? "../../Audit_assets/images/empty_person.jpg" : "data:image/jpeg;base64," + Convert.ToBase64String(img.Get(reader.GetString(1))),
                    GATE_NAME = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    JSON_RESPONSE = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    threshold_value = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    accuracy_value = reader.IsDBNull(7) ? "" : reader.GetString(7),
                });
            }
            reader.Close();
            dbManager.CloseConnection(connection);
            return aed;


        }

        //Audit Log Register Count
        public AuditLogRegisterCount AuditLogRegisterCount(string crn, int event_id, string result_reason, int gate_number, string from_date, string to_date, string version)
        {

            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;


            var parameters = new List<OracleParameter>();

            //Filter values
            parameters.Add(dbManager.CreateParameter("v_crn", crn, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_result_reason", result_reason, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_gate_number", gate_number, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_from_date", from_date, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_to_date", to_date, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_version", version, OracleDbType.Varchar2));

            //Registration Completed
            parameters.Add(dbManager.CreateParameter("reg_complete_single_attempt", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("reg_complete_multiple_attempt", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("reg_average_completed_users", OracleDbType.Decimal, ParameterDirection.Output));

            //Registration Dropped
            parameters.Add(dbManager.CreateParameter("reg_drop_single_attempt", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("reg_drop_multiple_attempt", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("reg_average_dropped_users", OracleDbType.Decimal, ParameterDirection.Output));

            //Registration attempts
            parameters.Add(dbManager.CreateParameter("reg_successful_attempts", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("reg_failed_attempts", OracleDbType.Int64, ParameterDirection.Output));

            OracleCommand objCmd = dbManager.GetCommand("SP_AUDIT_LOG_REGISTER_COUNT", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            //connection.Open();
            //objCmd.ExecuteReader();
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            dataReader.Close();
            dbManager.CloseConnection(connection);
            AuditLogRegisterCount alr1 = new AuditLogRegisterCount();
            alr1.reg_complete_single_attempt = Convert.IsDBNull(objCmd.Parameters["reg_complete_single_attempt"].Value) ? 0 : Convert.ToInt32(objCmd.Parameters["reg_complete_single_attempt"].Value.ToString());
            alr1.reg_complete_multiple_attempt = Convert.IsDBNull(objCmd.Parameters["reg_complete_multiple_attempt"].Value) ? 0 : Convert.ToInt32(objCmd.Parameters["reg_complete_multiple_attempt"].Value.ToString());
            alr1.reg_average_completed_users = Convert.IsDBNull(objCmd.Parameters["reg_average_completed_users"].Value) ? 0 : Convert.ToInt32(objCmd.Parameters["reg_average_completed_users"].Value.ToString());
            alr1.reg_drop_single_attempt = Convert.IsDBNull(objCmd.Parameters["reg_drop_single_attempt"].Value) ? 0 : Convert.ToInt32(objCmd.Parameters["reg_drop_single_attempt"].Value.ToString());
            alr1.reg_drop_multiple_attempt = Convert.IsDBNull(objCmd.Parameters["reg_drop_multiple_attempt"].Value) ? 0 : Convert.ToInt32(objCmd.Parameters["reg_drop_multiple_attempt"].Value.ToString());
            alr1.reg_average_dropped_users = Convert.IsDBNull(objCmd.Parameters["reg_average_dropped_users"].Value) ? "0" : objCmd.Parameters["reg_average_dropped_users"].Value.ToString();
            alr1.reg_successful_attempts = Convert.IsDBNull(objCmd.Parameters["reg_successful_attempts"].Value) ? 0 : Convert.ToInt32(objCmd.Parameters["reg_successful_attempts"].Value.ToString());
            alr1.reg_failed_attempts = Convert.IsDBNull(objCmd.Parameters["reg_failed_attempts"].Value) ? 0 : Convert.ToInt32(objCmd.Parameters["reg_failed_attempts"].Value.ToString());
            //dbManager.CloseConnection(connection);
            return alr1;
        }

        //Audit Log Verify Count
        public AuditLogVerifyCount AuditLogVerifyCount(string crn, int event_id, string result_reason, int gate_number, string from_date, string to_date, string version)
        {

            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;
            //parameters.Add(dbManager.CreateParameter("v_to_date", to_date + " 23:59:59", OracleDbType.Varchar2));

            var parameters = new List<OracleParameter>();

            //Filter values
            parameters.Add(dbManager.CreateParameter("v_crn", crn, OracleDbType.NVarchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_result_reason", result_reason, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_gate_number", gate_number, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_from_date", from_date, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_to_date", to_date, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_version", version, OracleDbType.Varchar2));

            //Registration Completed
            parameters.Add(dbManager.CreateParameter("verify_complete_single_attempt", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("verify_complete_multiple_attempt", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("verify_average_completed_users", OracleDbType.Decimal, ParameterDirection.Output));

            //Registration Dropped
            parameters.Add(dbManager.CreateParameter("verify_drop_single_attempt", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("verify_drop_multiple_attempt", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("verify_average_dropped_users", OracleDbType.Decimal, ParameterDirection.Output));

            //Registration attempts
            parameters.Add(dbManager.CreateParameter("verify_successful_attempts", OracleDbType.Int64, ParameterDirection.Output));
            parameters.Add(dbManager.CreateParameter("verify_failed_attempts", OracleDbType.Int64, ParameterDirection.Output));

            OracleCommand objCmd = dbManager.GetCommand("SP_AUDIT_LOG_VERIFY_COUNT", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            //connection.Open();
            //objCmd.ExecuteReader();
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            dataReader.Close();
            dbManager.CloseConnection(connection);
            AuditLogVerifyCount alv1 = new AuditLogVerifyCount();
            alv1.verify_complete_single_attempt = Convert.IsDBNull(objCmd.Parameters["verify_complete_single_attempt"].Value.ToString()) ? 0 : Convert.ToInt32(objCmd.Parameters["verify_complete_single_attempt"].Value.ToString());
            alv1.verify_complete_multiple_attempt = Convert.IsDBNull(objCmd.Parameters["verify_complete_multiple_attempt"].Value.ToString()) ? 0 : Convert.ToInt32(objCmd.Parameters["verify_complete_multiple_attempt"].Value.ToString());
            alv1.verify_average_completed_users = Convert.IsDBNull(objCmd.Parameters["verify_average_completed_users"].Value.ToString()) ? 0 : Convert.ToInt32(objCmd.Parameters["verify_average_completed_users"].Value.ToString());
            alv1.verify_drop_single_attempt = Convert.IsDBNull(objCmd.Parameters["verify_drop_single_attempt"].Value.ToString()) ? 0 : Convert.ToInt32(objCmd.Parameters["verify_drop_single_attempt"].Value.ToString());
            alv1.verify_drop_multiple_attempt = Convert.IsDBNull(objCmd.Parameters["verify_drop_multiple_attempt"].Value.ToString()) ? 0 : Convert.ToInt32(objCmd.Parameters["verify_drop_multiple_attempt"].Value.ToString());
            alv1.verify_average_dropped_users = Convert.IsDBNull(objCmd.Parameters["verify_average_dropped_users"].Value.ToString()) ? "0" : objCmd.Parameters["verify_average_dropped_users"].Value.ToString();
            alv1.verify_successful_attempts = Convert.IsDBNull(objCmd.Parameters["verify_successful_attempts"].Value.ToString()) ? 0 : Convert.ToInt32(objCmd.Parameters["verify_successful_attempts"].Value.ToString());            
            alv1.verify_failed_attempts = Convert.IsDBNull(objCmd.Parameters["verify_failed_attempts"].Value.ToString()) ? 0 : Convert.ToInt32(objCmd.Parameters["verify_failed_attempts"].Value.ToString());

            return alv1;
        }

        //Audit Log Excel Download
        public List<AuditLogExcelDownload> AuditLogExcelDownload(string crn, string event_id, string result_reason, string gate_number, string from_date, string to_date, string version)
        {

            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            if (from_date == null && to_date == null || from_date == "" && to_date == "")
            {
                from_date = DateTime.Now.ToString("yyyy-MM-dd");

                to_date = DateTime.Now.ToString("yyyy-MM-dd");
            }

            parameters.Add(dbManager.CreateParameter("v_crn", crn, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_gate_number", gate_number, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_from_date", from_date, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_to_date", to_date, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_version", version, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_results_reason", result_reason, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("excel_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_EXCEL_DETAILS_DOWNLOAD", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            List<AuditLogExcelDownload> aled = new List<AuditLogExcelDownload>();
            
            while (dataReader.Read())
            {
                
                AuditLogExcelDownload aledi = new AuditLogExcelDownload();
                aledi.CreatedOn = dataReader.IsDBNull(0) ? null : dataReader.GetString(0);
                aledi.CRN = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
                aledi.EventName = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                aledi.Version = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                aledi.StatusName = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                aledi.GateFailedAt = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                aledi.ResultReason = dataReader.IsDBNull(6) ? null : dataReader.GetString(6);
                aledi.ChannelName = dataReader.IsDBNull(7) ? null : dataReader.GetString(7);
                aledi.GATE_NAME = dataReader.IsDBNull(8) ? 0 : dataReader.GetInt32(8);
                aledi.threshold_value = dataReader.IsDBNull(9) ? null : dataReader.GetString(9);
                aledi.json_response = dataReader.IsDBNull(10) ? null : dataReader.GetString(10);
                aled.Add(aledi);
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return aled;
        }

        //Audit Log User Image Zip Download
        public List<UserImageZipDownload> UserImageZipDownload(string crn, string event_id, string result_reason, string gate_number, string from_date, string to_date, string version)
        {

            var dbManager = new DBManager("DBConnectionweb");
            OracleDataReader dataReader = null;
            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            if (from_date == null && to_date == null || from_date == "" && to_date == "")
            {
                from_date = DateTime.Now.ToString("yyyy-MM-dd");

                to_date = DateTime.Now.ToString("yyyy-MM-dd");
            }

            parameters.Add(dbManager.CreateParameter("v_crn", crn, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_gate_number", gate_number, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_from_date", from_date, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_to_date", to_date, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_version", version, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_results_reason", result_reason, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("zip_download_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            OracleCommand objCmd = dbManager.GetCommand("SP_USER_IMAGES_ZIP_DOWNLOAD", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            connection.Open();
            dataReader = objCmd.ExecuteReader();
            List<UserImageZipDownload> aled = new List<UserImageZipDownload>();

            while (dataReader.Read())
            {
                int get_event_id = 0;

                 get_event_id = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt32(3);
                
                //if(get_event_id != 0)
                //{
                    var ImageGallery = new ImageGallery(MongoDBConnectionstring, (Convert.ToInt32(get_event_id) == 1) ? MongoDB_Registration_Databse : MongoDB_Verification_Databse);
                //}
                

                UserImageZipDownload uizd = new UserImageZipDownload();
                uizd.CreatedOn = dataReader.IsDBNull(0) ? null : dataReader.GetString(0);
                uizd.CRN = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
                //uizd.customer_images = dataReader.IsDBNull(2)? null : Convert.ToBase64String(ImageGallery.Get(dataReader.GetString(2)));
                uizd.customer_images = (dataReader.IsDBNull(2) || get_event_id == 0) ? null : Convert.ToBase64String(ImageGallery.Get(dataReader.GetString(2)));
                aled.Add(uizd);
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return aled;
        }

        // GetDataMigrationData Function
        public static List<DataMigrationData> GetDataMigrationData()
        {
            var dbManager = new DBManager("DBConnectionweb");

            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            List<DataMigrationData> dmd = new List<DataMigrationData>();
            parameters.Add(dbManager.CreateParameter("data_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var dataReader = dbManager.GetDataReader("SP_GET_DATA_MIGRATION_DATA", CommandType.StoredProcedure, parameters.ToArray(), out connection);
            while (dataReader.Read())
            {
                dmd.Add(new DataMigrationData
                {
                    CRN = dataReader.IsDBNull(0) ? null : dataReader.GetString(0),
                    created_on = dataReader.IsDBNull(1) ? null : dataReader.GetString(1),
                });
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return dmd;
        }

        // GetCustomerImageData Function for Data Migration
        public static GetCustomerImageData GetCustomerImageData(int customer_registration_checker_id)
        {
            var dbManager = new DBManager("DBConnectionweb");

            OracleConnection connection = null;
            var parameters = new List<OracleParameter>();
            GetCustomerImageData gcid = new GetCustomerImageData();
            parameters.Add(dbManager.CreateParameter("v_customer_registration_checker_id", customer_registration_checker_id, OracleDbType.Int64));
            parameters.Add(dbManager.CreateParameter("image_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            var dataReader = dbManager.GetDataReader("SP_GET_CUSTOMER_IMAGE_DATA", CommandType.StoredProcedure, parameters.ToArray(), out connection);

            var ImageGallery = new ImageGallery(MongoDBConnectionstring, MongoDB_Registration_Databse);
            while (dataReader.Read())
            {

                gcid.customer_registration_checker_id = dataReader.IsDBNull(0) ? 0 : dataReader.GetInt32(0);
                gcid.CRN = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
                gcid.customer_image_id = dataReader.IsDBNull(2) ? null : Convert.ToBase64String(ImageGallery.Get(dataReader.GetString(2)));
                gcid.customer_image2_id = dataReader.IsDBNull(3) ? 0 : dataReader.GetInt32(3);
                gcid.customer_image3_id = dataReader.IsDBNull(4) ? 0 : dataReader.GetInt32(4);
               
            }
            dataReader.Close();
            dbManager.CloseConnection(connection);
            return gcid;
        }

        //Updating Resource group id in customer Checker table
        public static int update_resource_group_id(string resource_group_id, string new_person_id, int customer_registration_checker_id)
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

    }
}
