using System.Collections;
using System.Globalization;
using UnityEngine;

public class ParticleComponent : MonoBehaviour {

    public bool Alive;
    public bool ColorOverTime;
    public bool ScaleOverTime;
    public Color StartColor, EndColor;
    public bool UseRandomStartScale;
    public float StartScaleMin, StartScaleMax;
    public Vector2 StartScale, EndScale;
    public Vector2 Velocity;
    public Vector2 Acceleration;
    public Vector2 StartPosition;
    public Vector2 Gravity;
    public float Life;
    public float MaxLife;
    private SpriteRenderer spriteRenderer;
    private Material Mat { get; set; }

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Mat = spriteRenderer.material;
    }

    private void ScaleEffects() {
        var percent = Life / MaxLife;

        if ( ScaleOverTime )
            transform.localScale = Vector3.Lerp(StartScale , EndScale , percent);

        if ( !ColorOverTime )
            return;

        Mat.color = new Color(Mathf.Lerp(StartColor.r , EndColor.r , percent) ,
            Mathf.Lerp(StartColor.g , EndColor.g , percent) ,
            Mathf.Lerp(StartColor.b , EndColor.b , percent) ,
            Mathf.Lerp(StartColor.a , EndColor.a , percent));
        //Color.Lerp(startColor , endColor , percent);
        spriteRenderer.color = new Color(Mathf.Lerp(StartColor.r , EndColor.r , percent) ,
            Mathf.Lerp(StartColor.g , EndColor.g , percent) ,
            Mathf.Lerp(StartColor.b , EndColor.b , percent) ,
            Mathf.Lerp(StartColor.a , EndColor.a , percent));
    }

    private void ApplyGravity() {
        Acceleration += Gravity * Time.deltaTime;
    }

    public void SetGravity( Vector2 gravity ) {
        Gravity = gravity;
    }

    private void LifeTick() {
        Life += Time.deltaTime;
        if ( Life >= MaxLife ) {
            Kill();
        }
    }

    public void Kill() {
        Alive = false;
        gameObject.SetActive(false);
        //StopAllCoroutines();
    }

    public void Init( Vector2 pos , Vector2 horizontal , Vector2 vertical , Vector2 life ) {

        SetRandomVelocity(horizontal.x , horizontal.y , vertical.x , vertical.y);
        transform.position = new Vector3(pos.x , pos.y , 0f);
        //Life = MaxLife;
        Life = 0;
        MaxLife = Random.Range(life.x , life.y);
        gameObject.SetActive(true);
        Alive = true;
        //spriteRenderer.color = StartColor;
        Mat.color = StartColor;
        if ( UseRandomStartScale ) {
            var randomScale = Random.Range(StartScaleMin , StartScaleMax);
            StartScale=new Vector2(randomScale,randomScale);
            transform.localScale = StartScale;
        }
        else {
            transform.localScale = StartScale;
        }
    }

    public void SetRandomVelocity( float xmin , float xmax , float ymin , float ymax ) {
        Velocity = new Vector2(Random.Range(xmin , xmax) , Random.Range(ymin , ymax));

    }

    public void SetRandomStartScale(float min, float max) {
        StartScaleMin = min;
        StartScaleMax = max;
        var randomScale = Random.Range(StartScaleMin , StartScaleMax);
        StartScale=new Vector2(randomScale,randomScale);
    }

    public IEnumerator FadeToColor( Color startColor , Color endColor ) {
        spriteRenderer.color = startColor;
        var mat = spriteRenderer.material;
        mat.color = startColor;
        while ( Life < MaxLife ) {
            var percent = Life / MaxLife;

            mat.color = new Color(Mathf.Lerp(startColor.r , endColor.r , percent) ,
                Mathf.Lerp(startColor.g , endColor.g , percent) ,
                Mathf.Lerp(startColor.b , endColor.b , percent) ,
                Mathf.Lerp(startColor.a , endColor.a , percent));
            //Color.Lerp(startColor , endColor , percent);
            spriteRenderer.color = new Color(Mathf.Lerp(startColor.r , endColor.r , percent) ,
                Mathf.Lerp(startColor.g , endColor.g , percent) ,
                Mathf.Lerp(startColor.b , endColor.b , percent) ,
                Mathf.Lerp(startColor.a , endColor.a , percent));//Color.Lerp(startColor , endColor , percent);


            yield return null;
        }
    }

    public IEnumerator ScaleParticlesOverTime( Vector2 start , Vector2 end ) {
        transform.localScale = start;
        while ( Life < MaxLife ) {
            var percent = Life / MaxLife;
            transform.localScale = Vector3.Lerp(start , end , percent);
            yield return null;
        }
    }

    public void Tick() {
        if ( !Alive )
            return;

        LifeTick();
        ScaleEffects();
        ApplyGravity();

        Velocity += Acceleration;

        transform.position += new Vector3(Velocity.x , Velocity.y , 0f) * Time.deltaTime;

        Acceleration = Vector2.zero;

        //point towards velocity
        float angle = Mathf.Atan2(Velocity.y , Velocity.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle , Vector3.forward);
    }
}