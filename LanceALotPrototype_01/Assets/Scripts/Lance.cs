using UnityEngine;
using System.Collections;

public class Lance : MonoBehaviour {

	private SpringJoint2D groundJoint;
	public GameObject ground;
	public bool hit = false;

	// Use this for initialization
	void Start () {
		groundJoint = GetComponent<SpringJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(groundJoint.GetReactionForce(1).magnitude);
		if (hit && Input.GetKeyUp("space"))
		{
			groundJoint.enabled = false;
		}
	}

	void OnCollisionEnter2D(Collision2D target)
	{
		if (target.gameObject.tag == "Ground" && !hit)
		{
			Debug.Log("FLY!");
			groundJoint.enabled = true;
			groundJoint.connectedAnchor = new Vector2( transform.GetChild (0).position.x,transform.GetChild (0).position.y);
			//transform.parent.parent.GetComponent<HingeJoint2D>().enabled = false;
			transform.parent.parent.rigidbody2D.mass = 0.01f;
			transform.parent.parent.GetComponent<Horse>().Fly();
			transform.parent.rigidbody2D.mass = 0.01f;
			rigidbody2D.mass = 0.01f;
			hit = true;
			StartCoroutine(Release());
		}
	}

	IEnumerator Release()
	{
		yield return new WaitForSeconds(0.7f);
		transform.parent.parent.GetComponent<HingeJoint2D>().enabled=false;
		transform.parent.GetComponent<HingeJoint2D>().enabled=false;
	}
}
