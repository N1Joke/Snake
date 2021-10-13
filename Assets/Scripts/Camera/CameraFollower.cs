using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _offsetZ = -2;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _player.position.z - _offsetZ);
    }
}
