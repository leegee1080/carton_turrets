using UnityEngine;
using TMPro;
using DG.Tweening;


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
    DOTweenAnimation _ani;
    [SerializeField] Ease _aniEaseType;

    public virtual void Activate(IPassableObject obj)
    {
        DamageAmountStorage d = (DamageAmountStorage)obj;
        _damageAmount = d.damage.ToString();
        _text.text = _damageAmount;
        gameObject.transform.position = Vector3.zero;

        gameObject.transform.DOMoveY(1,1).SetEase(_aniEaseType).OnComplete(hideText);

        void hideText()
        {
            this.gameObject.SetActive(false);
        }
    }
}
