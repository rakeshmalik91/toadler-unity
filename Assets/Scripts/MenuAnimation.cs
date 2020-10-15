using UnityEngine;
using System.Collections;

public class MenuAnimation : MonoBehaviour {

	private Vector2 pos1;
	public Vector2 pos2;

	public float a = 2f;

	// Use this for initialization
	void Start () {
		pos1 = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMenu.paused) {
			if (GetComponent<UnityEngine.UI.Image> () != null) {
				GetComponent<UnityEngine.UI.Image> ().enabled = true;
			} else if (GetComponent<UnityEngine.UI.Text> () != null) {
				GetComponent<UnityEngine.UI.Text> ().enabled = true;
			}
			transform.position = Vector2.Lerp (transform.position, pos1, a * Time.deltaTime);
		} else {
			transform.position = Vector2.Lerp (transform.position, pos2, a * Time.deltaTime);
			if (Vector2.Distance (transform.position, pos2) < 10) {
				if (GetComponent<UnityEngine.UI.Image> () != null) {
					GetComponent<UnityEngine.UI.Image> ().enabled = false;
				} else if (GetComponent<UnityEngine.UI.Text> () != null) {
					GetComponent<UnityEngine.UI.Text> ().enabled = false;
				}
			}
		}
	}
}
