using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 resulting = target.position - transform.position;
		resulting.z = 0;
		transform.Translate(resulting*0.1f);*/
	}
}
