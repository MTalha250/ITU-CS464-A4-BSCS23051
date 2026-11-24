using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 1;
    [SerializeField] float spinSpeed = 90f;
    [SerializeField] AudioClip chime;

    void Update() => transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;   // the built-in Player tag, case-sensitive
        GameManager.Instance.AddScore(value);
        SfxPlayer.Instance.Play(chime);        // the SfxPlayer plays it, so it isn't cut off with the coin
        Destroy(gameObject);
    }
}
