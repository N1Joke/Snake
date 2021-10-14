using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeHead : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public event UnityAction DeathBlockCollided;
    public event UnityAction BonusCollected;
    public event UnityAction<int,Color> FoodCollected;
    public event UnityAction<Color> ColorSwitched;
    public event UnityAction FinishColided;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 newPosition)
    {
        _rigidbody.MovePosition(newPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Food food))
        {
            FoodCollected?.Invoke(1, food.GetComponent<Renderer>().material.color);
            food.gameObject.SetActive(false);

        }
        else if (other.gameObject.TryGetComponent(out Bonus bonus))
        {
            BonusCollected?.Invoke();
            bonus.gameObject.SetActive(false);
        }
        else if (other.gameObject.TryGetComponent(out DeathTrigger deathTrigger))
        {
            DeathBlockCollided?.Invoke();
        }
        else if(other.TryGetComponent(out ColorSwitcher colorSwitcher))
        {
            ColorSwitched?.Invoke(colorSwitcher._currentColor);
        }
        else if (other.TryGetComponent(out Finish finish))
        {
            FinishColided?.Invoke();
        }
    }
}
