using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


[System.Serializable]
public class SceneChangeEvent : UnityEvent<int>{}

 
public class GlobalVolumeController : MonoBehaviour
{
    public static GlobalVolumeController singleton;
    private void Awake()
    {

        if(singleton == null)
        {
            singleton = this;
            return;
        }
        
        Destroy(this.gameObject);
    }

    [SerializeField]public SceneChangeEvent OnSceneTransComplete;

    [SerializeField]Volume _volume;

    [Header("Scene Trans")]
    [SerializeField]float _sceneTime;
    [SerializeField]Ease _easeInType;
    [SerializeField]Ease _easeOutType;
    Vignette _v;
    LensDistortion _ld;
    Tween _sTween;
    float _sTweenValue = 1;
    [SerializeField]int _nextScene =0;


    [Header("Hit Color")]
    [SerializeField]float _hurtTime;
    ColorAdjustments _ca;
    Tween _caTween;
    float _caColorFilter = 1;



    private void Start()
    {
        _volume.profile.TryGet<Vignette>(out _v);
        _volume.profile.TryGet<LensDistortion>(out _ld);
        _volume.profile.TryGet<ColorAdjustments>(out _ca);

        _v.intensity.value = 1;
        _ld.intensity.value = -1;
        _ld.scale.value = 0.01f;
    }

    #region PulseHurt
    [ContextMenu("PulseHurt")]
    public void CharacterHurt()
    {
        _caTween = DOTween.To (() => _caColorFilter,
            x => _caColorFilter = x, 0, _hurtTime);
        _caTween.OnUpdate (UpdateHurtColor);
        _caTween.SetUpdate(true);
        _caTween.OnComplete (CompleteHurtColor);
    }
    public void UpdateHurtColor()
    {
        _ca.colorFilter.value = new Color(1, _caColorFilter, _caColorFilter);
    }
    public void CompleteHurtColor()
    {
        void KillTween()
        {
            _caTween = null;
        }
        _caTween = DOTween.To (() => _caColorFilter,
            x => _caColorFilter = x, 1, _hurtTime).OnUpdate (UpdateHurtColor);
        _caTween.SetUpdate(true);
        _caTween.OnComplete (KillTween);
    }     
    #endregion PulseHurt

    #region NewScene
    public void NewScene(int nextScene)
    {
        _nextScene = nextScene;
        _sTween = DOTween.To (() => _sTweenValue,
            x => _sTweenValue = x, 1, _sceneTime);
        _sTween.OnUpdate (UpdateSceneEffects);
        _sTween.SetUpdate(true);
        _sTween.SetEase(_easeInType);
        _sTween.OnComplete (CompleteSceneIn);
    }
    public void CompleteSceneIn()
    {
        _sTween = null;
        OnSceneTransComplete.Invoke(_nextScene);
        SceneManager.LoadSceneAsync(_nextScene);
    }
    #endregion NewScene

    #region QuitGame
    public void QuitGame()
    {
        _sTween = DOTween.To (() => _sTweenValue,
            x => _sTweenValue = x, 1, _sceneTime);
        _sTween.OnUpdate (UpdateSceneEffects);
        _sTween.SetUpdate(true);
        _sTween.SetEase(_easeInType);
        _sTween.OnComplete (CompleteQuitGame);
    }
    public void CompleteQuitGame()
    {
        _sTween = null;
        Application.Quit();
    }
    #endregion QuitGame

    #region ShowScene
    [ContextMenu("ShowScene")]
    public void ShowScene()
    {
        _sTween = DOTween.To (() => _sTweenValue,
            y => _sTweenValue = y, 0, _sceneTime);
        _sTween.OnUpdate (UpdateSceneEffects);
        _sTween.SetUpdate(true);
        _sTween.SetEase(_easeOutType);
        _sTween.OnComplete (CompleteSceneOut);
    }
    public void CompleteSceneOut()
    {
        _sTween = null;
    }
    #endregion ShowScene


    public void UpdateSceneEffects()
    {
        _v.intensity.value = Mathf.Lerp(0.4f, 1, _sTweenValue);
        _ld.intensity.value = Mathf.Lerp(0,-1, _sTweenValue);
        _ld.scale.value = Mathf.Lerp(1,0.01f, _sTweenValue);
    }
}
