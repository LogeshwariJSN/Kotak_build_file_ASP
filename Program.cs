﻿using KMBL.StepupAuthentication.CoreComponents.DataAccessHandler;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using RestSharp;
using System.Net;

namespace DashboardWindowsScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            //string FaceIDEndpoint = "https://southeastasia.api.cognitive.microsoft.com";
            //string subscriptionKey = "428def240e9c4d21ba5164dc967d1e8d";
          

            //string PersonGroupId = "002", ProxyFlag = "1", ProxyServerUrl = "http://10.10.2.88:8080/";

            //var client = new RestClient(FaceIDEndpoint + "/face/v1.0/persongroups/" + PersonGroupId + "/train");
            //if (ProxyFlag == "1")
            //    client.Proxy = new WebProxy(ProxyServerUrl);
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
            //request.AddHeader("content-type", "application/json");

            //IRestResponse response = client.Execute(request);

            var client = new RestClient("https://southeastasia.api.cognitive.microsoft.com/face/v1.0/persongroups/002/train");
            //if (ProxyFlag == "1")
            //    client.Proxy = new WebProxy(ProxyServerUrl);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Ocp-Apim-Subscription-Key", "428def240e9c4d21ba5164dc967d1e8d");
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);




            if ((int)response.StatusCode == 202)
                sp_register_data_logs("", 0, 0, 0, "", "", "Response Azure Face Train Scheduler - Person Group Training Success");
            else if ((int)response.StatusCode == 401 || (int)response.StatusCode == 403 || (int)response.StatusCode == 404 || (int)response.StatusCode == 409 || (int)response.StatusCode == 429)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                sp_register_data_logs("", 0, 0, 0, "", result.error.message, "Response Azure Face Train Scheduler");
            }
            else
                sp_register_data_logs("", 0, 0, 0, "", "", "Response Azure Face Train Scheduler - Unable to access the URL");
        }

        public static void sp_register_data_logs(string crn, int event_id, int channel_id, int gate_id, string request_data, string response_data, string log_description)
        {
            var dbManager = new DBManager("DBConnectionWeb");

            var parameters = new List<OracleParameter>();
            parameters.Add(dbManager.CreateParameter("v_crn", crn, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_event_id", event_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_channel_id", channel_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_gate_id", gate_id, OracleDbType.Int32));
            parameters.Add(dbManager.CreateParameter("v_request_data", request_data, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_response_data", response_data, OracleDbType.Varchar2));
            parameters.Add(dbManager.CreateParameter("v_log_description", log_description, OracleDbType.Varchar2));

            dbManager.Insert("INSERT_DATA_LOG", CommandType.StoredProcedure, parameters.ToArray());

        }
    }
}
