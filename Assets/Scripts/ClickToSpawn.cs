using UnityEngine;
using UnityEngine.Events;

public class ClickToSpawn : MonoBehaviour
{
    public GameObject CometPrefab;
    public UnityEvent OnProjectileHitTarget;
    public UnityEvent OnProjectileLaunch;

    public Vector3 RandomStartPosition;
    public float StartPosY;
    public Emitter CurrentEmitter { get; set; }

    private Camera Cam { get; set; }

    private void Awake() {
        Cam = Camera.main;
    }

    private void Start() {
        PickRandomSide();
    }

    private void PickRandomSide() {
        var left = Random.value;
        RandomStartPosition = Cam.ViewportToWorldPoint(left > .5
            ? new Vector3(-1, Random.Range(0, .5f), Cam.nearClipPlane)
            : new Vector3(1, Random.Range(0, .5f), Cam.nearClipPlane));
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            PickRandomSide();
            SpawnComet();
            if (OnProjectileLaunch != null)
                OnProjectileLaunch.Invoke();
        }
    }

    private void SpawnComet() {

        var screenPos = Cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        var comet = Instantiate(CometPrefab, RandomStartPosition, Quaternion.identity);
        var projectile = comet.GetComponent<Projectile>();
        var emitter = comet.GetComponent<Emitter>();
        if (emitter)
            projectile.OnArrived.AddListener(() => {
                CurrentEmitter = emitter;
                OnProjectileArrived();
            });

        projectile.TargetPos = screenPos;
    }

    private void OnProjectileArrived() {
        var emitters = CurrentEmitter.GetComponentsInChildren<Emitter>();
        foreach (var emitter in emitters) emitter.IsEmitting = false;

        if (OnProjectileHitTarget != null)
            OnProjectileHitTarget.Invoke();
    }
}