using BlogAI.Models;
using System.Security.Cryptography;
using System.Text;

namespace BlogAI.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var admin = new User
                {
                    Username = "admin",
                    PasswordHash = ComputeHash("admin123"),
                    Role = "Admin"
                };

                var user1 = new User
                {
                    Username = "user1",
                    PasswordHash = ComputeHash("user123"),
                    Role = "User"
                };

                var user2 = new User
                {
                    Username = "user2",
                    PasswordHash = ComputeHash("user123"),
                    Role = "User"
                };

                context.Users.AddRange(admin, user1, user2);
                context.SaveChanges();

                context.BlogPosts.AddRange(
                    new BlogPost { Title = "Welcome to BlogAI", Content = "This is the first blog post", UserId = admin.Id },
                    new BlogPost { Title = "User1 Post", Content = "Hello from user1", UserId = user1.Id },
                    new BlogPost { Title = "User2 Post", Content = "Hello from user2", UserId = user2.Id }
                );

                context.SaveChanges();
            }
        }

        private static string ComputeHash(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
