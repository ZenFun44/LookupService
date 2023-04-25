using Domain;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ICreditDataApiProvider
    {
        Task<CreditData> GetSsnCreditDetails(string ssn);
    }
}
