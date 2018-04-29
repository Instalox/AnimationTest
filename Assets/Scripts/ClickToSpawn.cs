using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToSpawn : MonoBehaviour {
	public GameObject CometPrefab;

	public Vector3 RandomStartPosition;
	public float StartPosY;

	private Camera Cam { get; set; }

	void Awake() {
		Cam = Camera.main;
	}

	void Start() {
		PickRandomSide();
	}

	void PickRandomSide() {
		var left = Random.value;
		RandomStartPosition = Cam.ViewportToWorldPoint(left > .5 ?
			new Vector3(-1 , Random.Range(0 , .5f) , Cam.nearClipPlane)
			: new Vector3(1 , Random.Range(0 , .5f) , Cam.nearClipPlane));
	}

	void Update() {
		if ( Input.GetMouseButtonDown(0) ) {
			PickRandomSide();
			SpawnComet();
		}
	}

	private void SpawnComet() {
		//var randompos = Camera.main.ViewportToWorldPoint(RandomStartPos);

		var screenPos = Cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x , Input.mousePosition.y , 0));

		var comet = Instantiate(CometPrefab , RandomStartPosition , Quaternion.identity);
		var projectile = comet.GetComponent<Projectile>();
		projectile.TargetPos = screenPos;

	}
}
