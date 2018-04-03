using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolution : MonoBehaviour {
	public Transform point;
	public float speed;
	private float ry,rz;

	void Start () {
		ry = Random.Range (0, 360);
		rz = Random.Range (0, 360);
	}

	void Update () {
		Vector3 axis = new Vector3(0,ry,rz);
		this.transform.RotateAround(point.position,axis,speed * Time.deltaTime);
	}
}
