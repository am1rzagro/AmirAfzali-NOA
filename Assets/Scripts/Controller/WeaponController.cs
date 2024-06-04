using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum Type {Gun_A,Gun_B,Gun_C }

    [System.Serializable]
    public class Parameter
    {
        [SerializeField] private Type _type;
        [SerializeField] private float FireSpeed;
        [SerializeField] private float CoolingSpeed;
        [SerializeField] private float HotSpeed;

        [SerializeField] private int damage = 10;
        [SerializeField] private int MaxBullet;
        [SerializeField] private int AllBullets;

        private const float hightOffset = 1.5f;

        private Transform spawnPoint;
        private Transform aimPoint;

        private float Timer;
        public Type type { get { return _type; } private set { _type = value; } }

        public Transform SpawnPoint { set { spawnPoint = value; } }
        public Transform AimPoint { set { aimPoint = value; } }

        private void AddBullet(int value) => AllBullets = (value + AllBullets) > MaxBullet ? MaxBullet : (AllBullets + value);

        public void Selected()
        {
            Timer = FireSpeed;
            HUDManager.Instance.statistics.SetTxtWeaponType(type.ToString());
            HUDManager.Instance.statistics.SetTxtBullet(AllBullets.ToString());
        }
        public void Shoot()
        {
            HUDManager.Instance.statistics.SetTxtBullet(AllBullets.ToString());

            if (CanShoot() == false)
                return;

            spawnPoint.transform.LookAt( new Vector3 (aimPoint.position.x, hightOffset, aimPoint.position.z));
            VFXManager.Instance.GunTail.Play(spawnPoint.position, spawnPoint.rotation);
            

            Timer = 0;
            AllBullets -= 1;

            Vector3 direction = spawnPoint.forward;//new Vector3(aimPoint.position.x, hightOffset, aimPoint.position.z)  - spawnPoint.position;
            RaycastHit hit;

            if (Physics.Raycast(spawnPoint.position, direction, out hit))
            {
                Debug.DrawRay(spawnPoint.position, direction, Color.green);

                AmmoboxController ammobox = hit.collider.gameObject.GetComponent<AmmoboxController>();
                if (ammobox)
                    AddBullet(ammobox.GetAmmo);

                HealthController healthController = hit.collider.gameObject.GetComponent<HealthController>();
                if (healthController)
                    healthController.SetDamage(damage);
            }
            else
            {
                Debug.DrawRay(spawnPoint.position, direction, Color.red);
                Debug.Log("Raycast did not hit any object.");
            }

        }
        private bool CanShoot()
        {
            Timer += Time.deltaTime;
            if (Timer < FireSpeed)
                return false;
            if (AllBullets <= 0)
                return false;

            return true;
        }
    }

    public Parameter[] Parameters ;

    [SerializeField] private Transform SpawnPointBullt;

    private int GunIndex = -1;

    public void Init(Transform aimPos)
    {
        foreach(Parameter prm in Parameters)
        {
            prm.SpawnPoint = SpawnPointBullt;
            prm.AimPoint = aimPos;
        }
        ChangeGun();
    }

    public void Shoot() => Parameters[GunIndex].Shoot();

    public void ChangeGun()
    {
        GunIndex++;
        if (GunIndex > Parameters.Length - 1)
            GunIndex = 0;

        Parameters[GunIndex].Selected();
    }


}
