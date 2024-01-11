using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : MonoBehaviour
{
    ParticleSystem hitParticle;

    float currentWaitTime;

    private void Awake()
    {
        hitParticle = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        hitParticle.Play();
    }

    private void Update()
    {
        currentWaitTime += Time.deltaTime;

        if(currentWaitTime > hitParticle.main.duration)
        {
            ObjectPoolManager.Instance.ReturnObject(PoolType.HitParticle, gameObject);
        }
    }

    private void OnDisable()
    {
        currentWaitTime = 0;
        hitParticle.Stop();
    }
}
