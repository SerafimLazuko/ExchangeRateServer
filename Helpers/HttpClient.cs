namespace ExchangeRateServer.Helpers
{
    public class HttpClient : IHttpClient
    {
        static System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

        public string Get(string url)
        {
            var result = string.Empty;

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                var responce = httpClient.Send(request);

                result = responce.Content.ReadAsStringAsync().Result;
            } 
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
            
            return result;
        }
    }
}
