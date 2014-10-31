using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
	public Transform target;

	Vector3 offset;

	void Start()
	{
		transform.position = new Vector3 (23.36095f, transform.position.y, transform.position.z);
		offset = transform.position - target.position;
	}

	void LateUpdate()
	{
		transform.position = target.position + offset;
	}
}
