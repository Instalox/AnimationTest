using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    private float CurrentLerpTime;

    public GameObject ExplosionPrefab;

    public bool FollowMouse = false;
    private bool HasExploded;
    public float LerpSpeed = 3f;
    public UnityEvent OnArrived;
    public Vector3 TargetPos { get; set; }
    private Camera Cam { get; set; }

    private void Awake() {
        Cam = Camera.main;
    }

    private void Update() {
        if (FollowMouse)
            TargetPos = Cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        CurrentLerpTime += Time.deltaTime;

        var distToTarget = Vector3.Distance(transform.position, TargetPos);
        if (distToTarget <= .1f) {
            if (OnArrived != null)
                OnArrived.Invoke();

            if (!HasExploded) {
                var explosion = Instantiate(ExplosionPrefab, transform.position, transform.rotation);
                Destroy(explosion.gameObject,3f);
                HasExploded = true;
            }
        }

        var percent = CurrentLerpTime / LerpSpeed;
        transform.position = Vector3.Lerp(transform.position, TargetPos, percent);
    }
}