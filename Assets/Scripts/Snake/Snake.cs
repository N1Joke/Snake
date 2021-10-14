using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(TailGenerator))]
[RequireComponent(typeof(SnakeInput))]
public class Snake : MonoBehaviour
{
    [SerializeField] private GameObject _bonusUI;
    [SerializeField] private int _tailSize;
    [SerializeField] private SnakeHead _head;
    [SerializeField] private float _speed;
    [SerializeField] private float _tailSpringiness;
    [SerializeField] private Text _textCountBonus;
    [SerializeField] private Text _textCurrentLevel;
    [SerializeField] private Text _tailCount;
    [SerializeField] private float _durationScaleShake;
    [SerializeField] private float _strengthScaleShake;

    private List<Segment> _tail;
    private TailGenerator _tailGenerator;
    private SnakeInput _input;        
    private int _countBonus;
    private int _currentLevel;
    private Renderer _headRenderer;

    public event UnityAction<int> SizeUpdated;
    public event UnityAction TailIsEmpty;

    private void Awake()
    {
        _headRenderer = _head.GetComponent<Renderer>();

        _tailGenerator = GetComponent<TailGenerator>();
        _input = GetComponent<SnakeInput>();
        _tail = _tailGenerator.Genearate(_tailSize, _headRenderer.material.color);

        SizeUpdated?.Invoke(_tail.Count);
    }

    private void Start()
    {     
        SetUpStartSettings();
        SizeUpdated?.Invoke(_tail.Count);
    }

    private void SetUpStartSettings()
    {
        _currentLevel = PlayerPrefs.GetInt("level");
        if (_currentLevel == 0)
            _currentLevel++;
        _textCurrentLevel.text = "LEVEL " + _currentLevel.ToString();
    }

    private void OnEnable()
    {
        _head.FoodCollected += OnFoodCollided;
        _head.BonusCollected += OnBonusCollected;
        _head.ColorSwitched += OnColorSwitched;        
    }

    private void OnDisable()
    {
        _head.FoodCollected -= OnFoodCollided;
        _head.BonusCollected -= OnBonusCollected;
        _head.ColorSwitched -= OnColorSwitched;        
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

    private void OnColorSwitched(Color color)
    {
        _headRenderer.material.color = color;
        foreach (Segment segment in _tail)
        {
            segment.GetComponent<Renderer>().material.color = color;
        }
    }

    private void OnBonusCollected()
    {
        _countBonus++;
        _textCountBonus.text = _countBonus.ToString();

        ShakeUIElement(_bonusUI);
    }

    private void OnFoodCollided(int foodSize, Color color)
    {
        if (color == _headRenderer.material.color)
        {
            _tail.AddRange(_tailGenerator.Genearate(foodSize, _headRenderer.material.color));
            SizeUpdated?.Invoke(_tail.Count);            
        }
        else
        {
            if (_tail.Count != 0)
            {
                Segment deletedSegment = _tail[_tail.Count - 1];
                _tail.Remove(deletedSegment);
                Destroy(deletedSegment.gameObject);

                SizeUpdated?.Invoke(_tail.Count);
            }
            else
            {
                TailIsEmpty?.Invoke();
            }
        }
        _tailCount.text = "TAIL: " + _tail.Count.ToString();

        ShakeUIElement(_tailCount.gameObject);
    }

    private void ShakeUIElement(GameObject gameObject)
    {
        gameObject.GetComponent<RectTransform>().DOShakeScale(_durationScaleShake, _strengthScaleShake);
    }
}
