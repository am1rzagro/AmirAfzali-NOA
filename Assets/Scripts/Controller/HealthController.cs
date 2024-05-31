using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public enum Type { ammobox,Enemy,player}
    [SerializeField] private Type type;

    [SerializeField] private int Health;
    private int StartHealt;

    public void ResetDamage() => Health = StartHealt;
    public void GetDamage(int Power)
    {
        Debug.Log("GetDamage "+ Power);
        StartHealt = StartHealt == 0 ? Health : StartHealt;
        Health = Health - Power;
        if (Health <= 0)
        {
            Health = 0;
            Dai();
        }
    }

    public void Dai()
    {
        switch (type)
        {
            case Type.ammobox:
                LevelController.Instance.ammo.SetDeactiveItem(gameObject);
                break;
            case Type.Enemy:
                LevelController.Instance.wave.DeactiveEnemyFromWave(gameObject);
                break;
        }
    }
}
