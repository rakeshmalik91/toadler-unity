using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {

	private UnityEngine.UI.Text textbox;

	public static int highScore;

	// Use this for initialization
	void Start () {
		highScore = PlayerPrefs.GetInt("highScore");
		textbox = GetComponent<UnityEngine.UI.Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Score.score > highScore) {
			highScore = Score.score;
		}
		textbox.text = highScore.ToString ();
	}
}
