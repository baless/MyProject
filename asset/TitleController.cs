using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    public Text highScoreLabel;

    public void Start()
    {
        Screen.SetResolution(1280,800,true);
        highScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "m";
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public void OnStartButtonClicked()
    {
        Application.LoadLevel("Main");
    }
}
