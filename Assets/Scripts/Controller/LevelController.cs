using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public interface levelItem
    {
        public void Init();
        public void InternalUpdate();
        public void Clear();
    }

    [System.Serializable]
    public class Wave : levelItem
    {
        [SerializeField] private WaveDataLoader dataLoader;
        [SerializeField] private int WaveIndex;

        [SerializeField] private Transform[] spawnPointList;

        [SerializeField] private List<ObjectPooling> waves = new List<ObjectPooling>();

        [SerializeField] private GameObject EnemyPrefab_A, EnemyPrefab_B, EnemyPrefab_C;

        private List<List<EnemyData>> allEnemies = new List<List<EnemyData>>();

        public void Init()
        {
            dataLoader.GetData();
            dataLoader.OnComplateData += OnComplateData;
        }
        public void InternalUpdate()
        {
            if (waves.Count == 0)
                return;

            WaveIndex = GetWaveIndex();
            if (WinGame())
                return;

            waves[WaveIndex].InternalUpdate();
        }
        public void Clear()
        {
            foreach (ObjectPooling pooling in waves)
                pooling.Clear();

            waves = new List<ObjectPooling>();
            allEnemies = new List<List<EnemyData>>();
        }

        public void DeactiveEnemyFromWave(GameObject value)
        {
            if (waves.Count < WaveIndex)
                return;

            waves[WaveIndex].SetDeactiveItem(value);
        }
        private bool WinGame()
        {
            if(WaveIndex >= waves.Count)
            {
                GameManager.Instance.SetWinGame();
                return true;
            }
            return false;
        }
        private int GetWaveIndex()
        {
            int result = 0;
            for (int i = 0; i< waves.Count; i++)
                result = waves[i].Complate ? result + 1 : result;

            HUDManager.Instance.statistics.SetTxtWave(result.ToString(), waves.Count.ToString());
            return result;
        }

        private void OnComplateData(List<List<EnemyData>> datas)
        {
            allEnemies = datas;

            foreach (var wave in allEnemies)
            {
                foreach (var enemy in wave)
                {
                    ObjectPooling pooling = new ObjectPooling();
                    pooling.MaxCreateThis = enemy.count;
                    pooling.TimeAdd = 1;
                    pooling.MaxActive = 3;
                    pooling.SpawnPointList = spawnPointList;
                    pooling.Prefab = GetCurrentPrefab(enemy.type);
                    waves.Add(pooling);
                }
            }
        }

        private GameObject GetCurrentPrefab(int ID)
        {
            switch(ID)
            {
                case 1:
                    return EnemyPrefab_A;
                case 2:
                    return EnemyPrefab_B;
                case 3:
                    return EnemyPrefab_C;
            }
            return EnemyPrefab_A;
        }
    }
    public Wave wave;

    [System.Serializable]
    public class ObjectPooling : levelItem
    {
        [System.Serializable]
        public struct activeItem
        {
            public GameObject Object;
            public Transform SpawnPoint;
        }

        [SerializeField] private Transform[] spawnPointList;

        [SerializeField] private GameObject prefab;

        [SerializeField] private int maxActive;
        [SerializeField] private int maxCreateThis;
        private int countCreate;
        private int countKill;

        [SerializeField] private float timeAdd = 10;
        private float timer;

        private List<activeItem> ActiveList = new List<activeItem>();
        private List<GameObject> DeactiveList = new List<GameObject>();
        private List<GameObject> AllItemsList = new List<GameObject>();

        public Transform[] SpawnPointList { set { spawnPointList = value; } }
        public GameObject Prefab { set { prefab = value; } }
        public int MaxActive { set { maxActive = value; } }
        public int MaxCreateThis { set { maxCreateThis = value; } }
        public float TimeAdd { set { timeAdd = value; } }

        public void Init()
        {

        }

        public void InternalUpdate()
        {
            timer += Time.deltaTime;
            if (DeactiveList.Count == 0)
            {
                NewInstantiate();
            }
            else
            {
                if (CanAddNewItem() == false)
                    return;

                timer = 0;
                countCreate++;

                var Obj = DeactiveList[0];
                var Pos = GetPos();
                if (Pos == null)
                    return;

                Obj.transform.position = Pos.position;
                DeactiveList.RemoveAt(0);

                activeItem item = new activeItem();
                item.Object = Obj;
                item.SpawnPoint = Pos;
                item.Object.SetActive(true);
                item.Object.GetComponent<HealthController>().ResetDamage();
                ActiveList.Add(item);
            }
        }

        public void Clear()
        {
            foreach (GameObject obj in AllItemsList)
                if(obj != null)
                    Destroy(obj.gameObject);

            AllItemsList = new List<GameObject>();
            DeactiveList = new List<GameObject>();
            ActiveList = new List<activeItem>();
        }

        public void SetDeactiveItem(GameObject gameObject)
        {
            gameObject.SetActive(false);
            countKill++;
            for (int i = 0; i< ActiveList.Count; i++)
                if(ActiveList[i].Object == gameObject)
                    ActiveList.RemoveAt(i);

            if (!DeactiveList.Contains(gameObject))
                DeactiveList.Add(gameObject);
        }


        private void NewInstantiate()
        {
            if (CanAddNewItem() == false)
                return;

            timer = 0;
            countCreate++;

            var instPos = GetPos();
            if (instPos == null)
                return;

            var inst = Instantiate(prefab, instPos.position, Quaternion.identity);

            activeItem item = new activeItem();
            item.Object = inst;
            item.SpawnPoint = instPos;
            ActiveList.Add(item);
            AllItemsList.Add(inst);
        }
        private Transform GetPos()
        {
            Transform Result = null;
            int countTry = 0;
            while (Result == null)
            {
                if (countTry >= 10)
                    return null;

                countTry++;

                var Pos = spawnPointList[Random.Range(0, spawnPointList.Length - 1)];
                Result = Pos;
                for (int i = 0; i < ActiveList.Count; i++)
                    if (ActiveList[i].SpawnPoint == Pos)
                    {
                        Result = null;
                        break;
                    }

            }
            return Result;
        }
        private bool CanAddNewItem()
        {
            if (StopAdd)
                return false;
            if (timer < timeAdd)
                return false;
            if (ActiveList.Count >= maxActive)
            {
                timer = 0;
                return false;
            }
            return true;
        }
        public bool StopAdd => maxCreateThis > 0 && countCreate >= maxCreateThis;
        public bool Complate => maxCreateThis > 0 && countKill >= maxCreateThis;

    }
    public ObjectPooling ammo;

    private List<levelItem> levelItems = new List<levelItem>();

    private bool Deactive = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.onEndGame += OnEndGame;
        GameManager.Instance.onStartGame += OnStartGame;
    }

    void Update()
    {
        if (Deactive)
            return;

        for (int i = 0; i < levelItems.Count; i++)
            levelItems[i].InternalUpdate();
    }

    private void OnEndGame()
    {
        Deactive = true;
        Clear();
    }

    private void Clear()
    {
        foreach (levelItem item in levelItems)
            item.Clear();
    }

    public void OnStartGame()
    {
        Clear();

        Deactive = false;
        levelItems = new List<levelItem>();
        levelItems.Add(ammo);
        levelItems.Add(wave);
        foreach (levelItem item in levelItems)
            item.Init();

    }

}
