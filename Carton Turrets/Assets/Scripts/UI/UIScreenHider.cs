using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIScreenHider : MonoBehaviour
{

    [SerializeField]DOTweenAnimation _attachedAnimation;

    private void Start()
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    [ContextMenu("In")]
    public void AnimateIn()
    {
        _attachedAnimation.DOPlayForwardById("in");
    }
    [ContextMenu("Out")]
    public void AnimateOut()
    {
        _attachedAnimation.DOPlayForwardById("out");
    }

}
