namespace ChatApi.Models
{
    public class Chat
    {
        public Chat()
        {
            Id=Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ToUserId { get; set; }
        public string Öessage { get; set; }=string.Empty;
        public DateTime Date { get; set; }
    }
}
