using UnityEngine;

public class Bumper : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioController.singleton.PlaySound("ui_bounce");
    }
}
