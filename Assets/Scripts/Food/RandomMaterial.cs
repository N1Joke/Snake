using UnityEngine;

public class RandomMaterial : MonoBehaviour
{
    [SerializeField] private Material[] _material;

    public Material GetRandomMaterial()
    {
        return _material[Random.Range(0, _material.Length)];
    }
}
