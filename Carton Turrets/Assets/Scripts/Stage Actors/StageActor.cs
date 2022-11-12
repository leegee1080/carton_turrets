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
    public Color FreezeColor;
    private bool _colorOverride;

    [Header("Animations")]
    public IEnumerator DamageBlinkerCoroutine;
    public AnimationCurve WalkingCurve;
    float _walkingTime = 0;
    public GameObject RendererHandle;

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

    public virtual void RotateSpriteWalkAnimation(float speed = 0, float size = 1, bool reset = false)
    {
        if(reset){_walkingTime = 0; RendererHandle.transform.localRotation = Quaternion.Euler(0,0,0); return;}
        speed = speed/10;
        if(_walkingTime > 2){_walkingTime = 0;}

        RendererHandle.transform.localRotation = Quaternion.Euler(0,0,WalkingCurve.Evaluate(_walkingTime)*(size * 10));
        _walkingTime += speed;
    }

    public void FlipSpriteCheck(float xValue)
    {
        SpriteRenderer sr = (SpriteRenderer)MainSprite;
        if(xValue > 0){sr.flipX = true; return;}
        if(xValue < 0){sr.flipX = false; return;}
    }

    public virtual void BlinkSprite()
    {
        if(_colorOverride){return;}
        if(DamageBlinkerCoroutine != null){StopCoroutine(DamageBlinkerCoroutine);}
        MainSprite.material.SetColor("_HitEffectColor", DamageColor);
        DamageBlinkerCoroutine = BlinkCoroutine();
        StartCoroutine(DamageBlinkerCoroutine);
    }

    public virtual void ChangeSpriteColor(bool resetColor, Color colorToApply)
    {
        if(resetColor)
        {
            if(DamageBlinkerCoroutine != null){StopCoroutine(DamageBlinkerCoroutine);}
            _colorOverride = false;
            MainSprite.material.SetColor("_HitEffectColor", DamageColor);
            MainSprite.material.SetFloat("_HitEffectBlend", 0);
            return;
        }
        _colorOverride = true;
        MainSprite.material.SetColor("_HitEffectColor", colorToApply);
        MainSprite.material.SetFloat("_HitEffectBlend", 1);
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
