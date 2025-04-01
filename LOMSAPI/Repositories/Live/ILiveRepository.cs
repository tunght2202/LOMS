using LOMSAPI.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LOMSAPI.Repositories.Live
{
    public interface ILiveRepository
    {
        Task<List<LiveStream>> GetLiveStreamsAsync();
        Task<List<LiveStream>> FetchLiveStreamsFromFacebookAsync();
    }
}
