using UnityEngine;
using System.Collections;

public class RippleGenerator : MonoBehaviour {

	public GameObject dummy;

	public float depth = 3f;

	private static GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Random.Range (0f, 1f) < 0.01f) {
			GameObject ripple = GameObject.Instantiate (dummy);
			ripple.transform.parent = transform;
			ripple.transform.position = new Vector3 (
				player.transform.position.x + Random.Range (-3f, 3f), 
				player.transform.position.y + Random.Range (-5f, 5f), 
				depth
			);
			ripple.GetComponent<Ripple> ().size = Random.Range (0.2f, 1f);
		}
		if (Random.Range (0f, 1f) < 0.5f && player.GetComponent<Frog>().IsJumping()) {
			GameObject ripple = GameObject.Instantiate (dummy);
			ripple.transform.parent = transform;
			ripple.transform.position = new Vector3 (
				player.transform.position.x, 
				player.transform.position.y, 
				depth
			);
			ripple.GetComponent<Ripple> ().size = Random.Range (0.2f, 0.3f);
		} else if (Random.Range (0f, 1f) < 0.01f && player.GetComponent<Frog>().IsDead()) {
			GameObject ripple = GameObject.Instantiate (dummy);
			ripple.transform.parent = transform;
			ripple.transform.position = new Vector3 (
				player.transform.position.x, 
				player.transform.position.y, 
				depth
			);
			ripple.GetComponent<Ripple> ().size = Random.Range (0.2f, 0.5f);
		}
	}
}
