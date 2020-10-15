using UnityEngine;
using System.Collections;

public class Bee : MonoBehaviour {

	private Vector3 initPos;

	// Use this for initialization
	void Start () {
		initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (initPos.x + Mathf.Sin(Time.time + GetHashCode()) * 0.3f, initPos.y + Mathf.Sin(Time.time * 2) * 0.3f, transform.position.z);
	}
}
