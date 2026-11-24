using UnityEngine;

// My camera follows 8 m behind the player, so the ears go on the player instead (Lecture 10):
// heard from where the player is, facing the way the camera faces.
public class ListenerOnPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform cameraTransform;

    void LateUpdate() => transform.SetPositionAndRotation(player.position, cameraTransform.rotation);
}
