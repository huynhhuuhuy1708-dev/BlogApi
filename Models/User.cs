using System.ComponentModel.DataAnnotations;

namespace BlogAI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";

        public List<BlogPost> BlogPosts { get; set; }
    }
}
