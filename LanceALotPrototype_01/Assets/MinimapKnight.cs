using UnityEngine;
using System.Collections;

public class MinimapKnight : MonoBehaviour
{

	public Transform target;

	void LateUpdate () {

		transform.position = new Vector3 (target.position.x, transform.position.y, transform.position.z);
	}
}
