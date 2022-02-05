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
        public async Task TestGetForDate() =>
            Assert.Equal(
                new RateForDate(
                    new Rate(CurrencyCodesEnum.Eur, 86.561m),
                    new DateTime(2022, 02, 04)),
                JsonConvert.DeserializeObject<RateForDate>(
                    await (await _client.SendAsync(
                            new HttpRequestMessage(
                                new HttpMethod("GET"),
                                $"https://localhost:44322/rates/date" +
                                $"?Code={CurrencyCodesEnum.Eur}" +
                                $"&DateTime={new DateTime(2022, 02, 04):yyyy.MM.dd}"))
                            .ConfigureAwait(false))
                        .Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false)));

        [Fact]
        public async Task TestGetForPeriod() =>
            Assert.Equal(
                new PeriodRateList(
                    new RateForDate[]
                    {
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 74.8926m),
                            new DateTime(2021, 12, 01)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 73.9746m),
                            new DateTime(2021, 12, 02)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 74.0637m),
                            new DateTime(2021, 12, 03)),
                        new(
                            new Rate(CurrencyCodesEnum.Usd, 73.7426m),
                            new DateTime(2021, 12, 04))
                    }),
                JsonConvert.DeserializeObject<PeriodRateList>(
                    await (await _client.SendAsync(
                                new HttpRequestMessage(
                                    new HttpMethod("GET"),
                                    $"https://localhost:44322/rates/period" +
                                    $"?Code={CurrencyCodesEnum.Usd}" +
                                    $"&FirstDate={new DateTime(2021, 12, 01):yyyy.MM.dd}" +
                                    $"&SecondDate={new DateTime(2021, 12, 06):yyyy.MM.dd}"))
                            .ConfigureAwait(false))
                        .Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false)));
    }
}