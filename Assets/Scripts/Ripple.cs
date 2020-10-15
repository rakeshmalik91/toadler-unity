using UnityEngine;
using System.Collections;

public class Ripple : MonoBehaviour {

	public float size = 1f;
	public float speed = 0.1f;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (0f, 0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale += new Vector3 (1f, 1f, 1f) * speed * size * Time.deltaTime * 100;
		GetComponent<SpriteRenderer>().color = new Color(
			GetComponent<SpriteRenderer>().color.r,
			GetComponent<SpriteRenderer>().color.g,
			GetComponent<SpriteRenderer>().color.b,
			GetComponent<SpriteRenderer>().color.a - speed * Time.deltaTime * 100
		);
		if(GetComponent<SpriteRenderer>().color.a <= 0f) {
			GameObject.Destroy(gameObject);
		}
	}
}
