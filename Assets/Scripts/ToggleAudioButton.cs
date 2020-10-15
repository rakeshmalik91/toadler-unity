using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class ToggleAudioButton : MonoBehaviour {

	void Start() {
		if(PlayerPrefs.HasKey("audio")) {
			AudioListener.volume = PlayerPrefs.GetFloat ("audio");
		} else {
			AudioListener.volume = 1f;
		}
		if (AudioListener.volume <= 0f) {
			GetComponent<SpriteSwap> ().ChangeSprite ();
		}
	}

	public void OnClick () {
		if (AudioListener.volume <= 0f) {
			AudioListener.volume = 1f;
		} else {
			AudioListener.volume = 0f;
		}
		PlayerPrefs.SetFloat ("audio", AudioListener.volume);
	}
}
