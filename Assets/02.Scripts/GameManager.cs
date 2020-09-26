using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform[] points;
    public GameObject MonsterPrefab;
    public float createTime = 3.0f;
    public static bool isGameOver = false;
    private WaitForSeconds ws;
    void Start()
    {
        GameObject spawnPointGroup = GameObject.Find("SpawnPointGroup");
        if(spawnPointGroup != null){
            points = spawnPointGroup.GetComponentsInChildren<Transform>();
        }    
        ws = new WaitForSeconds(createTime);
        StartCoroutine(CreateMonster());
    }

    IEnumerator CreateMonster(){
        while(!isGameOver){
            yield return ws;
            int idx = Random.Range(1,points.Length);
            GameObject monster = Instantiate<GameObject>(MonsterPrefab);
            monster.transform.position = points[idx].position;
            Vector3 dir = points[0].position - points[idx].position;
            Quaternion rot = Quaternion.LookRotation(dir);
            monster.transform.rotation = rot;
        }

        
    }
}
