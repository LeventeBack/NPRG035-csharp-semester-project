namespace QuizEdu.Src.Models;

public class QuizCombination
{
    public int Id { get; set; }
    public int ParentId { get; set; } = 0;
    public int ChildId { get; set; } = 0;
}