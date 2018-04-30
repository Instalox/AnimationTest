using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Emitter : MonoBehaviour {

    #region UI Variables

    [Header("UI References")] public Slider HorizontalSlider;

    public Slider VerticalSliderMin;
    public Slider VerticalSliderMax;
    public Slider EmissionRateSlider;
    public Slider ParticlesPerEmitSlider;
    public Slider ParticleLifeMinSlider;
    public Slider ParticleLifeMaxSlider;

    public Text EmissionRateText,
        ParticlesPerEmitText,
        ParticleLifeMinText,
        ParticleLifeMaxText;

    public Text HorVelTex, VertMinText, VertMaxText;

    #endregion

    [Header("Emitter Settings")]
    public bool UseParticleEditor;
    public GameObject ParticlePrefab;
    private float CurrentSpawnTime;
    public float EmissionRate = 1;
    public Vector2 Gravity;
    public Vector2 HorizontalSpawnVelocity;
    public Vector2 VerticalSpawnVelocity;
    public bool IsEmitting;
    public bool OverrideParticleLifeSpan;
    public bool UseRandomLife;
    public bool UseRandomScale;
    public bool OverrideParticleScale;
    public bool UseRandomRotation;
    public float RotationMin, RotationMax;
    public float ParticleLifeMin, ParticleLifeMax;
    public float StartScaleMin, StartScaleMax;
    public float EndScaleMin, EndScaleMax;
    public int ParticlesPerEmit;
    public int PoolAmount = 250;
    private readonly List<ParticleComponent> ParticleList = new List<ParticleComponent>();
    public bool IsCameraClearing { get; set; }
    public Camera Cam { get; set; }

    private void Awake() {
        Cam = Camera.main;
        IsCameraClearing = true;
        PoolParticles(PoolAmount);
        SetupParticleUI();
    }

    private void SetupParticleUI() {
        if ( !UseParticleEditor )
            return;
        SetEmissionRate();
        SetHorizontalVelocity();
        SetParticlesPerEmission();
        SetVerticalVelocityMin();
        SetVerticalVelocityMax();
        SetParticleMinLife();
        SetParticleMaxLife();
    }

    private void Update() {
        UpdateParticles();

        if ( !IsEmitting )
            return;

        CurrentSpawnTime += Time.deltaTime;

        if ( CurrentSpawnTime >= EmissionRate ) {
            Emit();
            CurrentSpawnTime = 0;
        }
    }

    private void UpdateParticles() {
        for ( int i = 0; i < ParticleList.Count; i++ ) {
            var part = ParticleList[i];
            if ( part.Alive ) {
                part.Tick();
            }
            //ParticleList[i].Tick();
        }
    }


    /// <summary>
    /// Create a pool of particles and add it to the list.
    /// </summary>
    /// <param name="amt"></param>
    private void PoolParticles( int amt ) {
        var ParticleHolder = new GameObject("Particles");
        for ( var i = 0; i < amt; i++ ) {
            var particle = Instantiate(ParticlePrefab , transform.position , transform.rotation);
            var particleComponent = particle.GetComponent<ParticleComponent>();
            ParticleList.Add(particleComponent);
            particle.SetActive(false);
            //particle.transform.parent = ParticleHolder.transform;
        }
    }

    /// <summary>
    /// Grab a particle out of the pool
    /// </summary>
    /// <returns></returns>
    public ParticleComponent GetInactiveParticle() {
        for ( var i = 0; i < ParticleList.Count; i++ )
            if ( !ParticleList[i].Alive )
                return ParticleList[i];
        return null;
    }


    public void Emit() {
        for ( var i = 0; i < ParticlesPerEmit; i++ ) {
            var particle = GetInactiveParticle();
            if ( particle != null ) {
                particle.Init(transform.position ,
                    HorizontalSpawnVelocity ,
                    VerticalSpawnVelocity ,
                    new Vector2(ParticleLifeMin , ParticleLifeMax), UseRandomLife);

                particle.SetGravity(Gravity);
                if ( UseRandomRotation ) {
                    var randomRot = Random.Range(RotationMin , RotationMax);
                    particle.transform.Rotate(0 , 0 , randomRot);

                }

                if ( OverrideParticleScale ) {
                    if ( UseRandomScale ) {
                        particle.SetRandomStartScale(StartScaleMin , StartScaleMax);
                        particle.SetRandomEndScale(EndScaleMin , EndScaleMax);

                    }
                    else {
                        particle.SetStartEndScale(StartScaleMin,EndScaleMin);
                    }
                }

                if (OverrideParticleLifeSpan) {
                   // particle.Life = ParticleLifeMin;
                    particle.MaxLife = ParticleLifeMax;
                }
            }
        }
    }

    // UI Methods------------------
    public void SetHorizontalVelocity() {
        var vel = HorizontalSlider.value;
        var velstring = vel.ToString("#.#");
        HorVelTex.text = "Horizontal Vel : -" + velstring + " | " + velstring;

        var velocity = new Vector2(-vel , vel);
        HorizontalSpawnVelocity = velocity;
    }

    public void SetVerticalVelocityMin() {
        if ( VerticalSliderMin.value > VerticalSliderMax.value )
            VerticalSliderMax.value = VerticalSliderMin.value;

        var vel = VerticalSliderMin.value;
        VertMinText.text = "Vertical Velocity Min: " + vel.ToString("#.##");
        var velocity = new Vector2(vel , VerticalSpawnVelocity.y);
        VerticalSpawnVelocity = velocity;
    }

    public void SetVerticalVelocityMax() {
        if ( VerticalSliderMax.value < VerticalSliderMin.value )
            VerticalSliderMin.value = VerticalSliderMax.value;

        var vel = VerticalSliderMax.value;
        VertMaxText.text = "Vertical Velocity Max: " + vel.ToString("#.##");
        var velocity = new Vector2(VerticalSpawnVelocity.x , vel);
        VerticalSpawnVelocity = velocity;
    }

    public void SetEmissionRate() {
        var rate = EmissionRateSlider.value;
        EmissionRateText.text = "Emission Delay: " + rate.ToString("##.##");
        EmissionRate = rate;
    }

    public void SetParticlesPerEmission() {
        var rate = ParticlesPerEmitSlider.value;
        ParticlesPerEmitText.text = "Particles Per Emit: " + rate;
        ParticlesPerEmit = (int) rate;
    }

    public void SetParticleMinLife() {
        if ( ParticleLifeMinSlider.value > ParticleLifeMaxSlider.value )
            ParticleLifeMaxSlider.value = ParticleLifeMinSlider.value;

        var life = ParticleLifeMinSlider.value;
        ParticleLifeMinText.text =
            "Particle Life Min: " + ParticleLifeMin.ToString("##.###");
        ParticleLifeMin = life;
    }

    public void SetParticleMaxLife() {
        if ( ParticleLifeMaxSlider.value < ParticleLifeMinSlider.value )
            ParticleLifeMaxSlider.value = ParticleLifeMinSlider.value;

        var life = ParticleLifeMaxSlider.value;

        ParticleLifeMaxText.text =
            "Particle Life Max: " + ParticleLifeMax.ToString("##.#");
        ParticleLifeMax = life;
    }

    public void ToggleEmitter() {
        IsEmitting = !IsEmitting;
    }

    public void ToggleCameraClear() {
        IsCameraClearing = !IsCameraClearing;
        Cam.clearFlags = IsCameraClearing
            ? CameraClearFlags.SolidColor
            : CameraClearFlags.Depth;
    }
}