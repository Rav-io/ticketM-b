namespace ticketmanager.DTO
{
    public class ProjectUserDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<int> UserIds { get; set; }
    }
}