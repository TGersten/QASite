namespace QASite.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Answer> Answers { get; set; } = new List<Answer>();

        
    }
}
