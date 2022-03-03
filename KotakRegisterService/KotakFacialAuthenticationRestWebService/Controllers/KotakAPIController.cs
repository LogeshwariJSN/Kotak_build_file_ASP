using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using KMBL.StepupAuthentication.CoreComponents;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using KMBL.StepupAuthentication.CoreComponents.DataAccessHandler;
using System.Data;
using Newtonsoft.Json;

namespace KotakFacialAuthenticationRestWebService.Controllers
{
    public class KotakAPIController : ApiController
    {
        // properties.
        private int StatusId { get; set; }
        private string Message { get; set; }

        // MongoDB Database ConnectionString
        private static string MongoDBConnectionString = ConfigurationManager.AppSettings["MongoDBConnectionstring"];
        // MongoDB Database Name
        private static string MongoDB_Registration_Database = ConfigurationManager.AppSettings["MongoDB_Registration_Database"];
        //Subscription Key, Face Endpoint and Group ID
        private string subscriptionKey = ConfigurationManager.AppSettings["FaceAPIKey"], FaceIDEndpoint = ConfigurationManager.AppSettings["FaceAPIEndPoint"], PersonGroupId = ConfigurationManager.AppSettings["PersonGroupId"], DataMigratePersonGroupId = ConfigurationManager.AppSettings["DataMigrationPersonGroupId"];

        // Proxy Server Url & Flag
        private static string ProxyServerUrl = ConfigurationManager.AppSettings["ProxyServerUrl"];
        private static string ProxyFlag = ConfigurationManager.AppSettings["ProxyFlag"];

        private byte[] RegistersecretKey = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["RegisterAPIKey"]);
        private byte[] DeRegistersecretKey = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["DeRegisterAPIKey"]);

        [HttpGet]
        [Route("register_data_migration")]
        public string register_data_migration(int starting_index, int number_of_records, string resource_group_id)
        {
            try

            {
                ExecutingSP es = new ExecutingSP();
                string output = "[";
                sp_register_data_logs("", 0, 0, 0, "","", "Calling Data Migration API");
                List<get_registered_user_data> GetRegisteredUserData = es.GetRegisteredUserData(starting_index,number_of_records,resource_group_id);
                var json = JsonConvert.SerializeObject(new
                {
                    operations = GetRegisteredUserData
                });
                sp_register_data_logs("", 0, 0, 0, "", json, "Data Migration API completed Get Data");

                AzureFaceAPIDataMigration AzureAPI = new AzureFaceAPIDataMigration(FaceIDEndpoint, DataMigratePersonGroupId, subscriptionKey, ProxyServerUrl, ProxyFlag);
                AzureFaceAPIDataMigration result = new AzureFaceAPIDataMigration();

                bool is_not_first = false;
                for (int i=0;i<GetRegisteredUserData.Count;i++)
                {
                    
                    result = AzureAPI.AddPerson(GetRegisteredUserData[i].CRN);
                    if (result.StatusId == 200)
                    {
                        string PersonID = result.Message;

                        for (int j = 0; j < 2; j++)
                        {
                            result = AzureAPI.AddFace(Convert.FromBase64String(GetRegisteredUserData[i].customer_image_id), PersonID); // Calling AddFace Function
                            
                        }
                        if (is_not_first)
                            output += ",";
                        is_not_first = true;

                        output += "{\"Customer Registration Checker ID\":" + GetRegisteredUserData[i].customer_registration_checker_id + ",\"CRN\":\"" +GetRegisteredUserData[i].CRN+"\",\"Registration Status\":\"Success\"}";
                        es.update_resource_group_id(DataMigratePersonGroupId, PersonID, GetRegisteredUserData[i].customer_registration_checker_id);

                        

                    }
                }
                //Data Logs
                sp_register_data_logs("", 0, 0, 0, "", output, "Response Data Migration API on End");
                result = AzureAPI.TrainPersonGroup();
                //int[] customer_registration_checker_id = new int[GetRegisteredUserData.Count];
                
                return output;
            }
            catch(Exception e)
            {
                return "Failure: "+ e.Message;
            }
            
        }

        [HttpPost]
        [Route("GetSurveyQuestionAndAnswer")]
        public object GetSurveyQuestionAndAnswer([FromBody] JObject json)
        {
            ExecutingSP es = new ExecutingSP();
            survey_question_and_answer sqaa = es.survey_question_and_answer();
            return sqaa;
        }

        [HttpPost]
        [Route("KotakRegistrationAPI/Orchestrate")]
        public HttpResponseMessage Post([FromBody] JObject json)
        {
            try { 
                HttpRequestHeaders headers = this.Request.Headers;
                string CRN = null;
                string EventID = null;
                string ChannelID = null;
                string DeviceDetails = null;
                string SequenceId = null;
                string VersionCode = null;

				try
				{
					if (headers.Contains("versioncode"))
					{
						VersionCode = headers.GetValues("versioncode").First();
                        if (InputValidate(VersionCode))
                        {
                            
                            //Data Logs
                            sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 1000, "", "{\"Response\":\"RES100\",\"Message\":'Insufficient Header Details in Gate service Validatation (Version Code)'}", "Response Registration Validation API");


                            throw new InsufficientHeaderDetails("RES100", "Insufficient Header Details in Gate service Validatation (Version Code)");
                        }
					}
					else
					{
						return ExpectationFailed();
					}
					
                    if (headers.Contains("CRN") && headers.Contains("SessionID") && headers.Contains("EventID") && headers.Contains("ChannelID") && headers.Contains("TokenID") && headers.Contains("AppID") && headers.Contains("DeviceDetails") && headers.Contains("ParentTransactionID") && headers.Contains("SequenceId") && headers.Contains("Authorization"))
                    {
                        CRN = headers.GetValues("CRN").First();
                        string SessionID = headers.GetValues("SessionID").First();
                        EventID = headers.GetValues("EventID").First();
                        ChannelID = headers.GetValues("ChannelID").First();
                        string TokenID = headers.GetValues("TokenID").First();
                        string AppID = headers.GetValues("AppID").First();
                        DeviceDetails = headers.GetValues("DeviceDetails").First();
                        string PID = headers.GetValues("ParentTransactionID").First();
                        SequenceId = headers.GetValues("SequenceId").First();
                        string Authorization = headers.GetValues("Authorization").First();

                        //Data Logs
                        sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 0, "{\"CRN\":\"" + CRN + "\",\"SessionID\":\"" + SessionID + "\",\"EventID\":" + EventID + ",\"ChannelID\":" + ChannelID + ",\"TokenID\":" + TokenID.ToString() + ",\"AppID\":" + AppID.ToString() + ",\"DeviceDetails\":\"" + DeviceDetails + "\",\"PID\":\"" + PID.ToString() + "\",\"SequenceID\":\"" + SequenceId + "\",\"Authorization\":\"" + Authorization + "\"}", "", "Calling Registration Validation API");

                        if (InputValidate(CRN) || CRN.Length > 10 || InputValidate(SequenceId) || SequenceId.Length != 32 || InputValidate(Authorization) || InputValidate(SessionID) || InputValidate(EventID) || int.Parse(EventID) != 1 || InputValidate(ChannelID) || (int.Parse(ChannelID) != 3 && int.Parse(ChannelID) != 4 && int.Parse(ChannelID) != 5) || InputValidate(TokenID) || InputValidate(AppID) || InputValidate(DeviceDetails) || InputValidate(PID))
                        {

                            //Data Logs
                            sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 0, "", "{\"Response\":\"RES100\",\"Message\":'Insufficient Header Details in Gate service(CRN, SequenceId, Authorization, SessionID, EventID, ChannelID, TokenID, AppID, DeviceDetails, ParentTransactionID)'}", "Response Registration Validation API");

                            throw new InsufficientHeaderDetails("RES100", "Insufficient Header Details in Registration Service (CRN, SequenceId, Authorization, SessionID, EventID, ChannelID, TokenID, AppID, DeviceDetails, ParentTransactionID)");
                        }
                        if (RegisterRequestHashing(CRN, SessionID, EventID, ChannelID, TokenID, AppID, DeviceDetails, PID,SequenceId, Authorization))
                        {
                            ExecutingSP es = new ExecutingSP();  // Creating Object for Execute SP   
                            
                            // Calling AzureOnStart Function
                            List<string> AzureGatePass_ObjectID = es.SP_AzureExecuteOnStart(null, TokenID, SessionID, CRN, 1000, EventID, ChannelID, DeviceDetails, Convert.ToInt32(PID),VersionCode);

                            string[] ImageObjectID = AzureGatePass_ObjectID[1].ToString().Split(','); //Splitting 2 Object IDs, use 1st objectId as 2nd objectId and 2nd for 3rd
                            if (ImageObjectID.Length == 1)
                            {
                                if (es.SP_ValidateGatePass(CRN, SessionID, 1000, AzureGatePass_ObjectID[0].ToString(), EventID, ChannelID) == 0) //Validating Gate Pass  
                                {
                                    es.SP_AzureOnEnd(CRN, SessionID, 1000, AzureGatePass_ObjectID[0].ToString(), "Azure Registration Failed", 7, Convert.ToInt32(PID), null, null, null, ChannelID, ImageObjectID[0].Trim(), ImageObjectID[0].Trim(), ImageObjectID[0].Trim(), null, null);

                                    //Data Logs
                                    sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 1000, "", "{\"Response\":\"RES202\",\"Message\":'Invalid Gate Pass'}", "Response Registration Validation API");

                                    throw new InsufficientHeaderDetails("RES202", "Invalid Gate Pass");
                                }
                                else
                                {
                                    KotakAPIController res = RegistrationExecute(CRN, AzureGatePass_ObjectID[1].ToString(), PersonGroupId);
                                    if (res.StatusId == 3000) // Calling OnEnd sp 
                                    {
                                        string registration_message = "Registration completed. The Face login feature will get activated after 24 Hrs.";

                                       es.SP_AzureOnEnd(CRN, SessionID, 1000, AzureGatePass_ObjectID[0].ToString(), "Azure Registration Succeeded", 6, Convert.ToInt32(PID), res.Message, null, null, ChannelID, ImageObjectID[0].Trim(), ImageObjectID[0].Trim(), ImageObjectID[0].Trim(), null, null);

                                        //Data Logs
                                        sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 1000, "", "{\"Response\":\"RES200\",\"SequenceID\":\"" + SequenceId + "\"}", "Response Registration Validation API");
                                        
                                        return Registration_Success("RES200", RegisterResponseHashing("RES200", SequenceId), registration_message, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")); //return new { RegistrationStatusCode = "RES200", Message = "" };
                                    }
                                    else // Calling OnEnd sp 
                                    {
                                        es.SP_AzureOnEnd(CRN, SessionID, 1000, AzureGatePass_ObjectID[0].ToString(), "Azure Registration Failed", 7, Convert.ToInt32(PID), null, null, null, ChannelID, ImageObjectID[0].Trim(), ImageObjectID[0].Trim(), ImageObjectID[0].Trim(), null, null);
                                        throw new InsufficientHeaderDetails("RES201", res.Message);
                                    }
                                }
                            }
                            else
                            {
                                es.SP_AzureOnEnd(CRN, SessionID, 1000, AzureGatePass_ObjectID[0].ToString(), "Azure Registration Failed", 7, Convert.ToInt32(PID), null, null, null, ChannelID, ImageObjectID[0].Trim(), ImageObjectID[0].Trim(), ImageObjectID[0].Trim(), null, null);

                                //Data Logs
                                sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 1000, "", "{\"Response\":\"RES203\",\"Message\":'Insufficient images for Registration '}", "Response Registration Validation API");

                                throw new InsufficientHeaderDetails("RES203", "Insufficient images for Registration");
                            }
                        }
                        else
                            return UnAuthorized();
                    }
                    else

                        //Data Logs
                        sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 1000, "", "{\"Response\":\"RES101\",\"Message\":'Insufficient Headers in Registration Service (CRN, SequenceId, Authorization, SessionID, EventID, ChannelID, TokenID, AppID, DeviceDetails, ParentTransactionID)'}", "Response Registration Validation API");

                    throw new InsufficientHeaderDetails("RES101", "Insufficient Headers in Registration Service (CRN, SequenceId, Authorization, SessionID, EventID, ChannelID, TokenID, AppID, DeviceDetails, ParentTransactionID)");
                }
                catch (InsufficientHeaderDetails ie)
                {
                    new ExecutingSP().SP_Exception(CRN, 1, 3, DeviceDetails, ie.Message, null, ie.Source, ie.StackTrace, ie.TargetSite.ToString());
                    if (ie.StatusCode == "RES201" || ie.StatusCode == "RES202" || ie.StatusCode == "RES203")
                    {

                        //Data Logs
                        sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 0, "", "{\"Response\":\"RES201 or  RES202 or RES203\",\"StatusCode\":\"" + ie.StatusCode + "\",\"SequenceID\":\"" + SequenceId + "\"}", "Response Registration Validation API");

                        return Success(ie.StatusCode, RegisterResponseHashing(ie.StatusCode, SequenceId));
                    }
                    else
                    {

                        //Data Logs
                        sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 0, "", "", "Something went wrong in Registration Validation API");

                        return ExpectationFailed();//return new { StatusCode = ie.StatusCode, Message = "Something went wrong, Please try again" };
                    }
                }
                catch (Exception e)
                {
                    new ExecutingSP().SP_Exception(CRN, (EventID == null) ? 1 : int.Parse(EventID), (ChannelID == null) ? 3 : int.Parse(ChannelID), DeviceDetails, e.Message, null, e.Source, e.StackTrace, e.TargetSite.ToString());
                    return ExceptionAccured();//return new { StatusCode = "CRN400", Message = "Something went wrong, Please try again" };
                }
            }
            catch (Exception)
            {
                return ExceptionAccured();
            }
        }

        public void sp_register_data_logs(string crn, int event_id, int channel_id, int gate_id, string request_data, string response_data, string log_description)
        {
            var dbManager = new DBManager("DBConnection");

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

        [HttpPost]
        [Route("KotakDeRegistrationAPI/Orchestrate")]
        public HttpResponseMessage DePost([FromBody] JObject json)
        {
            try
            {
                HttpRequestHeaders headers = this.Request.Headers;
                string CRN = null;
                string EventID = null;
                string ChannelID = null;
                string DeviceDetails = null;
                string SequenceId = null;
                string VersionCode = null;

				try
				{
					if (headers.Contains("versioncode"))
					{
						VersionCode = headers.GetValues("versioncode").First();
                        if (InputValidate(VersionCode))
                        {

                            //Data Logs
                            sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 2000, "", "{\"Response\":\"DRS100\",\"Message\":'Insufficient Header Details in DeRegistration (Version Code)'}", "Response De-Registration Validation API");

                            throw new InsufficientHeaderDetails("DRS100", "Insufficient Header Details in DeRegistration (Version Code)");
                        }
					}
					else
					{
						return ExpectationFailed();
					}
					
                    if (headers.Contains("CRN") && headers.Contains("SessionID") && headers.Contains("ChannelID") && headers.Contains("DeviceDetails") && headers.Contains("EventID") && headers.Contains("SequenceId") && headers.Contains("Authorization"))
                    {
                        CRN = headers.GetValues("CRN").First();
                        string SessionID = headers.GetValues("SessionID").First();
                        EventID = headers.GetValues("EventID").First();
                        ChannelID = headers.GetValues("ChannelID").First();
                        DeviceDetails = headers.GetValues("DeviceDetails").First();
                        SequenceId = headers.GetValues("SequenceId").First();
                        string Authorization = headers.GetValues("Authorization").First();
                        if (InputValidate(CRN) || CRN.Length > 10 || InputValidate(SequenceId) || SequenceId.Length != 32 || InputValidate(Authorization) || InputValidate(SessionID) || InputValidate(ChannelID) || (int.Parse(ChannelID) != 3 && int.Parse(ChannelID) != 4 && int.Parse(ChannelID) != 5) || InputValidate(DeviceDetails) || InputValidate(EventID) || int.Parse(EventID) != 14)
                        {

                            //Data Logs
                            sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 2000, "", "{\"Response\":\"DRS100\",\"Message\":'Insufficient Header Details in DeRegistration (CRN,SequenceId,Authorization, SessionID,ChannelID,EventID and DeviceDetails)'}", "Response De-Registration Validation API");

                            throw new InsufficientHeaderDetails("DRS100", "Insufficient Header Details in DeRegistration (CRN,SequenceId,Authorization, SessionID,ChannelID,EventID and DeviceDetails)");
                        }

                        if (DeRegisterRequestHashing(CRN, SessionID, EventID, ChannelID, DeviceDetails, SequenceId, Authorization))
                        {
                            ExecutingSP es = new ExecutingSP();  // Creating Object for Execute SP
                            KotakAPIController res = DeRegistrationExecute(es.SP_GET_PERSON_ID_DE_REGISTER(CRN));
                            if (res.StatusId == 3000 || res.StatusId == 3001) //DeRegistration Success
                            {
                                es.SP_DE_REGISTER_ON_END(CRN, Convert.ToInt32(SessionID), Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 2000, 6, DeviceDetails, "Azure De-registration Succeeded",VersionCode);//sp execute to change IS_Register=0 in Registration Checker Table and Insert entry into Audit log

                                //Data Logs
                                sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 2000, "", "{\"Response\":\"DRS200\",\"SequenceID\":\"" + SequenceId + "\"}", "Response De-Registeration Validation API");

                                return Success("DRS200", DeRegisterResponseHashing("DRS200", SequenceId));//return new { RegistrationStatusCode = "DRS200", Message = "Azure De-registration Succeeded" };
                            }
                            else // Calling OnEnd sp 
                            {
                                es.SP_DE_REGISTER_ON_END(CRN, Convert.ToInt32(SessionID), Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 2000, 7, DeviceDetails, "Azure De-registration Failed",VersionCode);

                                //Data Logs
                                sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 2000, "", "{\"Response\":\"DRS201\",\"Message\":\"" + res.Message + "\"}", "Response De-Registeration Validation API");

                                throw new InsufficientHeaderDetails("DRS201", res.Message);
                            }
                        }
                        else
                            return UnAuthorized();
                    }
                    else

                        //Data Logs
                        sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 2000, "", "{\"Response\":\"DRS101\",\"Message\":'Insufficient Headers  in DeRegistration (CRN,SequenceId,Authorization, SessionID,ChannelID,EventID and DeviceDetails)'}", "Response De-Registration Validation API");

                    throw new InsufficientHeaderDetails("DRS101", "Insufficient Headers  in DeRegistration (CRN,SequenceId,Authorization, SessionID,ChannelID,EventID and DeviceDetails)");
                }
                catch (InsufficientHeaderDetails ie)
                {
                    new ExecutingSP().SP_Exception(CRN, 1, 3, DeviceDetails, ie.Message, null, ie.Source, ie.StackTrace, ie.TargetSite.ToString());
                    if (ie.StatusCode == "DRS201")
                    {

                        //Data Logs
                        sp_register_data_logs(CRN, Convert.ToInt32(EventID), Convert.ToInt32(ChannelID), 2000, "", "", "Azure De-registration Failed");

                        return Success(ie.StatusCode, DeRegisterResponseHashing(ie.StatusCode, SequenceId));
                    }
                    else
                        return ExpectationFailed();//return new { StatusCode = ie.StatusCode, Message = "Something went wrong, Please try again" };
                }
                catch (Exception e)
                {
                    new ExecutingSP().SP_Exception(CRN, (EventID == null) ? 1 : int.Parse(EventID), (ChannelID == null) ? 3 : int.Parse(ChannelID), DeviceDetails, e.Message, null, e.Source, e.StackTrace, e.TargetSite.ToString());
                    return ExceptionAccured();//return new { StatusCode = "CRN400", Message = "Something went wrong, Please try again" };
                }
            }
            catch (Exception)
            {
                return ExceptionAccured();
            }
        }


        // Azure Face RegistrationExecute Function
        private KotakAPIController RegistrationExecute(string CRN, string ObjectID, string PersonGroupId)
        {
            AzureFaceAPIs AzureAPI = new AzureFaceAPIs(FaceIDEndpoint, PersonGroupId, subscriptionKey, ProxyServerUrl, ProxyFlag);
            AzureFaceAPIs result = AzureAPI.AddPerson(CRN);
            if (result.StatusId == 200)// Registering using 3 Images
            {
                string PersonID = result.Message;
                string[] images = ObjectID.Split(',');                
                //for (int i = 0; i < images.Length; i++)
                //{
                byte[] imagear = new ImageGallery(MongoDBConnectionString, MongoDB_Registration_Database).Get(images[0].Trim()); // Getting Image from MongoDB 
                for(int i = 0; i < 2; i++)
                {
                    result = AzureAPI.AddFace(imagear, PersonID); // Calling AddFace Function
                    if (result.StatusId == 400 || result.StatusId == 500)
                    {
                        //before going back, we need to delete person
                        //AzureAPI.DeletePerson(PersonID);
                        return new KotakAPIController { StatusId = 3001, Message = result.Message };
                    }
                }
                    
                //}
                //result = AzureAPI.TrainPersonGroup();
                //if (result.StatusId == 200)
                    return new KotakAPIController { StatusId = 3000, Message = PersonID };//success
                //else
                  //  return new KotakAPIController { StatusId = 3001, Message = result.Message };//failure
            }
            else
                return new KotakAPIController { StatusId = 3001, Message = result.Message };
        }


        // Azure Face RegistrationExecute Function
        private KotakAPIController DeRegistrationExecute(string PersonID)
        {
            AzureFaceAPIs AzureAPI = new AzureFaceAPIs(FaceIDEndpoint, PersonGroupId, subscriptionKey, ProxyServerUrl, ProxyFlag);

            //delete a person from Azure Face Model
            AzureFaceAPIs result = AzureAPI.DeletePerson(PersonID);
            if (result.StatusId == 200)
            {
                result = AzureAPI.TrainPersonGroup();
                if (result.StatusId == 200)



                    return new KotakAPIController { StatusId = 3000, Message = "Azure DeRegistration Succeeded" };//success
                else
                    return new KotakAPIController { StatusId = 3001, Message = "Azure DeRegistration Succeeded and Person Group Training Failed" };//success
            }
            else
                return new KotakAPIController { StatusId = 3002, Message = result.Message };
        }

        private HttpResponseMessage Registration_Success(string Code, string Hash, string Message, string Timestamp)
        {
            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK); //200               
            Response.Content = new StringContent("{\"StatusCode\":\"" + Code + "\",\"Authorization\":\"" + Hash + "\",\"Message\":\"" + Message + "\",\"Timestamp\":\"" + Timestamp + "\"}");
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Response;
        }


        private HttpResponseMessage Success(string Code, string Hash)
        {
            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK); //200               
            Response.Content = new StringContent("{\"StatusCode\":\"" + Code + "\",\"Authorization\":\"" + Hash + "\"}");
            Response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Response;
        }

        private bool RegisterRequestHashing(string CRN, string SessionID, string EventID, string ChannelID, string TokenID, string AppID, string DeviceDetails, string PID, string SequenceId, string Signature)
        {
            return Signature.Equals(RegisterGetHash(SequenceId.Substring(13, 4) +CRN + SessionID + EventID + ChannelID + TokenID + AppID + DeviceDetails + PID + SequenceId.Substring(23, 4)), StringComparison.Ordinal);
        }

        private string RegisterResponseHashing(string Code, string SequenceId)
        {
            return RegisterGetHash(SequenceId.Substring(17, 4) + Code + SequenceId.Substring(22, 4));
        }

        private bool DeRegisterRequestHashing(string CRN, string SessionID, string EventID, string ChannelID, string DeviceDetails,string SequenceId, string Signature)
        {
            return Signature.Equals(DeRegisterGetHash(SequenceId.Substring(2, 4) + CRN + SessionID + EventID + ChannelID + DeviceDetails + SequenceId.Substring(10, 4)), StringComparison.Ordinal);
        }

        private string DeRegisterResponseHashing(string Code, string SequenceId)
        {
            return DeRegisterGetHash(SequenceId.Substring(15, 4) + Code + SequenceId.Substring(20, 4));
        }

        private HttpResponseMessage UnAuthorized()
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized); //401              
        }

        private HttpResponseMessage ExpectationFailed()
        {
            return new HttpResponseMessage(HttpStatusCode.ExpectationFailed); //417              
        }

        private HttpResponseMessage ExceptionAccured()
        {
            return new HttpResponseMessage(HttpStatusCode.Conflict); //409              
        }

        private string RegisterGetHash(string Text)
        {
            HMACSHA256 hmac = new HMACSHA256(RegistersecretKey);
            hmac.Initialize();
            byte[] bytes = Encoding.UTF8.GetBytes(Text);
            byte[] rawHmac = hmac.ComputeHash(bytes);
            return Convert.ToBase64String(rawHmac);
        }
        private string DeRegisterGetHash(string Text)
        {
            HMACSHA256 hmac = new HMACSHA256(DeRegistersecretKey);
            hmac.Initialize();
            byte[] bytes = Encoding.UTF8.GetBytes(Text);
            byte[] rawHmac = hmac.ComputeHash(bytes);
            return Convert.ToBase64String(rawHmac);
        }


        private bool InputValidate(string SrtIn)
        {
            return (SrtIn == string.Empty || SrtIn.Trim().Length == 0 || SrtIn is null) ? true : false;
        }

        
    }
}