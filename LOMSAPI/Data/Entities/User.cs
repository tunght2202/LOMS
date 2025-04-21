using Microsoft.AspNetCore.Identity;

namespace LOMSAPI.Data.Entities
{
    public class User : IdentityUser
    {
        
        public DateTime CreatedAt { get; set; }
        public string? ImageURL { get; set; }
        public string? FullName { get; set; }
        public string? Sex { get; set; }
        public string? Address { get; set; }
        public string? TokenFacbook { get; set; }
        public string? PageId { get; set; }
        public virtual ICollection<LiveStream> LiveStreams { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
