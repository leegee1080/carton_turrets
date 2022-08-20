using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActor : MonoBehaviour
{
    [SerializeField]protected SphereCollider _collider;
    // public ActorDataScriptableObject ActorData;
    public GameObject ActorArtContainer;
    public Renderer MainSprite;
    public ActorStatesAbstractClass CurrentStateClass;

    [Header("Local Vars")]
    public float CurrentSpeed;
    public float CurrentHealth;

    [Header("Damage Effect Vars")]
    public Color DamageColor;
    
    public IEnumerator DamageBlinkerCoroutine;

    public virtual void ChangeState(ActorStatesAbstractClass newState)
    {
        if(CurrentStateClass != null){CurrentStateClass.OnExitState(this);}
        CurrentStateClass = newState;
        CurrentStateClass.OnEnterState(this);
    }

    public virtual void OnEnable()
    {
        // Setup();
    }

    private void FixedUpdate()
    {
        if(CurrentStateClass != null){CurrentStateClass.OnUpdateState(this);}
    }

    public virtual void Setup()
    {

    }
    public virtual void Activate()
    {
        ActorArtContainer.SetActive(true);
    }
    public virtual void Die()
    {

    }

    public void FlipSpriteCheck(float xValue)
    {
        SpriteRenderer sr = (SpriteRenderer)MainSprite;
        if(xValue > 0){sr.flipX = true; return;}
        if(xValue < 0){sr.flipX = false; return;}
    }

    public virtual void BlinkSprite()
    {
        if(DamageBlinkerCoroutine != null){StopCoroutine(DamageBlinkerCoroutine);}
        MainSprite.material.SetColor("_HitEffectColor", DamageColor);
        DamageBlinkerCoroutine = BlinkCoroutine();
        StartCoroutine(DamageBlinkerCoroutine);
    }

    protected IEnumerator BlinkCoroutine()
    {
        MainSprite.material.SetFloat("_HitEffectBlend", 1);
        for (float i = 1; i > 0; i -= 0.05f)
        {
            MainSprite.material.SetFloat("_HitEffectBlend", i);
            yield return new WaitForSeconds(0.01f);
        }
        

        MainSprite.material.SetFloat("_HitEffectBlend", 0);
    }
}


public abstract class ActorStatesAbstractClass
{
    public abstract string name{get;}
    public abstract void OnEnterState(StageActor _cont);
    public abstract void OnExitState(StageActor _cont);
    public abstract void OnUpdateState(StageActor _cont);
}
