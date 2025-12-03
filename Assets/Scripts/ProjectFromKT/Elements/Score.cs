public class Score
{
    private int _currentScore;

    public int CurrentScore => _currentScore;

    public void AddScore(int amount)
    {
        _currentScore += amount;
    }

    public void LoadScore(int score)
    {
        _currentScore = score;
    }
}