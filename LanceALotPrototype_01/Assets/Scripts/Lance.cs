using UnityEngine;
using System.Collections;

public class Lance : GameScript {

	private SpringJoint2D groundJoint;
	public GameObject ground;
	public bool hit = false;

	public LanceVisuals lanceVisuals;

	float lanceLength;
	float originalScale;
	bool bonusForceEnabled = true;
	bool lanceControlEnabled = true;

	void Start () {
		base.Start();
		groundJoint = GetComponent<SpringJoint2D>();
		originalScale = transform.localScale.x;
		lanceLength = Vector3.Distance(transform.position, transform.GetChild(0).position);
	}
	
	void FixedUpdate ()
	{
		lanceVisuals.lengthRatio = 1;

		if (!lanceControlEnabled)
			return;

		if (BlackBoard.Read<bool> ("Player", "ThumbSticksDown") && hit)
		{
			
			Vector3 targetPosition = groundJoint.connectedAnchor;
			Vector3 lookDirection = targetPosition - transform.position;

			BlackBoard.Write ("Player", "LanceDirection", lookDirection.normalized);

			transform.rotation = Quaternion.LookRotation (Vector3.forward, lookDirection);
			transform.Rotate (Vector3.forward, 90);

			float distanceRatio = Vector3.Distance (targetPosition, transform.position) / lanceLength;
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
		lanceVisuals.animateTip = false;
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
			//Vector3 targetPosition = groundJoint.connectedAnchor;
			//Vector3 lanceDirection = transform.position - targetPosition;
		}

		StartCoroutine (Release ());

		Time.timeScale = 1f;
	}

	[RegisterMessage("Player", "LanceHit")]
	void LanceHit()
	{
		groundJoint.distance = lanceLength;
		groundJoint.enabled = true;
		groundJoint.connectedAnchor = new Vector2 (transform.GetChild (0).position.x, transform.GetChild (0).position.y);
		hit = true;

		Time.timeScale = 0.2f;
	}

	void OnCollisionEnter2D(Collision2D target)
	{
		if (target.gameObject.tag == "Ground" && !hit)
		{
			SendMessage("Player", "LanceHit");
		}
	}

	IEnumerator Release()
	{
		yield return new WaitForSeconds(0.2f);
		SendMessage("Player", "ReleaseParts");
	}
}
