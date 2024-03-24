using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;
    public bool GameIsWon { get; set; }

    private List<HighScore> highScores = new List<HighScore>(); // List to store high scores

    // Awake function to implement singleton pattern
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to add a new high score
    public void AddHighScore(string playerName, int score)
    {
        highScores.Add(new HighScore(playerName, score));
        highScores = highScores.OrderByDescending(hs => hs.Score).ToList(); // Sort high scores in descending order
        if (highScores.Count > 5)
        {
            highScores.RemoveAt(highScores.Count - 1); // Remove lowest score if more than 5 high scores
        }
        SaveHighScores(); // Save high scores to file
    }

    // Function to save high scores to file
    private void SaveHighScores()
    {
        string json = JsonUtility.ToJson(highScores); // Serialize high scores to JSON
        PlayerPrefs.SetString("HighScores", json); // Save JSON to player preferences
        PlayerPrefs.Save(); // Save player preferences
    }

    // Function to load high scores from file
    public void LoadHighScores()
    {
        string json = PlayerPrefs.GetString("HighScores", ""); // Load JSON from player preferences
        if (!string.IsNullOrEmpty(json))
        {
            highScores = JsonUtility.FromJson<List<HighScore>>(json); // Deserialize JSON to high scores
        }
    }

    // Function to get current high scores
    public List<HighScore> GetHighScores()
    {
        return highScores;
    }
}
[System.Serializable]
public class HighScore
{
    public string PlayerName;
    public int Score;

    public HighScore(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}

