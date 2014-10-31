using UnityEngine;
using System.Collections;

public class Horse : MonoBehaviour {

	private bool fly=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!fly)
			rigidbody2D.AddForce(new Vector2(100,0));
	}

	public void Fly()
	{
		fly = true;
		rigidbody2D.AddTorque(-30f);
	}
}
