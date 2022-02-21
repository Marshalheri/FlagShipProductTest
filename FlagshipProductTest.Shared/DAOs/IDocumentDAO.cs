using FlagshipProductTest.Shared.Models;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DAOs
{
    public interface IDocumentDAO : IBaseConnection
    {
        Task<long> Add(Document document);
    }
}
