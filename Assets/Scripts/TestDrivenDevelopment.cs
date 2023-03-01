using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestDrivenDevelopment : Singleton<TestDrivenDevelopment>
{
    public GameObject targetPrefab;
    public GameObject slingshot;
    public int prefabCount;
    public float furtherPrefabSpawnPoint;

    private float nearestPrefabSpawnPointOffset = 1.63f; // the width of the top plank of a target prefab
    private bool isThereEnoughPrefabs;
    private int placedPrefabs;
    private List<GameObject> objectsInScene = new List<GameObject>();

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(objectsInScene);
    }

    void Update()
    {
        if (isThereEnoughPrefabs) return;
        else
        {
            Vector3 pos = new Vector3(
                Random.Range(slingshot.transform.position.x + nearestPrefabSpawnPointOffset, slingshot.transform.position.x + furtherPrefabSpawnPoint), 
                slingshot.transform.position.y, slingshot.transform.position.z);
            var instance = Instantiate(targetPrefab, pos, Quaternion.identity);
            objectsInScene.Add(instance);
            Test();
        }
    }

    private void Test()
    {
        placedPrefabs = 0;
        foreach (GameObject obj in objectsInScene)
        {
            string layerName = LayerMask.LayerToName(obj.layer);
            Debug.Log(layerName);
            if (layerName == "Target")
            {
                placedPrefabs++;
            }
        }
        if (placedPrefabs >= prefabCount)
        {
            isThereEnoughPrefabs = true;
        }
    }
}
