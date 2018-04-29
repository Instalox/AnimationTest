using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public bool FollowMouse = false;
	public Vector3 TargetPos { get; set; }
	private float CurrentLerpTime;
	public float LerpSpeed = 3f;
	public Action OnArrived;
	private Camera Cam { get; set; }

	void Awake() {
		Cam = Camera.main;
	}

	void Update() {
		if ( FollowMouse ) {
			TargetPos = Cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x , Input.mousePosition.y , 0));
		}

		CurrentLerpTime += Time.deltaTime;

		var distToTarget = Vector3.Distance(transform.position , TargetPos);
		if ( distToTarget <= .1f ) {

			if ( OnArrived != null )
				OnArrived.Invoke();
		}

		var percent = CurrentLerpTime / LerpSpeed;
		transform.position = Vector3.Lerp(transform.position , TargetPos , percent);
	}
}

