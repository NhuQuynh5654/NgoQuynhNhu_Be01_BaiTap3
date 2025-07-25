using System.Text.Json.Serialization;

namespace BaitapBackend3.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public Role? Role { get; set; }
    }
}
