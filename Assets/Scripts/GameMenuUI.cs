using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class GameMenuUI : MonoBehaviour
{
    public Button start;
    public static string nick_name;

    void Start() {
        start.onClick.AddListener(ProcessButton);
    } 

    void ProcessButton() {
        SceneManager.LoadScene("CannonGame");
    }

    public void ReadStringInput(string str) {
        nick_name = str;
        PlayerPrefs.SetString("saveName", str);
        Debug.Log(PlayerPrefs.GetString("saveName"));
    }
}
