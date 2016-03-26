using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public CharactorController character;
    public Text Score;
    public Text Life;
    public Text DeadScore;


    // Use this for initialization
    void Start() {

    }
	// Update is called once per frame
	public void Update () {

        if(Application.platform == RuntimePlatform.Android) { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.LoadLevel("Title");
            }
        }

        int score = CalcScore();
        Score.text = "Score : " + score + "m";

        int deadscore = CalcScore();
        DeadScore.text = "最終スコア : " + deadscore + "m";

        int life = CalcLife();
        Life.text = "Life Point : " + life;


        if (PlayerPrefs.GetInt("HighScore") < score)
            PlayerPrefs.SetInt("HighScore", score);

        if (character.Life() <= 0)
        {
            enabled = false;
            GameObject.Find("ToMain").GetComponent<UnityEngine.UI.Image>().enabled = true;
            GameObject.Find("ToReStart").GetComponent<UnityEngine.UI.Image>().enabled = true;
            GameObject.Find("DeadScore").GetComponent<UnityEngine.UI.Text>().enabled = true;
        }
    }

    int CalcScore()
    {
        return (int)character.transform.position.z;
    }

    int CalcLife()
    {
        return character.life;
    }

    public void ReturnToTitle()
    {
        Application.LoadLevel("Title");
    }

    public void ReStart()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }
}
