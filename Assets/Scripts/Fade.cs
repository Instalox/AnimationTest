using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fade : MonoBehaviour {
	public float Delay = 1f;
	private float CurrentTime;
	public UnityEvent OnEndFade;
	public UnityEvent OnStartFade;

	private bool IsFading = false;

	void Update() {
		if ( !IsFading )
			return;

		CurrentTime += Time.deltaTime;
		if ( CurrentTime >= Delay ) {
			if ( OnEndFade != null )
				OnEndFade.Invoke();
			enabled = false;
		}
	}

	public void StartFade() {
		if (OnStartFade != null) OnStartFade.Invoke();
		IsFading = true;
	}
}
