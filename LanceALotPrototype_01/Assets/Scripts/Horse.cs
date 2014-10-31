using UnityEngine;
using System.Collections;

public class Horse : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.AddForce(new Vector2(100,0));
	}
}
