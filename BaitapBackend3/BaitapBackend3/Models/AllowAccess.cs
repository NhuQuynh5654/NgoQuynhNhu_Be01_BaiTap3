using System.Text.Json.Serialization;

namespace BaitapBackend3.Models
{
    public class AllowAccess
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string TableName { get; set; }
        public string AccessProperties { get; set; }
        [JsonIgnore]
        public Role? Role { get; set; }
    }
}
