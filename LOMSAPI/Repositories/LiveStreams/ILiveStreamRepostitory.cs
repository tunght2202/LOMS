using LOMSAPI.Data.Entities;

namespace LOMSAPI.Repositories.LiveStreams
{
    public interface ILiveStreamRepostitory
    {
        Task<IEnumerable<LiveStream>> GetAllLiveStreamsFromFacebook(string userId); // Lấy từ Facebook API
        Task<LiveStream> GetLiveStreamById(string liveStreamId, string userid);
        Task<int> DeleteLiveStream(string liveStreamId);
        Task<bool> IsLiveStreamStillLive(string liveStreamId);

    }
}