using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour
{
    [SerializeField]Button[] _buttonArray;

    public void DisableButtons()
    {
        foreach (Button item in _buttonArray)
        {
            item.interactable=false;
        }
    }
    public void EnableButtons()
    {
        foreach (Button item in _buttonArray)
        {
            item.interactable=true;
        }

    }
}
