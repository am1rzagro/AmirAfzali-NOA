using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoboxController : MonoBehaviour
{
    [SerializeField] private float RotateSpeed = 10;

    [SerializeField] private Transform RotateObject;

    [SerializeField] private TextMesh TxtAmmoValue;

    private int AmmoValue;


    void Start()
    {
        AmmoValue = Random.Range(20, 1000);
        TxtAmmoValue.text = $"+{AmmoValue}";
    }
    void Update()
    {
        RotateObject.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
    }

    public int GetAmmo => AmmoValue;
}
