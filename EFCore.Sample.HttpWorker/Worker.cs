using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly.CircuitBreaker;

namespace EFCore.Sample.CircuitBreaker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly AsyncCircuitBreakerPolicy _circuitBreaker;

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration,
            AsyncCircuitBreakerPolicy circuitBreaker)
        {
            _logger = logger;
            _configuration = configuration;
            _circuitBreaker  = circuitBreaker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    object resultado = await ObterClientes();

                    _logger.LogInformation($"* {DateTime.Now:HH:mm:ss} * " +
                        $"Circuito = {_circuitBreaker.CircuitState}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"# {DateTime.Now:HH:mm:ss} # "+
                        $"Circuito = {_circuitBreaker.CircuitState} | " +
                        $"Falha ao invocar a API: {ex.GetType().FullName} | {ex.Message}");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task<object> ObterClientes()
        {
            var httpClient = new HttpClient();
            var urlApiContagem = _configuration["UrlApiContagem"];

            return await _circuitBreaker.ExecuteAsync<string>(() =>
            {
                return ObterListaClienteSafe2PayPorNome();
            });
        }

        private async Task<string> ObterListaClienteSafe2PayPorNome()
        {
            int codContaReceber = 987055;
            string cnpjCpf = "03264853000129";
            decimal valor = 47.50M;

            string url = $"https://api.safe2pay.com.br/v2/Transaction/List?PageNumber=1&RowsPerPage=100&Object.Customer.Identity={cnpjCpf}";

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("X-API-KEY", "053C5D4390184275BE840BCAC065240C");
            using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    return streamReader.ReadToEnd();
                }
            }

        }
    }
}