using UnityEngine;
using UnityEngine.InputSystem;

public class HammerToggle : MonoBehaviour
{
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private GameObject hammer;

    private void OnEnable()
    {
        triggerAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.Disable();
    }

    private void Update()
    {
        hammer.SetActive(triggerAction.action.IsPressed());
    }
}
