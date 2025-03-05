namespace RejtelyekHaza.DTOs
{
    public class LoggedInUserDTO
    {
        public string Token { get; set; } = null!;
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public int Permission { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
