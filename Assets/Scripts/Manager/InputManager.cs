
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControllType { Type_A, Type_B, Type_C };

public class InputManager : MonoBehaviour
{
    
    #region Singleton
    public static InputManager Instance;
    #endregion

    #region Private Field
    private PlayerActionMap playerActionMap;

    private Vector2 moveInput;
    private Vector2 axisInput;

    private Vector3 playerSelectedPosition;
    private Vector3 AimPos;

    private ControllType controllType;

    private Transform playerTransform;

    #endregion

    public delegate void OnInputKey();
    public event OnInputKey OnFire;
    public event OnInputKey OnChangeGun;

    public delegate void OnInputAxis(Vector2 moveInput);
    public event OnInputAxis OnMoveInput;
    public event OnInputAxis OnMouseMove;

    #region Mono Func
    private void Awake()
    {
        Instance = this;
        playerActionMap = new PlayerActionMap();
    }
    private void OnEnable() => playerActionMap.Enable();
    private void OnDisable() => playerActionMap.Disable();

    void Update()
    {
        moveInput = playerActionMap.Player.Move.ReadValue<Vector2>();
        SelectedPosition();

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (OnMoveInput != null)
            OnMoveInput(moveInput);
        if (OnMouseMove != null)
        {
            OnMouseMove(new Vector2(AimPos.x, AimPos.z));
        }

        if (Mouse.current.leftButton.IsPressed())
            if (OnFire != null)
                OnFire();

        if (Keyboard.current[Key.Tab].wasPressedThisFrame)
            if (OnChangeGun != null)
                OnChangeGun();
    }

    // Get select position on world
    private Vector3 SelectedPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit))
        {
            AimPos = hit.point;
        }
        return AimPos;
    }
    #endregion
    
}
