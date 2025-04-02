using LOMSAPI.Data.Entities;

namespace LOMSAPI.Repositories.LiveStreams
{
    public interface ILiveStreamRepostitory
    {
        Task<IEnumerable<LiveStream>> GetAllLiveStreams(string userId);
        Task<LiveStream> GetLiveStreamById(string liveStreamId, string userid);
        Task<int> DeleteLiveStream(string liveStreamId);
    }
}
