using UnityEngine;

public class CameraConstantWidth : MonoBehaviour
{
    [SerializeField] private Vector2 _DefaultResolution = new Vector2(720, 1280);
    [Range(0f, 1f)] public float _WidthOrHeight = 0;

    private Camera _componentCamera;

    private float _initialSize;
    private float _targetAspect;

    private float _initialFov;
    private float _horizontalFov = 120f;

    private void Start()
    {
        _componentCamera = GetComponent<Camera>();
        _initialSize = _componentCamera.orthographicSize;

        _targetAspect = _DefaultResolution.x / _DefaultResolution.y;

        _initialFov = _componentCamera.fieldOfView;
        _horizontalFov = CalcVerticalFov(_initialFov, 1 / _targetAspect);
    }

    private void Update()
    {
        if (_componentCamera.orthographic)
        {
            float constantWidthSize = _initialSize * (_targetAspect / _componentCamera.aspect);
            _componentCamera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, _WidthOrHeight);
        }
        else
        {
            float constantWidthFov = CalcVerticalFov(_horizontalFov, _componentCamera.aspect);
            _componentCamera.fieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, _WidthOrHeight);
        }
    }

    private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
    {
        float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

        float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

        return vFovInRads * Mathf.Rad2Deg;
    }
}
