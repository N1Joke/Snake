using UnityEngine;

public class SnakeInput : MonoBehaviour
{
    [SerializeField] private float _speedZ;
    [SerializeField] private float _hypotenuse;
    [SerializeField] private float _leftBorder;
    [SerializeField] private float _rightBorder;
    [SerializeField] private float _offsetYAtFavir;
    [SerializeField] private float _accelerationAtFavir;

    private Camera _camera;    

    private void Start()
    {        
        _camera = Camera.main;
    }

    public Vector3 GetDiractionToClick(Vector3 headPosition)
    {
        Vector3 directon = _camera.ScreenToViewportPoint(Input.mousePosition);
        _hypotenuse = Mathf.Sqrt(Mathf.Pow((_camera.transform.position.y - headPosition.y), 2) + Mathf.Pow((headPosition.z - _camera.transform.position.z), 2));
        directon.z = _hypotenuse;
        directon = _camera.ViewportToWorldPoint(directon);
        
        directon = new Vector3(Mathf.Clamp(directon.x, _leftBorder, _rightBorder), headPosition.y, headPosition.z + Time.deltaTime * _speedZ);

        return directon;
    }

    public Vector3 GetDiractionAtFavir(Vector3 headPosition)
    {
        Vector3 directon = new Vector3(0, headPosition.y, headPosition.z + Time.deltaTime * _speedZ * _accelerationAtFavir);

        return directon;
    }

    public float GetAccelerationValue()
    {
        return _accelerationAtFavir;
    }
}
