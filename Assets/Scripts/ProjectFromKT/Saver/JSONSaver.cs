using System.IO;
using UnityEngine;

public class JsonSaver : ISaver
{
    [System.Serializable]
    private class ScoreData
    {
        public int score;
    }

    public void SaveScore(int score, string path = null)
    {
        if (string.IsNullOrEmpty(path))
        {
            path = Path.Combine(Application.persistentDataPath, "score.json");
        }

        var scoreData = new ScoreData { score = score };
        string json = JsonUtility.ToJson(scoreData);
        File.WriteAllText(path, json);
        
        Debug.Log($"Score saved - JSON: {score}: {path}");
    }
}