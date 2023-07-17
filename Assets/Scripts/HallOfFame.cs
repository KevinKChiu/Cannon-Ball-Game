using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used tutorial from YouTube video: https://www.youtube.com/watch?v=iAbaqGYdnyI
public class HallOfFame : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    private Highscores highscores;

    private void Awake() {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");

        string name = PlayerPrefs.GetString("saveName");
        string score = PlayerPrefs.GetString("finalScore");

        entryTemplate.gameObject.SetActive(false);

        if (PlayerPrefs.GetString("highscoreTable") != "") {
            AddHighscoreEntry(int.Parse(score), name);
            string jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        } else {
            List<HighscoreEntry> highscoreEntryList = new List<HighscoreEntry>() {
                new HighscoreEntry{ score = 0, name = "Empty" },
                new HighscoreEntry{ score = 0, name = "Empty" },
                new HighscoreEntry{ score = 0, name = "Empty" },
                new HighscoreEntry{ score = 0, name = "Empty" },
                new HighscoreEntry{ score = 0, name = "Empty" },
            };
            highscores = new Highscores { highscoreEntryList = highscoreEntryList };
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
        }

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
                    HighscoreEntry temp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = temp;
                }
            }
        }
        highscoreEntryTransformList = new List<Transform>();
        int count = 0;
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
            if (count < 5) { 
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
            } else {
                break;
            }
            count += 1;
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 40f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
            default: 
                rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break; 
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        int score = highscoreEntry.score;
        entryTransform.GetComponentsInChildren<Text>()[0].text = rankString;
        entryTransform.GetComponentsInChildren<Text>()[1].text = score.ToString();
        string name = highscoreEntry.name;
        entryTransform.GetComponentsInChildren<Text>()[2].text = name;
        transformList.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name) {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscores.highscoreEntryList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry {
        public int score;
        public string name;
    }
}
