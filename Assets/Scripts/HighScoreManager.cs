using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;
    public bool GameIsWon { get; set; }

    private List<HighScore> highScores = new List<HighScore>(); // List to store high scores
    string fileName = "HighScores.txt"; // Bestandsnaam waarin de highscores worden opgeslagen

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
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (HighScore score in highScores)
            {
                writer.WriteLine(score.PlayerName + "," + score.Score);
            }
        }
    }

    // Function to load high scores from file
    public void LoadHighScores()
    {
        highScores.Clear(); // Clear existing high scores before loading
        if (File.Exists(fileName))
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        string playerName = parts[0];
                        int score;
                        if (int.TryParse(parts[1], out score))
                        {
                            highScores.Add(new HighScore(playerName, score));
                        }
                    }
                }
            }
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

