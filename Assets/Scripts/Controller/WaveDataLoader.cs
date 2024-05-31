using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public class WaveDataLoader : MonoBehaviour
{
    private string url = "http://www.profc.ir/users/shooterWaves";
    public List<List<EnemyData>> allEnemies = new List<List<EnemyData>>();

    public delegate void OnGetComplateData(List<List<EnemyData>> data);
    public OnGetComplateData OnComplateData;

    void Start()
    {
        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                WaveResponse waveResponse = JsonUtility.FromJson<WaveResponse>(json);

                foreach (var wave in waveResponse.data.waves)
                {
                    List<EnemyData> enemiesInWave = new List<EnemyData>();
                    foreach (var enemy in wave.enemy)
                    {
                        enemiesInWave.Add(enemy);
                    }
                    allEnemies.Add(enemiesInWave);
                }


                if (OnComplateData != null)
                    OnComplateData(allEnemies);
            }
        }
    }
}

[System.Serializable]
public class EnemyData
{
    public int type;
    public int count;
}

[System.Serializable]
public class WaveData
{
    public List<EnemyData> enemy;
}

[System.Serializable]
public class WaveResponse
{
    public int ok;
    public WaveDataList data;
}

[System.Serializable]
public class WaveDataList
{
    public List<WaveData> waves;
}