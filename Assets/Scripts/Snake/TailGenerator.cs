using System.Collections.Generic;
using UnityEngine;

public class TailGenerator : MonoBehaviour
{
    [SerializeField] Segment _segmentTemplate;

    public List<Segment> Genearate(int count, Color color)
    {
        List<Segment> tail = new List<Segment>();

        for (int i = 0; i < count; i++)
        {
            Segment segment = Instantiate(_segmentTemplate, transform);
            segment.gameObject.GetComponent<Renderer>().material.color = color;
            tail.Add(segment);
        }

        return tail;
    }
}
