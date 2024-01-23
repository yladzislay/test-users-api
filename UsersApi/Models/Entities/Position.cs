namespace UsersApi.Models.Entities
{
    public class Position
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal DefaultSalary { get; set; }
    }
}