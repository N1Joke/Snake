using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class SnakeHead : MonoBehaviour
{
    [SerializeField] private float _durationVacum;

    private Rigidbody _rigidbody;

    public event UnityAction DeathBlockCollided;
    public event UnityAction BonusCollected;
    public event UnityAction<int, Color> FoodCollected;
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
        if (other.gameObject.TryGetComponent(out Food food) && !food._eaten)
        {
            food._eaten = true;
            FoodCollected?.Invoke(1, food.GetComponent<Renderer>().material.color);
            StartCoroutine(ActivateVacuum(food.gameObject));
        }
        else if (other.gameObject.TryGetComponent(out Bonus bonus) && !bonus._eaten)
        {
            bonus._eaten = true;
            BonusCollected?.Invoke();
            StartCoroutine(ActivateVacuum(bonus.gameObject));
        }
        else if (other.gameObject.TryGetComponent(out DeathTrigger deathTrigger))
        {
            deathTrigger.gameObject.SetActive(false);
            DeathBlockCollided?.Invoke();
        }
        else if (other.TryGetComponent(out ColorSwitcher colorSwitcher))
        {
            ColorSwitched?.Invoke(colorSwitcher._currentColor);
        }
        else if (other.TryGetComponent(out Finish finish))
        {
            FinishColided?.Invoke();
        }
    }

    private IEnumerator ActivateVacuum(GameObject gameObject)
    {
        gameObject.transform.DOMove(transform.position, _durationVacum);
        yield return new WaitForSeconds(_durationVacum);
        gameObject.SetActive(false);
    }
}
