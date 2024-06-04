using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private float Speed = 4;
    [SerializeField] private float MaxMovmentDirection = 9;
    [SerializeField] private float aimHightValue = 2;
    private float animationMovmentValue;

    [SerializeField] private Transform AimPos;
    [SerializeField] private Transform WeaponPosition;

    [SerializeField] private WeaponController weaponPrefab;
    private WeaponController weaponController;

    private Animator animator;

    private bool deavtive;

    private string animationMovmentName = "Movment";

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InputManager.Instance.OnMoveInput  += OnMove;
        InputManager.Instance.OnMouseMove += OnMouseMove;
        InputManager.Instance.OnFire += OnFire;
        InputManager.Instance.OnChangeGun += OnChangeGun;

        animator = GetComponent<Animator>();

        weaponController = Instantiate(weaponPrefab, WeaponPosition);
        weaponController.transform.localPosition = Vector3.zero;
        weaponController.transform.localEulerAngles = Vector3.zero;
        weaponController.Init(AimPos);

        GameManager.Instance.onEndGame += OnEndGame;
    }

    void Update()
    {
        
    }

    private void OnMove(Vector2 input)
    {
        var XValue = input.x;
        if (transform.position.x >= MaxMovmentDirection)
            XValue = input.x > 0 ? 0 : input.x;
        if (transform.position.x <= -MaxMovmentDirection)
            XValue = input.x < 0 ? 0 : input.x;

        transform.Translate(new Vector3(XValue, 0, 0) * Time.deltaTime * Speed);
        animationController(XValue);
    }
    private void OnMouseMove(Vector2 input)
    {
        float ZPos = input.y;
        ZPos = Mathf.Clamp(ZPos,-23,1000);
        AimPos.position = new Vector3(input.x, aimHightValue, ZPos);
    }
    private void OnFire() => weaponController.Shoot();
    private void OnChangeGun() => weaponController.ChangeGun();
    private void animationController(float XValue)
    {
        if (animator == null)
        {
            Debug.LogWarning("animation Controller : animator == null");
            return;
        }

        animationMovmentValue = Mathf.MoveTowards(animationMovmentValue, XValue, Time.deltaTime * Speed);
        animator.SetFloat(animationMovmentName, animationMovmentValue);
    }

    private void OnEndGame()
    {
        deavtive = true;
        gameObject.SetActive(false);
    }
}
