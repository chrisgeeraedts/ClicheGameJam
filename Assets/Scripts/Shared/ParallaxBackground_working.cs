 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground_working : MonoBehaviour {

	private float length, startpos;
	public GameObject cam;
	public float y_offset;
	public float scale_offset;
	public float parallaxSpeed;

	// Use this for initialization
	void Start () {
		startpos = transform.position.x;
		length = GetComponent<SpriteRenderer>().bounds.size.x * scale_offset;

	}


	
	// Update is called once per frame
	void Update () {
		float temp = (cam.transform.position.x * (1 - parallaxSpeed));
		float dist = (cam.transform.position.x * parallaxSpeed);
		transform.position = new Vector3(startpos + dist, transform.position.y + y_offset, transform.position.z);

		if(temp > startpos + length) startpos += length;
		else if (temp < startpos - length) startpos -= length;
	}
}
