using UnityEngine;
using System.Collections;

public class Frog : MonoBehaviour {

	private bool  dragging = false;
	private Vector2 jumpTarget;
	private float jumpTime;
	private bool  jumping = false;
	private Vector3 jumpStart;
	private Vector3 defaultScale;
	private Animator animator;
	private bool dead = false;

	public Vector3 cameraOffset = new Vector3(0, 2, -50);
	public GameObject arrow;
	public float minJumpDist = 3f;
	public float maxJumpDist = 15f;
	public AnimationCurve jumpCurve;
	public float jumpSpeed = 0.1f;

	public AudioClip[] sounds;
	public AudioClip[] splashSound;

	void Start (){
		animator = GetComponent<Animator>();
		defaultScale = transform.localScale;
		ResetArrow();
		CheckGround();
	}

	void Update (){
		if(jumping) {
			Jump();
		} else {
			if(dragging) {
				SetAngle();
				SetDistance();
			}
			if(Input.GetMouseButtonUp(0)) {
				StartJump();
				ResetArrow();
			}
			if (!GameMenu.paused) {
				DrownLilypad ();
			}
		}
	}

	void OnMouseDown (){
		if(GameMenu.paused) {
			return;
		}
		if(!jumping) {
			dragging = true;
		}
	}

	void SetAngle (){
		Vector2 p1 = Camera.main.WorldToScreenPoint(transform.position);
		Vector2 p2 = Input.mousePosition;
		float angle = Vector2.Angle(p1 - p2, new Vector2(0, 1));
		if(p1.x > p2.x) {
			angle = -angle;
		}
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
	}

	void SetDistance (){
		Vector2 p1 = transform.position;
		Vector2 p2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float dist= Vector2.Distance(p1, p2);
		arrow.SetActive(true);
		arrow.transform.localPosition = new Vector3(
			arrow.transform.localPosition.x,
			Mathf.Clamp(F(dist), 0, (maxJumpDist - minJumpDist) / arrow.transform.lossyScale.y) + (minJumpDist / arrow.transform.lossyScale.y),
			arrow.transform.localPosition.z
		);
	}

	//function for cursor movement
	float F (float dist){
		return dist * dist * dist * 5;
	}

	void ResetArrow () {
		arrow.transform.localPosition = new Vector3(arrow.transform.localPosition.x, 0, arrow.transform.localPosition.z);
		arrow.SetActive(false);
	}

	void StartJump () {
		AudioClip sound = sounds[(int) Random.Range(0, sounds.Length)];
		GetComponent<AudioSource> ().clip = sound;
		GetComponent<AudioSource> ().Play ();

		if (GameMenu.paused) {
			return;
		}

		if(Vector2.Distance(arrow.transform.position, transform.position) >= minJumpDist) {
			jumpTarget = arrow.transform.position;
			jumpTime = 0;
			jumping = true;
			animator.SetBool ("jump", true);
			dead = false;
			animator.SetBool ("dead", false);
			dragging = false;
			jumpStart = transform.position;
		}
	}

	void Jump () {
		if (GameMenu.paused) {
			return;
		}
		Transform parent = transform.parent;
		transform.parent = null;
		jumpTime += jumpSpeed;
		transform.position = new Vector3(
			Mathf.Lerp(jumpStart.x, jumpTarget.x, jumpCurve.Evaluate(jumpTime)), 
			Mathf.Lerp(jumpStart.y, jumpTarget.y, jumpCurve.Evaluate(jumpTime)),
			transform.position.z
		);
		transform.localScale = defaultScale * (2 - jumpCurve.Evaluate(jumpTime));
		if(Mathf.Abs(jumpTime - 1.0f) < 0.001f) {
			transform.position = new Vector3(jumpTarget.x, jumpTarget.y, transform.position.z);
			transform.localScale = defaultScale;
			StartCoroutine("SetCamera");
			jumping = false;
			animator.SetBool ("jump", false);
			CheckGround();
		} else {
			transform.parent = parent;
		}
	}

	IEnumerator SetCamera () {
		Vector3 target = transform.position + cameraOffset;
		while(Vector3.Distance(Camera.main.transform.position, target) > 0.01f) {
			Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, target, 0.2f);
			yield return new WaitForSeconds(0.01f);
		}
		Camera.main.transform.position = target;
		yield return null;
	}

	void CheckGround (){
		foreach(GameObject jumpable in GameObject.FindGameObjectsWithTag("Jumpable")) {
			if(jumpable.transform.GetComponent<Collider2D>().bounds.Contains(new Vector3(transform.position.x, transform.position.y))) {
				transform.parent = jumpable.transform;
				break;
			}
		}
		//drown if on water
		if(transform.parent == null) {
			Die ();
		}
	}

	private float lilypadSitTime = -1;

	void DrownLilypad() {
		if (transform.parent != null) {
			if (transform.parent.GetComponent<Floating> () != null) {
				if (lilypadSitTime >= 0) {
					lilypadSitTime = Time.timeSinceLevelLoad;
				}
				Transform water = transform.parent.GetComponent<Floating> ().water.transform;
				if (water.localScale.x >= 1f) {
					transform.parent.GetComponent<Floating> ().Drown ();
					transform.parent = null;
					CheckGround ();
				} else {
					float scroreBasedDrownSpeed;
					if (Score.score < 25) {
						scroreBasedDrownSpeed = 0.25f;
					} else if (Score.score < 50) {
						scroreBasedDrownSpeed = 0.5f;
					} else if (Score.score < 75) {
						scroreBasedDrownSpeed = 0.75f;
					} else if (Score.score < 100) {
						scroreBasedDrownSpeed = 1f;
					} else if (Score.score < 150) {
						scroreBasedDrownSpeed = 1.5f;
					} else if (Score.score < 200) {
						scroreBasedDrownSpeed = 2f;
					} else if (Score.score < 300) {
						scroreBasedDrownSpeed = 2.25f;
					} else if (Score.score < 400) {
						scroreBasedDrownSpeed = 2.5f;
					} else if (Score.score < 500) {
						scroreBasedDrownSpeed = 2.75f;
					} else {
						scroreBasedDrownSpeed = 3f;
					}
					water.localScale += new Vector3 (1f, 1f, 1f) * Time.deltaTime * transform.parent.GetComponent<Floating> ().drownSpeed * scroreBasedDrownSpeed;
				}
			}
		} else {
			lilypadSitTime = -1;
		}
	}

	void Die() {
		AudioClip sound = splashSound[(int) Random.Range(0, splashSound.Length)];
		GetComponent<AudioSource> ().clip = sound;
		GetComponent<AudioSource> ().Play ();


		transform.parent = null;
		animator.SetBool("dead", true);
		dead = true;
		dragging = false;
		jumping = false;

		GameMenu.paused = true;

		if (Life.life > 0) {
			GameObject.Find ("ButtonPlay").GetComponent<UnityEngine.UI.Button> ().interactable = true;
			GameObject.Find ("ButtonPlay").transform.Find ("Life").gameObject.SetActive (true);
			GameObject.Find ("ButtonPlay").transform.Find ("Text").GetComponent<UnityEngine.UI.Text> ().text = "Continue";
			GameObject.Find ("ButtonPlay").transform.Find ("Text").GetComponent<UnityEngine.UI.Text> ().color = Util.SetAlpha(1f, GameObject.Find ("ButtonPlay").transform.Find ("Text").GetComponent<UnityEngine.UI.Text> ().color);
		} else {
			GameObject.Find ("ButtonPlay").GetComponent<UnityEngine.UI.Button> ().interactable = false;
			GameObject.Find ("ButtonPlay").transform.Find ("Life").gameObject.SetActive (false);
			GameObject.Find ("ButtonPlay").transform.Find ("Text").GetComponent<UnityEngine.UI.Text> ().text = "Play";
			GameObject.Find ("ButtonPlay").transform.Find ("Text").GetComponent<UnityEngine.UI.Text> ().color = Util.SetAlpha(0.5f, GameObject.Find ("ButtonPlay").transform.Find ("Text").GetComponent<UnityEngine.UI.Text> ().color);
		}
		GameObject.Find ("ButtonRetry").GetComponent<UnityEngine.UI.Button> ().interactable = true;

		GameMenu.SaveGame ();

		GameMenu.ReportGPGSProgress ();
	}

	public void Reset() {
		transform.parent = null;
		transform.position = new Vector3 (0, 0, transform.position.z);
		animator.SetBool("dead", false);
		dead = false;
		dragging = false;
		jumping = false;
		Start ();
		StartCoroutine("SetCamera");
	}

	public bool IsDead() {
		return dead;
	}

	public bool IsJumping() {
		return jumping;
	}
}