using UnityEngine;
using System.Collections;

public class LilypadGenerator : MonoBehaviour {

	public GameObject[] dummyJumpables;
	private static  GameObject[] dummyJumpablesS;
	public GameObject[] dummyDecor;
	private static GameObject[] dummyDecorS;
	public GameObject[] dummyCollectible;
	private static GameObject[] dummyCollectibleS;

	private float seed = 0;
	private ArrayList lilyPads = new ArrayList();
	private static GameObject player;

	private class Lilypad {
		public Vector2 position;
		public float scale;
		public float rotation;
		public bool visible;
		private GameObject instance;
		private int objectIndex;

		public Lilypad(Vector2 position, float scale, float rotation) {
			this.position = position;
			this.scale = scale;
			this.rotation = rotation;
			this.visible = false;
			this.objectIndex = (int) Random.Range(0f, (float) dummyJumpablesS.Length);
		}

		public void Instantiate(Transform parent) {
			if (this.visible) {
				return;
			}

			//Lilypad, Ground, Lily
			instance = GameObject.Instantiate (dummyJumpablesS[objectIndex]);
			instance.transform.position = this.position;
			instance.transform.localScale = new Vector3(1, 1, 1) * this.scale;
			instance.transform.eulerAngles = new Vector3(0, 0, this.rotation);
			instance.transform.parent = parent.transform;
			this.visible = true;

			//Bee
			if (Random.Range (0f, 1f) > 0.93f) {
				GameObject decor = GameObject.Instantiate (dummyDecorS [0]);
				decor.transform.position = new Vector3 (
					instance.transform.position.x + Random.Range (-5f, 5f), 
					instance.transform.position.y + Random.Range (-5f, 5f), 
					decor.transform.position.z
				);
				decor.transform.localScale = new Vector3 (Random.Range (0f, 1f) > 0.5f ? 1f : -1f, 1f, 1f);
				decor.transform.parent = instance.transform;
			}

			//Life
			if (Random.Range (0f, 1f) > 0.96f) {
				GameObject decor = GameObject.Instantiate (dummyCollectibleS [0]);
				decor.transform.position = new Vector3 (
					instance.transform.position.x,
					instance.transform.position.y,
					decor.transform.position.z
				);
				decor.transform.parent = instance.transform;
			}
		}

		public void Destroy() {
			if (!this.visible) {
				return;
			}
			GameObject.Destroy(instance);
			this.visible = false;
		}

		public bool IsInScreen() {
			return Vector2.Distance (player.transform.position, this.position) <= 20;
		}
	}

	// Use this for initialization
	void Start () {
		dummyJumpablesS = dummyJumpables;
		dummyDecorS = dummyDecor;
		dummyCollectibleS = dummyCollectible;

		player = GameObject.FindGameObjectWithTag ("Player");

		seed = Random.Range (0f, 1f);
		lilyPads.Add (new Lilypad(new Vector2(0f, 2f), 1f, 0f));

		GenerateNewLilypads ();
		HandleLilypadsInstances ();
	}
	
	// Update is called once per frame
	void Update () {
		GenerateNewLilypads ();
		HandleLilypadsInstances ();
		//Debug.Log ("Total lilypads: " + lilyPads.Count + ", Visible lilypads: " + (transform.childCount-1));
	}
		
	void GenerateNewLilypads() {
		if (((Lilypad)lilyPads [lilyPads.Count - 1]).visible) {
			GenerateNewLilypad ();
		}
	}

	void GenerateNewLilypad() {
		Lilypad lastLilyPad = (Lilypad) lilyPads[lilyPads.Count - 1];

		Vector2 newPosition;
		if (Score.score < 20) {
			newPosition = lastLilyPad.position + new Vector2 (Random.Range (-2f, 2f), Random.Range (1f, 2f));
		} else if (Score.score < 50) {
			newPosition = lastLilyPad.position + new Vector2 (Random.Range (-2f, 2f), Random.Range (1f, 3f));
		} else if (Score.score < 100) {
			newPosition = lastLilyPad.position + new Vector2 (Random.Range (-2.5f, 2.5f), Random.Range (1f, 4f));
		} else if (Score.score < 200) {
			newPosition = lastLilyPad.position + new Vector2 (Random.Range (-3f, 3f), Random.Range (1f, 3f));
		} else if (Score.score < 300) {
			newPosition = lastLilyPad.position + new Vector2 (Random.Range (-2.5f, 2.5f), Random.Range (1f, 3f));
		} else if (Score.score < 500) {
			newPosition = lastLilyPad.position + new Vector2 (Random.Range (-2f, 2f), Random.Range (1f, 2.5f));
		} else {
			newPosition = lastLilyPad.position + new Vector2 (Random.Range (-3f, 3f), Random.Range (1f, 3f));
		}

		float newScale;
		if (Score.score < 10) {
			newScale = Random.Range (0.6f, 1.2f);
		} else if (Score.score < 50) {
			newScale = Random.Range (0.6f, 1f);
		} else if (Score.score < 100) {
			newScale = Random.Range (0.5f, 0.8f);
		} else if (Score.score < 200) {
			newScale = Random.Range (0.5f, 1f);
		} else {
			newScale = Random.Range (0.5f, 1.5f);
		}

		float newRotation = Random.Range (0f, 360f);

		lilyPads.Add (new Lilypad(newPosition, newScale, newRotation));

		lastLilyPad.Instantiate (this.transform);
	}

	void HandleLilypadsInstances() {
		for (int i = lilyPads.Count - 1; i >= 0; i--) {
			Lilypad lilyPad = (Lilypad) lilyPads[i];
			if (lilyPad.IsInScreen()) {
				lilyPad.Instantiate (this.transform);
			} else {
				lilyPad.Destroy ();
			}
		}
	}

	public void Reset() {
		for (int i = lilyPads.Count - 1; i >= 0; i--) {
			Lilypad lilyPad = (Lilypad) lilyPads[i];
			lilyPad.Destroy ();
		}
		lilyPads.Clear ();
		Start ();
	}
}
