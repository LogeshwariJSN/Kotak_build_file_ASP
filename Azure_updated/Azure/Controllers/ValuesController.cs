using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Azure.Controllers
{
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route("azure")]
        public string azure()
        {
            string FaceIDEndpoint = "https://southeastasia.api.cognitive.microsoft.com";
            string subscriptionKey = "428def240e9c4d21ba5164dc967d1e8d";
            string PersonGroupId = "002", ProxyFlag = "0", ProxyServerUrl = "http://10.10.2.88:8080/";
            var client = new RestClient(FaceIDEndpoint + "/face/v1.0/persongroups/" + PersonGroupId + "/train");
            if (ProxyFlag == "1")
                client.Proxy = new WebProxy(ProxyServerUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("ocp-apim-subscription-key", subscriptionKey);
            request.AddHeader("content-type", "application/json");
            IRestResponse response = client.Execute(request);
            dynamic result = JsonConvert.DeserializeObject(response.Content);


            return result;
        }
    }
}
