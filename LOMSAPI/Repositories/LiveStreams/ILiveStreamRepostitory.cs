using LOMSAPI.Data.Entities;

namespace LOMSAPI.Repositories.LiveStreams
{
    public interface ILiveStreamRepostitory
    {
        Task<IEnumerable<LiveStream>> GetAllLiveStreamsFromDb(string userId); // Lấy từ DB
        Task<IEnumerable<LiveStream>> GetAllLiveStreamsFromFacebook(string userId); // Lấy từ Facebook API
        Task<LiveStream> GetLiveStreamById(string liveStreamId, string userid);
        Task<int> DeleteLiveStream(string liveStreamId);
    }
}