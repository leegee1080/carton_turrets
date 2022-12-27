using System.Collections;
using UnityEngine;

public class EndgameController : EnemyActor, IDroppodControllable
{
    [SerializeField]GameObject _artObject;
    [SerializeField]DropPodImpact _bombImpactObject;
    [SerializeField]GameObject _bombKillerCollider;
    [SerializeField]MeshRenderer _bombModel;
    [SerializeField]ParticleSystem _bombStarsPS;
    [SerializeField]ParticleSystem _bombMoneyPS;
    [SerializeField]bool _launched;
    [SerializeField]bool _impacted;


    private void FixedUpdate()
    {
        if(!_launched || _impacted){return;}

        if(Vector3.Distance(_artObject.transform.localPosition, Vector3.zero) > 0.1f)
        {
            float step = CurrentSpeed *Time.deltaTime;
            _artObject.transform.localPosition =  Vector3.MoveTowards(_artObject.transform.localPosition, Vector3.zero, step);
            return;
        }
        AudioController.singleton.PlaySound("endgame_nuke_land");
        _bombImpactObject.Nuke(this.transform.position);
        _impacted = true;
    }

    public void LaunchBomb(Vector3 impactLocation)
    {
        AudioController.singleton.PlaySound("endgame_nuke_alarm");
        this.transform.position = impactLocation;
        _artObject.SetActive(true);
        _launched = true;
    }
    public void Impact(){}//unused as part of Idroppod
    public void CompleteLanding()
    {
        AudioController.singleton.PlaySound("endgame_nuke_explode");
        _bombKillerCollider.SetActive(true);
    }

    public override void TakeDamage(float amt)
    {
        if(!_impacted){return;}
        CurrentHealth -= amt;
        BlinkSprite();
        if(CurrentHealth <=0 )
        {
            StageController.singlton.PlayerWin();
            _bombModel.gameObject.SetActive(false);
            _bombStarsPS.Play();
            _bombMoneyPS.Play();
        }
    }
    public override void Freeze(float time)
    {

    }

    public override void BlinkSprite()
    {
        if(DamageBlinkerCoroutine != null){StopCoroutine(DamageBlinkerCoroutine);}
        _bombModel.material.SetColor("_HitEffectColor", DamageColor);
        DamageBlinkerCoroutine = BlinkMeshCoroutine();
        StartCoroutine(DamageBlinkerCoroutine);
    }
    IEnumerator BlinkMeshCoroutine()
    {
        _bombModel.material.SetFloat("_HitEffectBlend", 1);
        for (float i = 1; i > 0; i -= 0.05f)
        {
            _bombModel.material.SetFloat("_HitEffectBlend", i);
            yield return new WaitForSeconds(0.01f);
        }
        

        _bombModel.material.SetFloat("_HitEffectBlend", 0);
    }
}
