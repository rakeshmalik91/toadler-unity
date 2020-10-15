using UnityEngine;
using System.Collections;

public class SpriteSwap : MonoBehaviour {

	private bool changed = false;
	private Sprite oldSprite;
	public Sprite newSprite;

	void Awake() {
		oldSprite = GetComponent<UnityEngine.UI.Image> ().sprite;
	}

	public void ChangeSprite () {
		if (changed) {
			GetComponent<UnityEngine.UI.Image> ().sprite = oldSprite;
		} else {
			GetComponent<UnityEngine.UI.Image> ().sprite = newSprite;
		}
		changed = !changed;
	}
}
