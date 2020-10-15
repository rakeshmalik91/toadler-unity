#pragma strict

public var offset: Vector3 = new Vector3(0f, 7.5f, 0f);
public var target: Transform;

function Start () {
	transform.position = target.position + offset;
}
