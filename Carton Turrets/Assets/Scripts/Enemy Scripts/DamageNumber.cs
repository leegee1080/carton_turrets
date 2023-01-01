using System.Collections;
using UnityEngine;
using TMPro;


public class DamageAmountStorage: IPassableObject
{
    public float damage;
    public DamageAmountStorage(float amount)
    {
        damage = amount;
    }

}

public class DamageNumber : MonoBehaviour
{
    [SerializeField] string _damageAmount;
    [SerializeField] TMP_Text _text;
    IEnumerator _aniCR;

    public virtual void Activate(IPassableObject obj)
    {
        DamageAmountStorage d = (DamageAmountStorage)obj;
        _damageAmount = d.damage.ToString();
        _text.text = _damageAmount;
        

        if(_aniCR != null){StopCoroutine(_aniCR);}
        _aniCR = Animation();
        StartCoroutine(_aniCR);
    }

    private IEnumerator Animation()
    {
        gameObject.transform.localPosition = Vector3.zero;

        float timer = 1;
        float stepTime = 0.01f;
        for (float i = 0; i < timer; i += stepTime)
        {
            gameObject.transform.position += (Vector3.up/100);
            yield return new WaitForSeconds(stepTime);
        }
        hideText();
    }

    void hideText()
    {
        this.gameObject.SetActive(false);
    }
}
