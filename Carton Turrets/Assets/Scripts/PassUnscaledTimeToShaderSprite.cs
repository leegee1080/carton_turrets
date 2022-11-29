using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class PassUnscaledTimeToShaderSprite : MonoBehaviour {
    private SpriteRenderer rend;
    private string originalMatName;
    private static readonly int UnscaledTime = Shader.PropertyToID("globalUnscaledTime");
 
    private void Awake() {
        rend = GetComponent<SpriteRenderer>(); //get sprite component
        originalMatName = rend.material.name; //cache original material name
    }
 
    void Update() {
        rend.material.SetFloat("globalUnscaledTime", Time.unscaledTime);
        // if (rend.material.HasProperty(UnscaledTime)) rend.material.SetFloat(UnscaledTime, Time.unscaledTime);
        // else Destroy(this); //Remove if material has no matching property
        // rend.material = Instantiate(rend.material) ;//Force mask stencils to update
        // rend.material.name = originalMatName; //ensure that (Clone) isn't appended to the end
    }
}
