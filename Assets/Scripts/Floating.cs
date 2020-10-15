using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour {

	private Vector3 initPos;
	private float seed;
	private float index;
	public float speed = 0.25f;
	public float amplitude = 1f;
	public float drownSpeed = 0.2f;

	public GameObject water;

	// Use this for initialization
	void Start () {
		initPos = transform.position;
		index = Camera.main.WorldToScreenPoint(transform.position).y / (Screen.height / 5);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = initPos + new Vector3(Mathf.PerlinNoise(0, (Time.time + index) * speed), 0, 0) * amplitude;
	}

	public void Drown() {
		tag = "Untagged";
		StartCoroutine ("DrownCoroutine");
	}

	public IEnumerator DrownCoroutine() {
		while (gameObject.activeSelf) {
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).tag == "Player") {
					continue;
				}
				if (transform.GetChild (i).GetComponent<SpriteRenderer> () != null) {
					transform.GetChild (i).GetComponent<SpriteRenderer> ().color = Color.Lerp (transform.GetChild (i).GetComponent<SpriteRenderer> ().color, new Color (1, 1, 1, 0), 0.1f);
				}
			}
			water.GetComponent<SpriteRenderer> ().color = Color.Lerp (water.GetComponent<SpriteRenderer> ().color, new Color (1, 1, 1, 0), 0.1f);
			yield return new WaitForSeconds (0.01f);
		}
		yield return null;
	}
}
