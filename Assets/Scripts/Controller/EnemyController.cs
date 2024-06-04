using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum Type {None,TypeA, TypeB, TypeC }

    [SerializeField] private Type type;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Color Color;

    [SerializeField] private float Speed = 5;

    [SerializeField] private int DamagePower;

    private HealthController healthController;

    private void Start()
    {
        SetColor(Color);
        healthController = GetComponent<HealthController>();
    }

    void Update()
    {
        if (type == Type.None) return;
        Movment();
    }
    private void Movment()
    {
        transform.Translate(Vector3.back * Time.deltaTime * Speed);

        if (transform.position.z <= -30)
        {
            healthController.Dai();
            PlayerController.Instance.GetComponent<HealthController>().SetDamage(DamagePower);
        }
    }
    private void SetColor(Color color)
    {
        if(meshRenderer == null)
        {
            Debug.LogWarning("Can't Set Color :meshRenderer == null ");
            return;
        }
        Material material = meshRenderer.material;
        material.SetColor("_Color", color);
        meshRenderer.material = material;
    }
}
