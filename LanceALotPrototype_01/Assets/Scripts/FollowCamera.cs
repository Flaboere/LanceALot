using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
	public Rigidbody2D[] targets;

	Vector3 minOffset;
	Vector3 maxOffset;
	float depth;
	float heightByDistanceRatio;
	float widthByHeightRatio;

	void Start()
	{		
		heightByDistanceRatio = 2*Mathf.Sin (Mathf.Deg2Rad*camera.fieldOfView/2);
		Bounds bounds = CalculateTargetBounds (targets);
		depth = targets[0].transform.position.z;
		float distance = depth - transform.position.z;
		float height = distance * heightByDistanceRatio;
		float width = height * camera.aspect;
		Vector3 cameraHalfSize = new Vector2 (width / 2, height / 2);
		minOffset = bounds.min - (transform.position - cameraHalfSize);
		maxOffset = (transform.position + cameraHalfSize) - bounds.max;
	}

	void LateUpdate()
	{
		Bounds bounds = CalculateTargetBounds (targets);
		bounds.Encapsulate (bounds.min - minOffset);
		bounds.Encapsulate (bounds.max + maxOffset);
		float height = bounds.size.y;
		float width = bounds.size.x;
		height = Mathf.Max (height, width / camera.aspect);
		float distance = height / heightByDistanceRatio;	
		transform.position = new Vector3(bounds.center.x, bounds.center.y, depth - distance);

		var position = transform.position;
		position.y = position.y + (-position.z * 0.125f) - 3.255478f/2f;
		transform.position = position;
		//transform.position = target.position + offset;
	}

	private Bounds CalculateTargetBounds(Rigidbody2D[] rigidbodies){
		Bounds toReturn = new Bounds(rigidbodies[0].worldCenterOfMass, Vector3.zero);
		foreach (Rigidbody2D rb in rigidbodies) {
			toReturn.Encapsulate(rb.worldCenterOfMass);
		}
		return toReturn;
	}
}
