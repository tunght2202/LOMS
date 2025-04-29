using LOMSAPI.Models;
namespace LOMSAPI.Repositories.Comments
{
    public interface ICommentRepository
    {
        Task<List<CommentModel>> GetAllComments(string LiveStreamId, string token);
    }
}