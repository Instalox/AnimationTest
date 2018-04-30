using UnityEngine;

public class ParticleComponent : MonoBehaviour
{
    public Vector2 Acceleration;
    public bool Alive;
    public bool ColorOverTime;
    public float EndScaleMin, EndScaleMax;
    public Vector2 Gravity;
    public float Life;
    public float MaxLife;
    public bool RotateToVelocity;
    public bool ScaleOverTime;
    private SpriteRenderer spriteRenderer;
    public Color StartColor, EndColor;
    public Vector2 StartPosition;
    public Vector2 StartScale, EndScale;
    public float StartScaleMin, StartScaleMax;
    public bool UseRandomEndScale;
    public bool UseRandomStartScale;
    public Vector2 Velocity;
    private Material Mat { get; set; }

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Mat = spriteRenderer.material;
    }

    private void ScaleEffects() {
        var percent = Life / MaxLife;

        if (ScaleOverTime)
            transform.localScale = Vector3.Lerp(StartScale, EndScale, percent);

        if (!ColorOverTime)
            return;

        Mat.color = new Color(Mathf.Lerp(StartColor.r, EndColor.r, percent),
            Mathf.Lerp(StartColor.g, EndColor.g, percent),
            Mathf.Lerp(StartColor.b, EndColor.b, percent),
            Mathf.Lerp(StartColor.a, EndColor.a, percent));
        spriteRenderer.color = new Color(Mathf.Lerp(StartColor.r, EndColor.r, percent),
            Mathf.Lerp(StartColor.g, EndColor.g, percent),
            Mathf.Lerp(StartColor.b, EndColor.b, percent),
            Mathf.Lerp(StartColor.a, EndColor.a, percent));
    }

    private void ApplyGravity() {
        Acceleration += Gravity * Time.deltaTime;
    }

    public void SetGravity(Vector2 gravity) {
        Gravity = gravity;
    }

    private void LifeTick() {
        Life += Time.deltaTime;
        if (Life >= MaxLife) Kill();
    }

    public void Kill() {
        Alive = false;
        gameObject.SetActive(false);
    }

    public void Init(Vector2 pos, Vector2 horizontal, Vector2 vertical, Vector2 life, bool randomLife) {
        SetRandomVelocity(horizontal.x, horizontal.y, vertical.x, vertical.y);
        transform.position = new Vector3(pos.x, pos.y, 0f);
        Life = 0;
        if (randomLife)
            MaxLife = Random.Range(life.x, life.y);
        MaxLife = life.y;
        gameObject.SetActive(true);
        Alive = true;
        Mat.color = StartColor;
        if (UseRandomStartScale) {
            var randomScale = Random.Range(StartScaleMin, StartScaleMax);
            StartScale = new Vector2(randomScale, randomScale);
            transform.localScale = StartScale;
        }
        else {
            transform.localScale = StartScale;
        }
    }

    public void SetRandomVelocity(float xmin, float xmax, float ymin, float ymax) {
        Velocity = new Vector2(Random.Range(xmin, xmax), Random.Range(ymin, ymax));
    }

    public void SetRandomStartScale(float min, float max) {
        StartScaleMin = min;
        StartScaleMax = max;
        var randomScale = Random.Range(StartScaleMin, StartScaleMax);
        StartScale = new Vector2(randomScale, randomScale);
        transform.localScale = StartScale;
    }

    public void SetRandomEndScale(float min, float max) {
        EndScaleMin = min;
        EndScaleMax = max;
        var randomScale = Random.Range(EndScaleMin, EndScaleMax);
        EndScale = new Vector2(randomScale, randomScale);
    }

    public void SetStartEndScale(float start, float end) {
        StartScale = new Vector2(start, start);
        EndScale = new Vector2(end, end);
    }

    public void Tick() {
        if (!Alive)
            return;

        LifeTick();
        ScaleEffects();
        ApplyGravity();

        Velocity += Acceleration;

        transform.position += new Vector3(Velocity.x, Velocity.y, 0f) * Time.deltaTime;

        Acceleration = Vector2.zero;

        //point towards velocity
        if (!RotateToVelocity)
            return;
        var angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}