using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnakeHead : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public event UnityAction DeathBlockCollided;
    public event UnityAction BonusCollected;
    public event UnityAction<int> FoodCollected;

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
            FoodCollected?.Invoke(1);
        }
        else if (other.gameObject.TryGetComponent(out Bonus bonus))
        {
            BonusCollected?.Invoke();
        }
        else if (other.gameObject.TryGetComponent(out DeathTrigger deathTrigger))
        {
            DeathBlockCollided?.Invoke();
            Time.timeScale = 0;
        }
    }
}
