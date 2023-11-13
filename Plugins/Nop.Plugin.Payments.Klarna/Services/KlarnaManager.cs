using DocumentFormat.OpenXml.Vml;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nop.Plugin.Payments.Klarna.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Nop.Plugin.Payments.Klarna.Services
{
    public class KlarnaManager
    {
        public static bool SinglePaymentMBWayRequest(KlarnaRequestModel model, string AccountId, string ApiKey, bool isSandbox)
        {
            try
            {
                RestClient client = isSandbox ? new RestClient("https://api.test.easypay.pt/2.0") : new RestClient(" https://api.prod.easypay.pt/2.0");
                RestRequest request = new RestRequest("single", RestSharp.Method.POST, DataFormat.Json);
                request.AddHeader("AccountId", AccountId);
                request.AddHeader("ApiKey", ApiKey);
                string message = JsonConvert.SerializeObject(model, new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd HH:mm"
                });

                request.AddParameter("application/json", message, ParameterType.RequestBody);
                var restResponse = client.Post(request);

                //IRestResponse restResponse = client.Post((IRestRequest)request);

                dynamic val = JsonConvert.DeserializeObject(restResponse.Content);

                if (val.status.Value == "error")
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static (string, string) SinglePaymentMBReferenceRequest(KlarnaRequestModel model, string AccountId, string ApiKey, bool isSandbox)
        {
            RestClient client = isSandbox ? new RestClient("https://api.test.easypay.pt/2.0") : new RestClient(" https://api.prod.easypay.pt/2.0");
            RestRequest request = new RestRequest("single", Method.POST, DataFormat.Json);
            request.AddHeader(nameof(AccountId), AccountId);
            request.AddHeader(nameof(ApiKey), ApiKey);
            string message = JsonConvert.SerializeObject((object)model, (JsonConverter)new IsoDateTimeConverter()
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm"
            });

            request.AddParameter("application/json", (object)message, ParameterType.RequestBody);
            IRestResponse restResponse = client.Post((IRestRequest)request);

            dynamic val = JsonConvert.DeserializeObject(restResponse.Content);
            if (val.status.Value == "error")
            {
                return (null, null);
            }
            string item = val.method.entity.Value;
            string item2 = val.method.reference.Value;
            return (item, item2);
        }

        public static string SinglePaymentCreditCardRequest(KlarnaRequestModel model, string AccountId, string ApiKey, bool isSandbox)
        {
            RestClient client = (isSandbox ? new RestClient("https://api.test.easypay.pt/2.0") : new RestClient(" https://api.prod.easypay.pt/2.0"));
            RestRequest restRequest = new RestRequest("single", Method.POST, DataFormat.Json);
            restRequest.AddHeader("AccountId", AccountId);
            restRequest.AddHeader("ApiKey", ApiKey);
            string text = JsonConvert.SerializeObject((object)model, (JsonConverter[])(object)new JsonConverter[1] { (JsonConverter)new IsoDateTimeConverter
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm"
        } });
            Debug.WriteLine(text);
            restRequest.AddParameter("application/json", text, ParameterType.RequestBody);
            IRestResponse restResponse = client.Post(restRequest);
            Debug.WriteLine(restResponse.Content);
            dynamic val = JsonConvert.DeserializeObject(restResponse.Content);
            return val.method.url.Value;
        }

        public static string SinglePaymentSantanderConsumerRequest(KlarnaRequestModel model, string AccountId, string ApiKey, bool isSandbox)
        {
            try
            {
                RestClient client = isSandbox ? new RestClient("https://api.test.easypay.pt/2.0") : new RestClient(" https://api.prod.easypay.pt/2.0");
                RestRequest request = new RestRequest("single", RestSharp.Method.POST, DataFormat.Json);
                request.AddHeader("AccountId", AccountId);
                request.AddHeader("ApiKey", ApiKey);

                string message = JsonConvert.SerializeObject(model, new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd HH:mm"
                });

                request.AddParameter("application/json", message, ParameterType.RequestBody);
                var restResponse = client.Post(request);

                //IRestResponse restResponse = client.Post((IRestRequest)request);

                dynamic val = JsonConvert.DeserializeObject(restResponse.Content);

                if (val.status.Value == "error")
                {
                    return "";
                }
                return val.method.url.Value;

            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string SingleDetailsRequest(string id, string AccountId, string ApiKey, bool isSandbox)
        {
            RestClient client = (isSandbox ? new RestClient("https://api.test.easypay.pt/2.0") : new RestClient(" https://api.prod.easypay.pt/2.0"));
            RestRequest restRequest = new RestRequest("single/" + id, Method.GET, DataFormat.Json);
            restRequest.AddHeader("AccountId", AccountId);
            restRequest.AddHeader("ApiKey", ApiKey);
            IRestResponse restResponse = client.Get(restRequest);
            //Debug.WriteLine("SINGLE DETAILS: " + restResponse.Content);
            return restResponse.Content;
        }

        public static List<string> SingleDetailsListRequest(KlarnaListPaymentRequestModel model, string AccountId, string ApiKey, bool isSandbox)
        {
            Uri baseUrl = (isSandbox ? new Uri("https://api.test.easypay.pt/2.0") : new Uri("https://api.prod.easypay.pt/2.0"));
            RestClient client = new RestClient(baseUrl);
            RestRequest restRequest = new RestRequest("single", Method.GET, DataFormat.Json);
            restRequest.AddHeader("AccountId", AccountId);
            restRequest.AddHeader("ApiKey", ApiKey);
            restRequest.AddQueryParameter("page", model.Page.ToString());
            restRequest.AddQueryParameter("records_per_page", model.RecordsPerPage.ToString());
            restRequest.AddQueryParameter("type", model.Type.ToString());
            restRequest.AddQueryParameter("created_at", model.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            IRestResponse restResponse = client.Get(restRequest);
            Debug.WriteLine("SINGLE DETAILS LIST: " + restResponse.Content);
            dynamic val = JsonConvert.DeserializeObject(restResponse.Content);
            dynamic val2 = val.data;
            if (val2 == null)
            {
                return new List<string>();
            }
            //return (from x in (IEnumerable<object>)val2
            //        where x.Payment_Status.Value == "paid"
            //        select (string)Convert.ToString(x.key.Value))?.ToList();
            return new List<string>();
        }


    }
}