using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatPauseMenu : MonoBehaviour
{
    [field: SerializeField]public PlayerStatEnum _upgradeType {get; private set;}
    // [SerializeField]TMP_Text _tierText;
    // [SerializeField]TMP_Text _upgradeNameText;
    [SerializeField]TMP_Text _statAmountText;
    // [SerializeField]SpriteRenderer _upgradeSpriteRenderer;

    public void UpdateStatBlock(string amount)
    {
        _statAmountText.text = amount;
    }
}
