using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
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
        
        public Double MembershipFee { get; set; }
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
        
        public Double MaxAmountPerPeriod { get; set; }
        public Double MaxAmountPerPayment { get; set; }
        public Double MaxTotalAmount { get; set; }
        public Double MaxPaymentsPerPeriod { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public string DayOfYear { get; set; }
        public string DayOfMonth { get; set; }
        public string DayOfWeek { get; set; }
        
        public string CancelUrl { get; set; }
        public int MaxUses { get; set; }
    }

    public class PagSeguroPreApprovalPlanResponse
    {
        public bool IsSandBox { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string CheckoutUrl => (IsSandBox ? $"https://sandbox.pagseguro.uol.com.br/v2/pre-approvals/request.html?code={Code}" : $"https://pagseguro.uol.com.br/v2/pre-approvals/request.html?code={Code}");
    }

    public class PagSeguroPaymentResponse
    {
        public string TransactionCode { get; set; }
        public DateTime Date { get; set; }
    }

    public class PagSeguroAddress
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
    }

    public class PagSeguroCreditorFees
    {
        public decimal IntermediationRateAmount { get; set; }
        public decimal IntermediationFeeAmount { get; set; }
    }
    
    public enum PagSeguroShippingType
    {
        PAC = 1,
        Sedex = 2,
        NotSpecified = 3
    }
    
    public class PagSeguroShipping
    {
        public PagSeguroAddress Address { get; set; }
        public PagSeguroShipping ShippingType { get; set; }
        public decimal? Cost { get; set; }
    }

    public enum PagSeguroPaymentMethodType
    {
        CartaoCredito = 1,
        Boleto = 2,
        DebitoOnline = 3,
        SaldoPagSeguro = 4,
        OiPaggo = 5, //Not available for use
        DepositoConta = 7
    }
    
    public enum PagSeguroPaymentMethodCode
    {
        CartaoCreditoVisa = 101,
        CartaoCreditoMasterCard = 102,
        CartaoCreditoAmericanExpress = 103,
        CartaoCreditoDiners = 104,
        CartaoCreditoHipercard = 105,
        CartaoCreditoAura = 106,
        CartaoCreditoElo = 107,
        CartaoCreditoPlenoCard = 108,
        CartaoCreditoPersonalCard = 109,
        CartaoCreditoJCB = 110,
        CartaoCreditoDiscover = 111,
        CartaoCreditoBrasilCard = 112,
        CartaoCreditoFORTBrasil = 113,
        CartaoCreditoCARDBAN = 114,
        CartaoCreditoVALECard = 115,
        CartaoCreditoCabal = 116,
        CartaoCreditoMais = 117,
        CartaoCreditoAvista = 118,
        CartaoCreditoGRANDCard = 119,
        CartaoCreditoSorocred = 120,
        BoletoBradesco = 201, //Not available for use
        BoletoSantander = 202,
        DebitoOnlineBradesco = 301,
        DebitoOnlineItau = 302,
        DebitoOnlineUnibanco = 303, //Not available for use
        DebitoOnlineBancoBrasil = 304,
        DebitoOnlineBancoReal = 305, //Not available for use
        DebitoOnlineBanrisul = 306,
        DebitoOnlineHSBC = 307,
        SaldoPagseguro = 401,
        OiPaggo = 501, //Not available for use
        DepositoContaBancoBrasil = 701,
        DepositoContaHSBC = 702
    }
    
    public class PagSeguroPaymentMethod
    {
        public PagSeguroPaymentMethodType Type { get; set; }
        public PagSeguroPaymentMethodCode Code { get; set; }
        
    }

    public enum PagSeguroTransactionType
    {
        Payment = 1,
        Transfer = 2,
        FundAddition = 3,
        Withdraw = 4,
        Charge = 5,
        Donation = 6, 
        Bonus = 7,
        BonusRepass = 8,
        Operational = 9,
        PoliticalDonation = 10
    }

    public enum PagSeguroTransactionStatus
    {
        Initiated = 0,
        WaitingPayment = 1,
        InAnalysis = 2,
        Paid = 3,
        Available = 4,
        InDispute = 5,
        Refunded = 6,
        Cancelled = 7
    }

    public class PagSeguroPaymentItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public double Ammount { get; set; }
        public int Quantity { get; set; }
    }

    public class PagSeguroPhone
    {
        public string AreaCode { get; set; }
        public string Number { get; set; }
    }
    
    public class PagSeguroSender 
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public PagSeguroPhone Phone { get; set; }
    }

    
    public class PagSeguroTransaction
    {
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public string Reference { get; set; }
        public PagSeguroTransactionType Type { get; set; }
        public PagSeguroTransactionStatus Status { get; set; }
        public string CancellationSource { get; set; }
        public PagSeguroPaymentMethod PaymentMethod { get; set; }
        public string PaymentLink { get; set; }
        public Double GrossAmount { get; set; }
        public Double DiscountAmount { get; set; }
        public Double FeeAmount { get; set; }
        public PagSeguroCreditorFees CreditorFees { get; set; }
        public Double NetAmount { get; set; }
        public Double ExtraAmount { get; set; }
        public DateTime LastEventDate { get; set; }
        public DateTime EscrowEndDate { get; set; }
        public int InstallmentCount { get; set; }
        public int ItemCount { get; set; }
        //public IList<PagSeguroPaymentItem> Items { get; set; } //TODO: This should be uncommented but the serializer is not working with the current json (need to fix it before)
        public PagSeguroSender Sender { get; set; }
        public PagSeguroShipping Shipping { get; set; }
    }

    public class PagSeguroPreApproval
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public string Tracker { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public DateTime LastEventDate { get; set; }
        public string Charge { get; set; }
        public PagSeguroSender Sender { get; set; }
    }


    public class PagSeguroPreApprovalPayment
    {
        public PagSeguroPreApprovalPayment()
        {
            Items = new List<PagSeguroPaymentItem>();
        }
        public List<PagSeguroPaymentItem> Items { get; set; }
        public string Reference { get; set; }
        public string PreApprovalCode { get; set; }
    }

    public class PagSeguroClient
    {
        // always use dot separator for doubles
        private CultureInfo _enUsCulture = CultureInfo.GetCultureInfo("en-US");
        private string Email { get; set; }
        private string Token { get; set; }


        private const string BaseUrl = "https://ws.pagseguro.uol.com.br";
        private const string SandBoxBaseUrl = "https://ws.sandbox.pagseguro.uol.com.br";
        private const string PreApprovalRequestMethod = "/pre-approvals/request";
        private const string PreApprovalReadMethod = "pre-approvals/notifications/";
        private const string PreApprovalPaymentMethod = "pre-approvals/payment";
        private const string TransactionNotificationMethod = "v3/transactions/notifications/";

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
        
        private HttpRequestMessage GetPreparedGetRequest(string url, List<KeyValuePair<string, string>> content)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
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
            content.Add(new KeyValuePair<string, string>("preApprovalAmountPerPayment", preApproval.AmountPerPayment.ToString("0.00", _enUsCulture)));

            if (preApproval.TrialPeriodDuration > 0)
            {
                content.Add(new KeyValuePair<string, string>("preApprovalTrialPeriodDuration",
                    preApproval.TrialPeriodDuration.ToString()));
            }

            if (preApproval.ExpirationValue > 0)
            {
                content.Add(new KeyValuePair<string, string>("preApprovalExpirationValue",
                    preApproval.ExpirationValue.ToString()));
                content.Add(new KeyValuePair<string, string>("preApprovalExpirationUnit",
                    preApproval.ExpirationUnitText));
            }

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

        public async Task<PagSeguroTransaction> ReadTransaction(string transactionCode)
        {
            HttpClient client = GetHttpClient();

            client.BaseAddress = (IsSandBox ? new Uri(SandBoxBaseUrl) : new Uri(BaseUrl));

            var url = TransactionNotificationMethod + transactionCode + $"?email={Email}&token={Token}";

            Console.WriteLine($"Request URL: {client.BaseAddress + url}");
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseContent);
                doc.LoadXml(doc.DocumentElement.OuterXml);
                
                string jsonResponse = JsonConvert.SerializeXmlNode(doc).Replace("{\"transaction\":", "").Replace("}}}", "}}");

                PagSeguroTransaction transaction =
                    JsonConvert.DeserializeObject<PagSeguroTransaction>(jsonResponse);

                return transaction;
            }

            throw new Exception($"Ocorreu um erro ao consultar a transação. Status Resposta: {response.StatusCode}, Resposta: {response.Content.ReadAsStringAsync().Result}");

        }

        public async Task<PagSeguroPreApproval> ReadPreApproval(string preApprovalCode)
        {
            HttpClient client = GetHttpClient();

            client.BaseAddress = (IsSandBox ? new Uri(SandBoxBaseUrl) : new Uri(BaseUrl));

            var content = new List<KeyValuePair<string, string>>();

            var url = PreApprovalReadMethod + preApprovalCode + $"?email={Email}&token={Token}";
            
            Console.WriteLine($"Request URL: {client.BaseAddress + url}");
            
            var response = await client.SendAsync(GetPreparedGetRequest(url, content));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                PagSeguroPreApproval preApproval =
                    JsonConvert.DeserializeObject<PagSeguroPreApproval>(responseContent);
                
                return preApproval;
            }

            throw new Exception($"Ocorreu um erro ao consultar a o plano recorrente. Status Resposta: {response.StatusCode}, Resposta: {response.Content.ReadAsStringAsync().Result}");

        }

        public async Task<PagSeguroPaymentResponse> RequestPreApprovalPayment(PagSeguroPreApprovalPayment preApprovalPaymentRequest)
        {
            HttpClient client = GetHttpClient();

            client.BaseAddress = (IsSandBox ? new Uri(SandBoxBaseUrl) : new Uri(BaseUrl));

            var content = new List<KeyValuePair<string, string>>();

            content.Add(new KeyValuePair<string, string>("reference", preApprovalPaymentRequest.Reference));
            content.Add(new KeyValuePair<string, string>("preApprovalCode", preApprovalPaymentRequest.PreApprovalCode));

            for (var i = 0; i < preApprovalPaymentRequest.Items.Count; i++)
            {
                content.Add(new KeyValuePair<string, string>($"itemId{i+1}", preApprovalPaymentRequest.Items[i].Id));
                content.Add(new KeyValuePair<string, string>($"itemDescription{i+1}", preApprovalPaymentRequest.Items[i].Description));
                content.Add(new KeyValuePair<string, string>($"itemAmount{i+1}", preApprovalPaymentRequest.Items[i].Ammount.ToString("0.00", _enUsCulture)));
                content.Add(new KeyValuePair<string, string>($"itemQuantity{i+1}", preApprovalPaymentRequest.Items[i].Quantity.ToString()));
            }
            
            var url = PreApprovalPaymentMethod + $"?email={Email}&token={Token}";

            var response = await client.SendAsync(GetPreparedPostRequest(url, content));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                PagSeguroPaymentResponse preApprovalPaymentResponse =
                    JsonConvert.DeserializeObject<PagSeguroPaymentResponse>(response.Content
                        .ReadAsStringAsync().Result);
                

                return preApprovalPaymentResponse;
            }

            throw new Exception($"Ocorreu um erro ao solicitar pagamento manual. Status Resposta: {response.StatusCode}, Resposta: {response.Content.ReadAsStringAsync().Result}");

        }

    }
}
