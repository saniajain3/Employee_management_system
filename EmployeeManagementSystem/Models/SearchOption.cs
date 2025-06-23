using System.Text.Json.Serialization;

namespace EmployeeManagementSystem.Models
{
    public class SearchOptions
    {
        public string? Search { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; } = 10;

    }
    public class PagedData<T>
    {
        [JsonPropertyName("data")]
        public int TotalData { get; set; }
        [JsonPropertyName("totalData")]
        public List<T> Data { get; set; } = new List<T>();
    }
}
