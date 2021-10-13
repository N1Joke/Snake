using System.Collections.Generic;
using UnityEngine;

public class TailGenerator : MonoBehaviour
{
    [SerializeField] Segment _segmentTemplate;

    public List<Segment> Genearate(int count)
    {
        List<Segment> tail = new List<Segment>();

        for (int i = 0; i < count; i++)
        {
            tail.Add(Instantiate(_segmentTemplate, transform));
        }

        return tail;
    }
}
