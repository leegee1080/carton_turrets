using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public void PlaySound(string soundName)
    {
        AudioController.singleton.PlaySound(soundName);
    }
}
