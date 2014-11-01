using UnityEngine;
using System.Collections;

public class CameraBounds : MonoBehaviour {

	public Transform Horse;
	public Vector2 
		Margin, 
		Smoothing;
	public BoxCollider2D Bounds;

	private Vector3
		_min, 
		_max;

	public bool isFollowing { get; set;}


	// Use this for initialization
	public void Start () {
		_min = Bounds.bounds.min;
		_max = Bounds.bounds.max;
		isFollowing = true;
	}
	
	// Update is called once per frame
	public void Update () {
		var x = transform.position.x;
		var y = transform.position.y;

		if (isFollowing) {
			if(Mathf.Abs(x - Horse.position.x) > Margin.x)
				x = Mathf.Lerp(x, Horse.position.x, Smoothing.x * Time.deltaTime);

			if(Mathf.Abs(y - Horse.position.y) > Margin.y)
				y = Mathf.Lerp(y, Horse.position.y, Smoothing.y * Time.deltaTime);
		}

		var CameraHalfWidth = camera.orthographicSize * ((float) Screen.width / Screen.height);

		x = Mathf.Clamp(x, _min.x + CameraHalfWidth, _max.x - CameraHalfWidth);
		y = Mathf.Clamp(y, _min.y + camera.orthographicSize, _max.y - camera.orthographicSize);
	}
}