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
        public async Task TestGetForDate()
        {
            var date = new DateTime(2022, 02, 04);

            var request = new HttpRequestMessage(
                new HttpMethod("GET"),
                $"https://localhost:44322/rates/date" +
                $"?Code={CurrencyCodesEnum.Eur}" +
                $"&DateTime={date:yyyy.MM.dd}");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<RateForDate>(await response.Content.ReadAsStringAsync());

            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.Eur, 86.561m),
                    date),
                result);
        }

        [Fact]
        public async Task TestGetForPeriod()
        {
            var first = new DateTime(2021, 12, 06);
            var second = new DateTime(2021, 12, 01);
            var expectedResult = new PeriodRateList(
                new RateForDate[]
                {
                    new(
                        new Rate(CurrencyCodesEnum.Usd, 74.8926m),
                        second),
                    new(
                        new Rate(CurrencyCodesEnum.Usd, 73.9746m),
                        second.AddDays(1)),
                    new(
                        new Rate(CurrencyCodesEnum.Usd, 74.0637m),
                        second.AddDays(2)),
                    new(
                        new Rate(CurrencyCodesEnum.Usd, 73.7426m),
                        second.AddDays(3))
                });

            var request = new HttpRequestMessage(
                new HttpMethod("GET"),
                $"https://localhost:44322/rates/period" +
                $"?Code={CurrencyCodesEnum.Usd}" +
                $"&FirstDate={first:yyyy.MM.dd}" +
                $"&SecondDate={second:yyyy.MM.dd}");
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<PeriodRateList>(await response.Content.ReadAsStringAsync());

            Assert.Equal(expectedResult, result);
        }
    }
}