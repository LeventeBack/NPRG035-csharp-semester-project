namespace QuizEdu.Src.Models;

/// <summary>
/// This class represents a quiz combination in the database.
/// It contains the id, parent id, and child id.
/// </summary>
public class QuizCombination
{
    public int Id { get; set; }
    public int ParentId { get; set; } = 0;
    public int ChildId { get; set; } = 0;
}