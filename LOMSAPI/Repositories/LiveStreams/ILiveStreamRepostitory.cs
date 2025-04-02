using LOMSAPI.Data.Entities;

namespace LOMSAPI.Repositories.LiveStreams
{
    public interface ILiveStreamRepostitory
    {
        Task<IEnumerable<LiveStream>> GetAllLiveStreams(string pageId);
        Task<LiveStream> GetLiveStreamById(string liveStreamId);
        Task<int> DeleteLiveStream(string liveStreamId);
    }
}
