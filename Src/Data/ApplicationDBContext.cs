
using Microsoft.EntityFrameworkCore;
using QuizEdu.Src.Models;

/// <summary>
/// ApplicationDBContext class is used to create the database context for the application.
/// It is used to create the database tables and their relationships.
/// </summary>
public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
    }

    public DbSet<Question> Questions { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<QuizCombination> QuizCombinations { get; set; }
    public DbSet<GameResult> GameResults { get; set; }
}