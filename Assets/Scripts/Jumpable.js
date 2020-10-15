#pragma strict

private var player: GameObject;

function Start () {
	player = GameObject.FindGameObjectWithTag("Player");
}

function Update () {
	/*if(player.transform.parent == null && Vector3.Distance(player.transform.position, transform.position) < GetComponent.<CircleCollider2D>().radius) {
		player.transform.parent = this.transform;
	}*/
}