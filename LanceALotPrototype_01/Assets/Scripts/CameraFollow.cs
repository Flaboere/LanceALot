using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	private Transform actualTarget;
	public Vector3 resulting = Vector3.zero;

	// Use this for initialization
	void Start () {
		actualTarget.position=transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp("space"))
		{
			actualTarget.position = target.position;
		}

		resulting = actualTarget.position - transform.position;

		resulting.z = 0;
		transform.Translate(resulting*0.1f);
	}
}
