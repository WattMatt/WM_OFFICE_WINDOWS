namespace WMOffice.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; } // "active", "completed", "archived"

        public Project(string id, string name, string status)
        {
            Id = id;
            Name = name;
            Status = status;
        }
    }
}
