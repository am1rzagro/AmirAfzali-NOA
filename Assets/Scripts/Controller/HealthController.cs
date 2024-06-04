using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public enum Type { ammobox,Enemy,player}
    [SerializeField] private Type type;

    [SerializeField] private int Health;
    private int StartHealt;

    private void Start()
    {
        SetHBMaxValue();
        ShowHB();
    }

    public void ResetDamage() => Health = StartHealt;
    public void SetDamage(int Power)
    {
        if (Health == 0) return;

        StartHealt = StartHealt == 0 ? Health : StartHealt;
        Health = Health - Power;
        ShowHB();
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
            case Type.player:
                GameManager.Instance.SetLoseGame();
                break;
        }
    }

    private void ShowHB()
    {
        switch (type)
        {
            case Type.player:
                HUDManager.Instance.statistics.SetSliderHB(Health);
                break;
        }
    }

    private void SetHBMaxValue()
    {
        switch (type)
        {
            case Type.player:
                HUDManager.Instance.statistics.SetSliderHBMaxValue(Health);
                break;
        }
    }
}
