using RestSharp;
using Newtonsoft.Json;
using System.Net;

namespace KMBL.StepupAuthentication.CoreComponents
{
    public class AzureFaceAPIDataMigration
    {
        private string FaceIDEndpoint, PersonGroupId, subscriptionKey, ProxyServerUrl, ProxyFlag;

        // properties.
        public int StatusId { get; set; }
        public string Message { get; set; }

        //constructor
        public AzureFaceAPIDataMigration() { }

        public AzureFaceAPIDataMigration(string FaceIDEndpoint, string PersonGroupId, string subscriptionKey, string ProxyServerUrl = null, string ProxyFlag = "0")
        {
            this.FaceIDEndpoint = FaceIDEndpoint;
            this.PersonGroupId = PersonGroupId;
            this.subscriptionKey = subscriptionKey;
            this.ProxyServerUrl = ProxyServerUrl;
            this.ProxyFlag = ProxyFlag;
        }

        // Add a New Person
        /*StatusId 200 for success, StatusId 400 for Fail  and StatusId 500 for Not able to access*/
        public AzureFaceAPIDataMigration AddPerson(string name)
        {
            var client = new RestClient(FaceIDEndpoint + "/face/v1.0/persongroups/" + PersonGroupId + "/persons");
            if (ProxyFlag == "1")
                client.Proxy = new WebProxy(ProxyServerUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("ocp-apim-subscription-key", subscriptionKey);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"name\": \"" + name + "\"\r\n}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 200, Message = result.personId };
            }
            else if ((int)response.StatusCode == 400 || (int)response.StatusCode == 401 || (int)response.StatusCode == 403 || (int)response.StatusCode == 404 || (int)response.StatusCode == 409 || (int)response.StatusCode == 415 || (int)response.StatusCode == 429)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 400, Message = result.error.message };
            }
            else
                return new AzureFaceAPIDataMigration { StatusId = 500, Message = "Unable to access the URL" };
        }


        // AddFace Function
        /*StatusId 200 for success, StatusId 400 for Fail  and StatusId 500 for Not able to access*/
        public AzureFaceAPIDataMigration AddFace(byte[] imageBytes, string PersonID)
        {
            var client = new RestClient(FaceIDEndpoint + "/face/v1.0/persongroups/" + PersonGroupId + "/persons/" + PersonID + "/persistedFaces?detectionModel=detection_03");
            if (ProxyFlag == "1")
                client.Proxy = new WebProxy(ProxyServerUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("ocp-apim-subscription-key", subscriptionKey);
            request.AddHeader("content-type", "application/octet-stream");
            request.AddParameter("undefined", imageBytes, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 200, Message = result.persistedFaceId };
            }
            else if ((int)response.StatusCode == 400 || (int)response.StatusCode == 401 || (int)response.StatusCode == 403 || (int)response.StatusCode == 404 || (int)response.StatusCode == 408 || (int)response.StatusCode == 409 || (int)response.StatusCode == 415 || (int)response.StatusCode == 429)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 400, Message = result.error.message };
            }
            else
                return new AzureFaceAPIDataMigration { StatusId = 500, Message = "Unable to access the URL" };
        }



        // TrainPersonGroup Function
        /*StatusId 200 for success, StatusId 400 for Fail  and StatusId 500 for Not able to access*/
        public AzureFaceAPIDataMigration TrainPersonGroup()
        {
            var client = new RestClient(FaceIDEndpoint + "/face/v1.0/persongroups/" + PersonGroupId + "/train");
            if (ProxyFlag == "1")
                client.Proxy = new WebProxy(ProxyServerUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("ocp-apim-subscription-key", subscriptionKey);
            request.AddHeader("content-type", "application/json");

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 202)
                return new AzureFaceAPIDataMigration { StatusId = 200, Message = "Person Group Training Success" };
            else if ((int)response.StatusCode == 401 || (int)response.StatusCode == 403 || (int)response.StatusCode == 404 || (int)response.StatusCode == 409 || (int)response.StatusCode == 429)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 400, Message = result.error.message };
            }
            else
                return new AzureFaceAPIDataMigration { StatusId = 500, Message = "Unable to access the URL" };
        }



        // Face Detection Function
        /*StatusId 200 for success, 201 for No Face Found, 202 for Multiple Face Found, 400 for Fail  and 500 for Not able to access*/
        public AzureFaceAPIDataMigration DetectFace(byte[] imageBytes)
        {
            var client = new RestClient(FaceIDEndpoint + "/face/v1.0/detect?returnFaceId=true&returnFaceLandmarks=false&recognitionModel=recognition_01&returnRecognitionModel=false");
            if (ProxyFlag == "1")
                client.Proxy = new WebProxy(ProxyServerUrl);
            var request = new RestRequest(Method.POST);
            request.AddParameter("undefined", imageBytes, ParameterType.RequestBody);
            request.AddHeader("ocp-apim-subscription-key", subscriptionKey);
            request.AddHeader("content-type", "application/octet-stream");

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                if (result.Count == 1)
                    return new AzureFaceAPIDataMigration { StatusId = 200, Message = result[0].faceId };
                else if (result.Count == 0)
                    return new AzureFaceAPIDataMigration { StatusId = 201, Message = "No Face Found" };
                else
                    return new AzureFaceAPIDataMigration { StatusId = 202, Message = "Multiple Face Found" };
            }
            else if ((int)response.StatusCode == 400 || (int)response.StatusCode == 401 || (int)response.StatusCode == 403 || (int)response.StatusCode == 408 || (int)response.StatusCode == 415 || (int)response.StatusCode == 429)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 400, Message = result.error.message };
            }
            else
                return new AzureFaceAPIDataMigration { StatusId = 500, Message = "Unable to access the URL" };
        }



        // Face Detection Function
        /*StatusId 200 for success, 201 for Unauthorized Person, 400 for Fail  and 500 for Not able to access*/
        public AzureFaceAPIDataMigration IdentifyPerson(string FaceID)
        {
            var client = new RestClient(FaceIDEndpoint + "/face/v1.0/identify");
            if (ProxyFlag == "1")
                client.Proxy = new WebProxy(ProxyServerUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("ocp-apim-subscription-key", subscriptionKey);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\r\n    \"personGroupId\": \"" + PersonGroupId + "\",\r\n    \"faceIds\": [\r\n        \"" + FaceID + "\"\r\n    ],\r\n    \"maxNumOfCandidatesReturned\": 1,\r\n    \"confidenceThreshold\": 0.6\r\n}", ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                if (result.Count == 1 && result[0].candidates.Count == 1)
                    return new AzureFaceAPIDataMigration { StatusId = 200, Message = result[0].candidates[0].personId };
                else
                    return new AzureFaceAPIDataMigration { StatusId = 201, Message = "Unauthorized Person" };
            }
            else if ((int)response.StatusCode == 400 || (int)response.StatusCode == 401 || (int)response.StatusCode == 403 || (int)response.StatusCode == 409 || (int)response.StatusCode == 415 || (int)response.StatusCode == 429)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 400, Message = result.error.message };
            }
            else
                return new AzureFaceAPIDataMigration { StatusId = 500, Message = "Unable to access the URL" };
        }



        // Get a Person Info using PersonId
        /*StatusId 200 for success, StatusId 400 for Fail  and StatusId 500 for Not able to access*/
        public AzureFaceAPIDataMigration GetPersonInfo(string personId)
        {
            var client = new RestClient(FaceIDEndpoint + "/face/v1.0/persongroups/" + PersonGroupId + "/persons/" + personId);
            if (ProxyFlag == "1")
                client.Proxy = new WebProxy(ProxyServerUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("ocp-apim-subscription-key", subscriptionKey);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 200, Message = result.name };
            }
            else if ((int)response.StatusCode == 401 || (int)response.StatusCode == 403 || (int)response.StatusCode == 404 || (int)response.StatusCode == 409 || (int)response.StatusCode == 429)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 400, Message = result.error.message };
            }
            else
                return new AzureFaceAPIDataMigration { StatusId = 500, Message = "Unable to access the URL" };
        }



        // Delete a Person using PersonId
        /*StatusId 200 for success, StatusId 400 for Fail  and StatusId 500 for Not able to access*/
        public AzureFaceAPIDataMigration DeletePerson(string personId)
        {
            var client = new RestClient(FaceIDEndpoint + "/face/v1.0/persongroups/" + PersonGroupId + "/persons/" + personId);
            if (ProxyFlag == "1")
                client.Proxy = new WebProxy(ProxyServerUrl);
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("ocp-apim-subscription-key", subscriptionKey);

            IRestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
                return new AzureFaceAPIDataMigration { StatusId = 200, Message = "Person Group Deleted Successfully" };
            else if ((int)response.StatusCode == 401 || (int)response.StatusCode == 403 || (int)response.StatusCode == 404 || (int)response.StatusCode == 409 || (int)response.StatusCode == 429)
            {
                dynamic result = JsonConvert.DeserializeObject(response.Content);
                return new AzureFaceAPIDataMigration { StatusId = 400, Message = result.error.message };
            }
            else
                return new AzureFaceAPIDataMigration { StatusId = 500, Message = "Unable to access the URL" };
        }
    }
}
