using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Отвечает за контроль инпута кроме движения игрока
/// </summary>
public class PlayerSubmitController : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private string actionMapName = "Main";

    private InputAction iKeyAction;
    private InputAction eKeyAction;
    private InputAction spaceKeyAction;

    private void Awake()
    {
        var keyboardMouseMap = actionAsset.FindActionMap(actionMapName);

        iKeyAction = keyboardMouseMap.FindAction("I");
        eKeyAction = keyboardMouseMap.FindAction("E");
        spaceKeyAction = keyboardMouseMap.FindAction("Space");

        iKeyAction.Enable();
        iKeyAction.performed += OnInventoryKeyPressed;

        eKeyAction.Enable();
        eKeyAction.performed += OnSubmitKeyPressed;

        spaceKeyAction.Enable();
        spaceKeyAction.performed += OnSpaceKeyPressed;

        keyboardMouseMap.Enable();
    }

    private void OnInventoryKeyPressed(InputAction.CallbackContext context)
    {
        Debug.Log("I key pressed");
    }

    private void OnSubmitKeyPressed(InputAction.CallbackContext context)
    {
        EventsBus.Publish(new OnRemoveAllEnemies());
        Debug.Log("E key pressed");
    }

    private void OnSpaceKeyPressed(InputAction.CallbackContext context)
    {
        EventsBus.Publish(new OnEnemySpawn() { prefabName = "EnemyBoy" });

        Debug.Log("Space key pressed");
    }
}
