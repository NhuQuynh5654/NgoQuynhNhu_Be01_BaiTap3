using System.Text.Json.Serialization;

namespace BaitapBackend3.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        [JsonIgnore]
        public ICollection<User> Users { get; set; } = new List<User>(); // ✅ Thêm mặc định
        [JsonIgnore]
        public ICollection<AllowAccess> AllowAccesses { get; set; } = new List<AllowAccess>();

    }
}
