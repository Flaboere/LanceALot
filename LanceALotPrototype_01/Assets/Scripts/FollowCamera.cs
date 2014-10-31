using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
	public Transform target;

	Vector3 offset;

	void Start()
	{
		offset = transform.position - target.position;
	}

	void LateUpdate()
	{
		transform.position = target.position + offset;
	}
}
