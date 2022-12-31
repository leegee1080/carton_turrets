using UnityEngine;

public class ScoreObject : MonoBehaviour
{
    [SerializeField]ParticleSystem _ps;
    [SerializeField]int _score;
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.transform.localPosition = Vector3.zero;
        other.gameObject.SetActive(false);
        _ps.Play();
        StageMoneyEarnedIndicatorUI.singlton.UpdateMoneyAmountUI(_score);

        if(_score <= 0){ AudioController.singleton.PlaySound("ui_coin_lose");}
    }
}
