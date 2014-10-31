using UnityEngine;
using System.Collections;

public class Lance : GameScript {

	private SpringJoint2D groundJoint;
	public GameObject ground;
	public bool hit = false;

	void Start () {
		base.Start();
		groundJoint = GetComponent<SpringJoint2D>();
	}
	
	void FixedUpdate () {

		if (BlackBoard.Read<bool> ("Player", "ThumbSticksDown") && hit)
		{
			Vector3 targetPosition = groundJoint.connectedAnchor;
			Vector3 lookDirection = targetPosition - transform.position;
			transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDirection);
			transform.Rotate (Vector3.forward, 90);

			if (Vector3.Distance(targetPosition, transform.position) > 12)
				SendMessage("Player", "ReleaseLance");
		}
	}

	[RegisterMessage("Player", "ReleaseLance")]
	void ReleaseLance()
	{
		groundJoint.enabled = false;
		StartCoroutine (Release ());
	}

	void OnCollisionEnter2D(Collision2D target)
	{
		if (target.gameObject.tag == "Ground" && !hit)
		{
			groundJoint.enabled = true;
			groundJoint.connectedAnchor = new Vector2( transform.GetChild (0).position.x,transform.GetChild (0).position.y);
			transform.parent.parent.rigidbody2D.mass = 0.01f;
			transform.parent.rigidbody2D.mass = 0.01f;
			rigidbody2D.mass = 0.01f;
			hit = true;

			SendMessage("Player", "LanceHit");
		}
	}

	IEnumerator Release()
	{
		yield return new WaitForSeconds(0.4f);
		transform.parent.parent.GetComponent<HingeJoint2D>().enabled=false;
		transform.parent.GetComponent<HingeJoint2D>().enabled=false;
	}
}
