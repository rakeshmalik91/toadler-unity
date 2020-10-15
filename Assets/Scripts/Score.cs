using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Score : MonoBehaviour {

	private UnityEngine.UI.Text textbox;
	private GameObject player;

	private Vector2 initPlayerPos;

	public static int score = 0;

	public static long score200Duration = 0;
	public static long score500Duration = 0;

	// Use this for initialization
	void Start () {
		score = 0;
		textbox = GetComponent<UnityEngine.UI.Text> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		initPlayerPos = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		int lastScore = score;
		score = (int) Mathf.Max(score, player.transform.position.y - initPlayerPos.y);
		textbox.text = score.ToString ();

		if (lastScore < 200 && score >= 200) {
			float duration = Time.time - GameMenu.startTime;
			score200Duration = (long) (duration * 1000);
			Debug.Log ("200 score time: " + duration);
		} else if (lastScore < 500 && score >= 500) {
			float duration = Time.time - GameMenu.startTime;
			score500Duration = (long) (duration * 1000);
			Debug.Log ("500 score time: " + duration);
		}
	}
}
