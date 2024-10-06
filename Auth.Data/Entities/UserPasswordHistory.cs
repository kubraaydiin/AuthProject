namespace Auth.Data.Entities
{
    public class UserPasswordHistory : BaseEntity
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
