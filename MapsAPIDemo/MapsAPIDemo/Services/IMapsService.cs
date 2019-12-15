using System.Threading.Tasks;
using MapsAPIDemo.Models;

namespace MapsAPIDemo.Services
{
    public interface IMapsService
    {
        Task<SearchResultModel> GetTextSearch(SearchModel model);
    }
}