using UnityEngine;
using System.Collections;

public class Lance : GameScript {

	private SpringJoint2D groundJoint;
	public GameObject ground;
	public bool hit = false;

	public LanceVisuals lanceVisuals;

	float lanceLength = 9;
	float originalScale;
	bool bonusForceEnabled = true;
	bool lanceControlEnabled = true;

	void Start () {
		base.Start();
		groundJoint = GetComponent<SpringJoint2D>();
		originalScale = transform.localScale.x;
	}
	
	void FixedUpdate ()
	{

		if (!lanceControlEnabled)
			return;

		if (BlackBoard.Read<bool> ("Player", "ThumbSticksDown") && hit)
		{
			Vector3 targetPosition = groundJoint.connectedAnchor;
			Vector3 lookDirection = targetPosition - transform.position;
			transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDirection);
			transform.Rotate (Vector3.forward, 90);

			float distanceRatio = Vector3.Distance(targetPosition, transform.position)/10f;
			lanceVisuals.lengthRatio = distanceRatio;
			transform.localScale = new Vector3 (Mathf.Max (distanceRatio * originalScale, originalScale), transform.localScale.y, transform.localScale.z);

			if (distanceRatio > 1.1f)
				bonusForceEnabled = false;

			if (distanceRatio > 1.5f)
				SendMessage("Player", "ReleaseLance");

			//if (Vector3.Distance(targetPosition, transform.position) > 12)
			//	SendMessage("Player", "ReleaseLance");
		}
	}

	[RegisterMessage ("Player", "HitGround")]
	void PlayerHitGround()
	{
		lanceControlEnabled = false;
		groundJoint.enabled = false;
	}

	[RegisterMessage("Player", "ReleaseLance")]
	void ReleaseLance()
	{
		if (!lanceControlEnabled)
			return;
		
		groundJoint.enabled = false;

		// Add extra force for flying
		if (bonusForceEnabled)
		{
			Debug.Log("BONUS!");
			Vector3 targetPosition = groundJoint.connectedAnchor;
			Vector3 lanceDirection = transform.position - targetPosition;
			transform.parent.parent.rigidbody2D.AddForce(lanceDirection.normalized*15);
		}

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
