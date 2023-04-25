using Domain;
using Services.Contracts;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class LookupServiceProvider : ICreditDataApiProvider
    {
        private readonly IHttpClientFactory factory;
        private readonly JsonSerializerOptions _options;

        public LookupServiceProvider(IHttpClientFactory factory)
        {
            this.factory = factory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<CreditData> GetSsnCreditDetails(string param)
        {
            var personalDetailsTask = GetPersonalDetails(param);
            var assessedIncomeTask = GetAssessedIncome(param);
            var debtTask = GetDebt(param);

            await Task.WhenAll(personalDetailsTask, assessedIncomeTask, debtTask);


            var personalDetailsResult = personalDetailsTask.Result;
            var assessedIncomeResult = assessedIncomeTask.Result;
            var debtResult = debtTask.Result;

            var creditData = new CreditData
            {
                First_name = personalDetailsResult.FirstName,
                Last_name = personalDetailsResult.Lastname,
                Address = personalDetailsResult.Address,
                Assessed_Income = assessedIncomeResult.Assessed_Income,
                Balance_Of_Debt = debtResult.Balance_Of_Debt,
                Complaints = debtResult.Complaints
            };

            return creditData;
        }

        private async Task<PersonalDetails> GetPersonalDetails(string ssn)
        {
            var httpClient = factory.CreateClient("credit-data");

            using (var response = await httpClient.GetAsync("personal-details/{ssn}"))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var personalDetails = await JsonSerializer.DeserializeAsync<PersonalDetails>(stream, _options);

                return personalDetails;
            }
        }

        private async Task<Income> GetAssessedIncome(string ssn)
        {
            var httpClient = factory.CreateClient("credit-data");

            using (var response = await httpClient.GetAsync("assessed-income/{ssn}"))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var income = await JsonSerializer.DeserializeAsync<Income>(stream, _options);

                return income;
            }
        }

        private async Task<Debt> GetDebt(string ssn)
        {
            var httpClient = factory.CreateClient("credit-data");

            using (var response = await httpClient.GetAsync("debt/{ssn}"))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();
                var debt = await JsonSerializer.DeserializeAsync<Debt>(stream, _options);

                return debt;
            }
        }
    }
}
