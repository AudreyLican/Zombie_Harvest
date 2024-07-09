using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;

    string newGameScene = "Neptunia_Island";

    public AudioClip bg_music;
    public AudioSource menu_channel;

    void Start()
    {
        menu_channel.PlayOneShot(bg_music);

        // Set the high score text
        int highscore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highscore}";
    }

    public void StartNewGame()
    {
        menu_channel.Stop();

        SceneManager.LoadScene(newGameScene);
    }
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
