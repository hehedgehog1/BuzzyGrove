using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts;

    public void DisplayHighScores()
    {
        List<ScoreEntry> scoreEntries = new List<ScoreEntry>();
        int maxDays = PlayerPrefs.GetInt("Day", 0); // Get the current day

        // Collect all the scores for each day
        for (int day = 1; day <= maxDays; day++)
        {
            int score = PlayerPrefs.GetInt("ScoreDay" + day, 0);
            scoreEntries.Add(new ScoreEntry(day, score));
        }

        // Sort the list by score in descending order
        scoreEntries.Sort((a, b) => b.score.CompareTo(a.score));

        // Display the top 5 scores
        for (int i = 0; i < Mathf.Min(5, scoreEntries.Count); i++)
        {
            scoreTexts[i].text = "Day " + scoreEntries[i].day + ": " + scoreEntries[i].score;
        }
    }

    public class ScoreEntry
    {
        public int day;
        public int score;

        //score the day and honeycount together
        public ScoreEntry(int day, int score)
        {
            this.day = day;
            this.score = score;
        }
    }

}
