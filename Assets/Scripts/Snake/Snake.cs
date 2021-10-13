using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TailGenerator))]
[RequireComponent(typeof(SnakeInput))]
public class Snake : MonoBehaviour
{
    [SerializeField] private int _tailSize;
    [SerializeField] private SnakeHead _head;
    [SerializeField] private float _speed;
    [SerializeField] private float _tailSpringiness;

    private List<Segment> _tail;
    private TailGenerator _tailGenerator;
    private SnakeInput _input;        

    public event UnityAction<int> SizeUpdated;

    private void Awake()
    {
        _tailGenerator = GetComponent<TailGenerator>();
        _input = GetComponent<SnakeInput>();
        _tail = _tailGenerator.Genearate(_tailSize);

        SizeUpdated?.Invoke(_tail.Count);
    }

    private void Start()
    {
        SizeUpdated?.Invoke(_tail.Count);
    }

    private void OnEnable()
    {
        _head.FoodCollected += OnFoodCollided;
        _head.BonusCollected += OnBonusCollected;
    }

    private void OnDisable()
    {
        _head.FoodCollected -= OnFoodCollided;
        _head.BonusCollected -= OnBonusCollected;
    }

    private void FixedUpdate()
    {
        Move(_head.transform.position + _head.transform.forward * _speed * Time.fixedDeltaTime);

        _head.transform.position = _input.GetDiractionToClick(_head.transform.position);
    }

    private void Move(Vector3 nextPosition)
    {
        Vector3 preveousPosition = _head.transform.position;

        foreach (var segment in _tail)
        {
            Vector3 tempPosition = segment.transform.position;
            segment.transform.position = Vector3.Lerp(segment.transform.position, preveousPosition, _tailSpringiness * Time.deltaTime);
            preveousPosition = tempPosition;
        }

        _head.Move(nextPosition);
    }

    private void OnBonusCollected()
    {
        Debug.Log("Bonus collected");
    }
    private void OnFoodCollided(int foodSize)
    {
        _tail.AddRange(_tailGenerator.Genearate(foodSize));
        SizeUpdated?.Invoke(_tail.Count);
    }
}
