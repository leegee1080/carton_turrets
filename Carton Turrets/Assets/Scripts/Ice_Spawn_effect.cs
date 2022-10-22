using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Spawn_effect : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] iceSprites;
    [SerializeField] ParticleSystem[] IceDestroyFXs;
    [SerializeField] float SecondsBetweenUpdate = 0.1f;
    [SerializeField] float SecondsTillDestroy = 1f;
    [SerializeField] int NumberOfSteps = 100;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnIce());
        StartCoroutine(RemoveIce());
    }

    private IEnumerator RemoveIce()
    {
        yield return new WaitForSeconds(SecondsTillDestroy);

        foreach(SpriteRenderer iceSprite in iceSprites)
        {
            iceSprite.enabled = false;
        }

        foreach(ParticleSystem IceDestroyFX in IceDestroyFXs)
        {
            IceDestroyFX.Play();
        }

        yield return new WaitForSeconds(SecondsTillDestroy);

        Destroy(this.gameObject);

    }

    private IEnumerator SpawnIce()
    {
        for(int i = NumberOfSteps; i >= 0; i--)
        {
            foreach (SpriteRenderer iceSprite in iceSprites)
            {
                iceSprite.material.SetFloat("_DissolveAmount", ((float)i)/((float)NumberOfSteps));
            }
            yield return new WaitForSeconds(SecondsBetweenUpdate);
        }
    }
}
