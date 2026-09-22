using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<int> OnScoreChanged;

    public int Score { get; private set; }

    public void AddScore(int amount)
    {
        Score += amount;
        Debug.Log($"[Score] {(amount >= 0 ? "+" : "")}{amount} -> {Score}");
        OnScoreChanged?.Invoke(Score);
    }

    public void ResetScore()
    {
        Score = 0;
        OnScoreChanged?.Invoke(Score);
    }
}
