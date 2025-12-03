using UnityEngine;

public class PlayerPrefsSaver : ISaver
{
    public void SaveScore(int score, string path = null)
    {
        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.Save();
        Debug.Log($"Score saved - PlayerPrefs: {score}");
    }
}