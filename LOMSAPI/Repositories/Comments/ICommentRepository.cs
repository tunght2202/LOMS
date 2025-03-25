using LOMSAPI.Data.Entities;
namespace LOMSAPI.Repositories.Comments
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllComments(string LiveStreamURL);
        Task<List<Comment>> GetCommentsByProductCode(string LiveStreamURL, string ProductCode);
    }
}