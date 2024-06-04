using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public class VFX 
    {
        [SerializeField] private ParticleSystem particle;

        private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

        public void Play(Vector3 Pos,Quaternion rotation)
        {
            if (CanUseOfList(Pos,rotation))
                return;

            NewInstansiate(Pos, rotation);
            return;
        }

        private void NewInstansiate(Vector3 Pos, Quaternion rotation)
        {
            ParticleSystem prt = Instantiate(particle,Pos,rotation);
            particleSystems.Add(prt);

            prt.Emit(1);
        }

        private bool CanUseOfList(Vector3 Pos, Quaternion rotation)
        {
            if (particleSystems.Count == 0)
                return false;
            for (int i = 0; i < particleSystems.Count; i++)
                if (particleSystems[i].isPlaying == false)
                {
                    particleSystems[i].gameObject.transform.position = Pos;
                    particleSystems[i].gameObject.transform.rotation = rotation;
                    particleSystems[i].Emit(1);

                    return true;
                }
            return false;
        }
    }

    public VFX GunTail;

}
