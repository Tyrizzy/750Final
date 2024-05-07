using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSys : MonoBehaviour
{
    private SpriteRenderer SpriteRender;
    private PhysicsController physicsController;

    [SerializeField] private GameObject Prefab;
    [SerializeField] private GameObject Parent;
    [SerializeField] private Color ParticleColor = Color.white;
    [SerializeField][Range(0, 5)] private float ParticleMass = 1;
    [SerializeField] private float ParticleSize = 1;
    [SerializeField] private float ParticleSpeed =  1;
    [SerializeField] private float ParticleLifeTime = 1;
    [SerializeField] private float ParticleRotation = 0;
    int ParticleCount = 0;

    private void Awake()
    {
        Prefab.GetComponent<SpriteRenderer>().color = ParticleColor;
        physicsController = Prefab.GetComponent<PhysicsController>();
    }

    public void SpawnParticle()
    {
        Vector2 RandSpeed = new Vector2(Random.Range(-ParticleSpeed, ParticleSpeed), Random.Range(-ParticleSpeed, ParticleSpeed));
        Vector2 PSize = new Vector2(ParticleSize, ParticleSize);

        physicsController.mass = ParticleMass;
        Prefab.transform.localScale = PSize;
        Prefab.transform.rotation = Quaternion.Euler(0, 0, ParticleRotation * Time.deltaTime);

        physicsController.velocity = RandSpeed;
        GameObject DestroyableParticle = Instantiate(Prefab, Parent.transform.position, Quaternion.identity);

        if (DestroyableParticle != null)
        {
            ParticleCount++;
        }

        Destroy(DestroyableParticle, ParticleLifeTime);

        GameDebugger.DebugLog(7, "Particles are Being Spawned");
        GameDebugger.DebugLog(7, "Total Particles Spawned: " + ParticleCount);
    }
}
