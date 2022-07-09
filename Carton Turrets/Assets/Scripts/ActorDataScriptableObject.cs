using UnityEngine;

public class ActorDataScriptableObject : ScriptableObject
{
    [Header("Character Text")]
    public new string name;
    public string Desc;

    [Header("Character Vars")]
    public int MaxHealth;
    public float MaxSpeed;

    // [Header("Character Art")]
    // public Sprite Icon;
}
