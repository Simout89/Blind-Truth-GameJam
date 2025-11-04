using UnityEngine;
using UnityEngine.Events;

public class LevelTrigger : MonoBehaviour
{
    public UnityEvent OnEnterEvent;
    public UnityEvent OnExitEvent;

    private string _player = "Player";

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == _player)
            OnEnterEvent?.Invoke();
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == _player)
            OnExitEvent?.Invoke();
    }
}