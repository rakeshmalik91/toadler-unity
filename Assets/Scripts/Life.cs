using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

	private UnityEngine.UI.Text textbox;

	public static int life;
	public static int lifeUsedInCurrentGame = 0;
	public static int lifeCollectedInCurrentGame = 0;

	// Use this for initialization
	void Start () {
		life = PlayerPrefs.GetInt("life");
		textbox = GetComponent<UnityEngine.UI.Text> ();
	}

	// Update is called once per frame
	void Update () {
		textbox.text = life.ToString ();
	}
}
