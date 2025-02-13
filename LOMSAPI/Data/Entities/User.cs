using Microsoft.AspNetCore.Identity;

namespace LOMSAPI.Data.Entities
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<LiveStream> LiveStreams { get; set; }
    }
}
