using Xunit;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using RateGetters.Rates.Models;
using RateGetters.Rates.Models.Enums;

namespace WebAPI.Tests.Rates
{
    public class RateTests
    {
        private readonly HttpClient _client;

        public RateTests() =>
            _client = new TestServer(
                    new WebHostBuilder()
                        .UseEnvironment("Development")
                        .UseStartup<Startup>())
                .CreateClient();

        [Fact]
        public async Task TestConvertCurrency()
        {
            var fromRate = JsonConvert.DeserializeObject<RateForDate>(
                await GetContentFromResponse(
                    await SendRequest(
                        HttpMethod.Get,
                        $"https://localhost:44322/rates/date" +
                        $"?Code={CurrencyCodesEnum.Rub}" +
                        $"&DateTime={DateTime.Today:yyyy.MM.dd}")));
            var toRate = JsonConvert.DeserializeObject<RateForDate>(
                await GetContentFromResponse(
                    await SendRequest(
                        HttpMethod.Get,
                        $"https://localhost:44322/rates/date" +
                        $"?Code={CurrencyCodesEnum.Eur}" +
                        $"&DateTime={DateTime.Today:yyyy.MM.dd}")));
            Assert.NotNull(fromRate);
            Assert.NotNull(toRate);
            var expected = 115000 * fromRate.Value / toRate.Value;
            
            var actual = JsonConvert.DeserializeObject<decimal>(
                await GetContentFromResponse(
                    await SendRequest(
                        HttpMethod.Get,
                        $"https://localhost:44322/rates/convert?From=Rub&To=Usd&BaseValue=115000")));
            
            Assert.Equal(expected, actual);
        }
        [Fact]
        public async Task TestGetForDate()
        {
            var expected = new RateForDate(
                CurrencyCodesEnum.Eur,
                86.561m,
                new DateTime(2022, 02, 04));

            var actual = JsonConvert.DeserializeObject<RateForDate>(
                await GetContentFromResponse(
                    await SendRequest(
                        HttpMethod.Get,
                        $"https://localhost:44322/rates/date" +
                        $"?Code={CurrencyCodesEnum.Eur}" +
                        $"&DateTime={new DateTime(2022, 02, 04):yyyy.MM.dd}")));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task TestGetForPeriod()
        {
            var expected = new PeriodRateList(
                new RateForDate[]
                {
                    new(
                        CurrencyCodesEnum.Usd,
                        74.8926m,
                        new DateTime(2021, 12, 01)),
                    new(
                        CurrencyCodesEnum.Usd,
                        73.9746m,
                        new DateTime(2021, 12, 02)),
                    new(
                        CurrencyCodesEnum.Usd,
                        74.0637m,
                        new DateTime(2021, 12, 03)),
                    new(
                        CurrencyCodesEnum.Usd,
                        73.7426m,
                        new DateTime(2021, 12, 04))
                });

            var actual = JsonConvert.DeserializeObject<PeriodRateList>(
                await GetContentFromResponse(
                    await SendRequest(
                        HttpMethod.Get,
                        $"https://localhost:44322/rates/period" +
                        $"?Code={CurrencyCodesEnum.Usd}" +
                        $"&FirstDate={new DateTime(2021, 12, 01):yyyy.MM.dd}" +
                        $"&SecondDate={new DateTime(2021, 12, 06):yyyy.MM.dd}")));

            Assert.Equal(expected, actual);
        }

        private async Task<HttpResponseMessage> SendRequest(HttpMethod method, string requestUri)
        {
            using var request = new HttpRequestMessage(method, requestUri);
            using var response = _client.SendAsync(request);
            return await response.ConfigureAwait(false);
        }

        private static async Task<string> GetContentFromResponse(HttpResponseMessage responseMessage) =>
            await responseMessage
                .Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);
    }
}