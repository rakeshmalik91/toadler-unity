using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour {

	public float fadeOutTime = 1f;
	public float waitTime = 1f;
	public float fadeInTime = 1f;

	public string sceneToLoadOnFadeOut;

	private int state = 0;
	private float waitStartTime = 0;

	// Use this for initialization
	void Awake () {
		GetComponent<CanvasGroup> ().alpha = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case 0:
			GetComponent<CanvasGroup> ().alpha -= Time.deltaTime / fadeOutTime;
			if (GetComponent<CanvasGroup> ().alpha <= 0f) {
				GetComponent<CanvasGroup> ().alpha = 0f;
				if (waitTime >= 0) {
					state = 1;
					waitStartTime = Time.time;
				} else {
					GameObject.Destroy (gameObject);
				}
			}
			break;
		case 1:
			if (Time.time - waitStartTime >= waitTime) {
				state = 2;
			}
			break;
		case 2:
			GetComponent<CanvasGroup> ().alpha += Time.deltaTime / fadeInTime;
			if (GetComponent<CanvasGroup> ().alpha >= 1f) {
				GetComponent<CanvasGroup> ().alpha = 1f;
				SceneManager.LoadScene (sceneToLoadOnFadeOut);
			}
			break;
		}
	}
}
