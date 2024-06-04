using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [System.Serializable]
    public class Statistics
    {
        [SerializeField] private Text TxtBullet;
        [SerializeField] private Text TxtWave;
        [SerializeField] private Text TxtWeaponType;

        [SerializeField] private Slider slider_HB;

        public void SetTxtBullet(string value) => TxtBullet.text = $"Bullet : {value}";
        public void SetTxtWave(string value, string value2) => TxtWave.text = $"Wave : {value} / {value2}";
        public void SetTxtWeaponType(string value) => TxtWeaponType.text = $"Type : {value}";
        public void SetSliderHB(int value) => slider_HB.value = value;
        public void SetSliderHBMaxValue(int value) => slider_HB.maxValue = value;

    }
    public Statistics statistics;

    [SerializeField] private GameObject Root;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}
