using UnityEngine;

[CreateAssetMenu]
public class EmitterConfig : ScriptableObject {
    public int ParticlesPerEmit;
    public bool IsEmitting;
    public float SpawnRate = 1;
    public float ParticleLifeMin, ParticleLifeMax;
    public Vector2 HorizontalSpawnVelocity;
    public Vector2 VerticalSpawnVelocity;
    public Vector2 Gravity;
}