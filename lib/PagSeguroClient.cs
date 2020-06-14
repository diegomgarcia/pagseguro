using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PagSeguro
{
    public enum PagSeguroPreApprovalPlanChargeType
    {
        Auto,
        Manual
    }

    public enum PagSeguroPreApprovalExpirationUnitType
    {
        Days,
        Months,
        Years
    }

    public enum PagSeguroApprovalChargePeriodType
    {
        Weekly,
        Monthly,
        Bimonthly,
        Trimonthly,
        SemiAnnually,
        Yearly
    }

    public class PagSeguroPreApprovalPlanDetails
    {
        public string Reference { get; set; }
        public string Name { get; set; }

        public PagSeguroPreApprovalPlanChargeType Charge { get; set; }
        public string ChargeText
        {
            get
            {
                switch (Charge)
                {
                    case PagSeguroPreApprovalPlanChargeType.Auto: return "AUTO";
                    case PagSeguroPreApprovalPlanChargeType.Manual: return "MANUAL";
                    default: return "AUTO";
                }
            }
        }

        public PagSeguroApprovalChargePeriodType ChargePeriod { get; set; }
        public string ChargePeriodText
        {
            get
            {
                switch (ChargePeriod)
                {
                    case PagSeguroApprovalChargePeriodType.Weekly: return "WEEKLY";
                    case PagSeguroApprovalChargePeriodType.Monthly: return "MONTHLY";
                    case PagSeguroApprovalChargePeriodType.Bimonthly: return "BIMONTHLY";
                    case PagSeguroApprovalChargePeriodType.Trimonthly: return "TRIMONTHLY";
                    case PagSeguroApprovalChargePeriodType.SemiAnnually: return "SEMIANNUALLY";
                    case PagSeguroApprovalChargePeriodType.Yearly: return "YEARLY";
                    default: return "MONTHLY";
                }
            }
        }

        public Double AmountPerPayment { get; set; }
        public int TrialPeriodDuration { get; set; }
        public int ExpirationValue { get; set; }

        public PagSeguroPreApprovalExpirationUnitType ExpirationUnit { get; set; }
        public string ExpirationUnitText
        {
            get
            {
                switch (ExpirationUnit)
                {
                    case PagSeguroPreApprovalExpirationUnitType.Days: return "DAYS";
                    case PagSeguroPreApprovalExpirationUnitType.Months: return "MONTHS";
                    case PagSeguroPreApprovalExpirationUnitType.Years: return "YEARS";
                    default: return "DAYS";
                }
            }
        }


        public string Details { get; set; }

    }

    public class PagSeguroPreApprovalPlanResponse
    {
        public bool IsSandBox { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string CheckoutUrl => (IsSandBox ? $"https://sandbox.pagseguro.uol.com.br/v2/pre-approvals/request.html?code={Code}" : $"https://pagseguro.uol.com.br/v2/pre-approvals/request.html?code={Code}");
    }


    public class PagSeguroTransactionNotification
    {
        public string notificationCode { get; set; }
        public string notificationType { get; set; }
    }


    public class PagSeguroClient
    {
        private string Email { get; set; }
        private string Token { get; set; }


        private const string BaseUrl = "https://ws.pagseguro.uol.com.br";
        private const string SandBoxBaseUrl = "https://ws.sandbox.pagseguro.uol.com.br";
        private const string PreApprovalRequestMethod = "/pre-approvals/request";


        private bool IsSandBox { get; set; }
        private bool UseProxy { get; set; }
        private string ProxyHost { get; set; }
        private string ProxyPort { get; set; }


        public PagSeguroClient(bool isSandBox, string email, string token, bool useProxy = false, string proxyHost = "localhost", string proxyPort = "8888")
        {
            IsSandBox = isSandBox;

            Email = email;
            Token = token;

            UseProxy = useProxy;
            ProxyHost = proxyHost;
            ProxyPort = proxyPort;
        }


        private HttpClient GetHttpClient()
        {
            if (UseProxy)
            {
                var proxy = new WebProxy()
                {
                    Address = new Uri("http://" + ProxyHost + ":" + ProxyPort),
                    UseDefaultCredentials = true
                };

                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                };

                return new HttpClient(handler: httpClientHandler, disposeHandler: true);
            }

            return new HttpClient();
        }

        private HttpRequestMessage GetPreparedPostRequest(string url, List<KeyValuePair<string, string>> content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new FormUrlEncodedContent(content);

            request.Headers.Add("Accept", "application/vnd.pagseguro.com.br.v3+json;charset=ISO-8859-1");
            request.Content.Headers.ContentType.CharSet = "ISO-8859-1";

            foreach (var v in request.Headers.Accept)
            {
                if (v.MediaType.Contains("application/vnd.pagseguro.com.br"))
                {
                    var field = v.GetType().GetTypeInfo().BaseType.GetField("_mediaType", BindingFlags.NonPublic | BindingFlags.Instance);
                    field.SetValue(v, "application/vnd.pagseguro.com.br.v3+json;charset=ISO-8859-1");
                    v.Parameters.Clear();
                }
            }

            return request;
        }

        public async Task<PagSeguroPreApprovalPlanResponse> CreatePreApprovalPlan(PagSeguroPreApprovalPlanDetails preApproval)
        {
            HttpClient client = GetHttpClient();

            client.BaseAddress = (IsSandBox ? new Uri(SandBoxBaseUrl) : new Uri(BaseUrl));

            var content = new List<KeyValuePair<string, string>>();

            content.Add(new KeyValuePair<string, string>("reference", preApproval.Reference));
            content.Add(new KeyValuePair<string, string>("preApprovalName", preApproval.Name));
            content.Add(new KeyValuePair<string, string>("preApprovalCharge", preApproval.ChargeText));
            content.Add(new KeyValuePair<string, string>("preApprovalPeriod", preApproval.ChargePeriodText));
            content.Add(new KeyValuePair<string, string>("preApprovalAmountPerPayment", preApproval.AmountPerPayment.ToString("0.00")));

            if (preApproval.TrialPeriodDuration > 0)
            {
                content.Add(new KeyValuePair<string, string>("preApprovalTrialPeriodDuration",
                    preApproval.TrialPeriodDuration.ToString()));
            }

            content.Add(new KeyValuePair<string, string>("preApprovalExpirationValue", preApproval.ExpirationValue.ToString()));
            content.Add(new KeyValuePair<string, string>("preApprovalExpirationUnit", preApproval.ExpirationUnitText));
            content.Add(new KeyValuePair<string, string>("preApprovalDetails", preApproval.Details));
            content.Add(new KeyValuePair<string, string>("receiverEmail", Email));


            var url = PreApprovalRequestMethod + $"?email={Email}&token={Token}";

            var response = await client.SendAsync(GetPreparedPostRequest(url, content));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                PagSeguroPreApprovalPlanResponse preApprovalResponse =
                    JsonConvert.DeserializeObject<PagSeguroPreApprovalPlanResponse>(response.Content
                        .ReadAsStringAsync().Result);
                preApprovalResponse.IsSandBox = IsSandBox;

                return preApprovalResponse;
            }

            throw new Exception($"Ocorreu um erro ao registrar a solicitação. Status Resposta: {response.StatusCode}, Resposta: {response.Content.ReadAsStringAsync().Result}");

        }

    }
}
