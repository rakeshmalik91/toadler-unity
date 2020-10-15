using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	public Transform target;
	public Vector3 offset = new Vector3(0f, 7.5f, 0f);
	public bool constraintX = false;
	public bool constraintY = false;
	public bool constraintZ = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newTransform = new Vector3();
		if(!constraintX) newTransform.x = target.position.x + offset.x;
		if(!constraintY) newTransform.y = target.position.y + offset.y;
		if(!constraintZ) newTransform.z = target.position.z + offset.z;
		transform.position = newTransform;
	}
}
