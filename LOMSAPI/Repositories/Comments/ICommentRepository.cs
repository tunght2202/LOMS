using LOMSAPI.Data.Entities;
namespace LOMSAPI.Repositories.Comments
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllComments(string LiveStreamId);
        Task<List<Comment>> GetCommentsByProductCode(string ProductCode);
    }
}