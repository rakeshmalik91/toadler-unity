using UnityEngine;
using System.Collections;

public class LifeCollectible : MonoBehaviour {

	private Vector3 initPos;
	private static GameObject player = null;
	private bool taken = false;

	// Use this for initialization
	void Start () {
		initPos = transform.position;
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, initPos.y + Mathf.Sin(Time.time * 5) * 0.1f, transform.position.z);
		if (!taken && Vector2.Distance(player.transform.position, transform.position) <= 0.5f) {
			GetComponent<AudioSource> ().Play ();
			Life.life++;
			Life.lifeCollectedInCurrentGame++;
			PlayerPrefs.SetInt ("life", Life.life);
			PlayerPrefs.Save ();
			taken = true;
			GameObject.Destroy(transform.GetChild (0).gameObject);
		}
	}
}
