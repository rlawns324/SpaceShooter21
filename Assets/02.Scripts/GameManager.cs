using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton Design Pattern
    public static GameManager instance = null;
    public Transform[] points;
    public GameObject monsterPrefab;
    public float createTime = 3.0f;
    public bool isGameOver = false;
    private WaitForSeconds ws;
    public int maxPool = 20;
    public List<GameObject> monsterPool = new List<GameObject>();
    private void Awake() {
        //서로다른 scene에서 중복 생성 방지
        if(instance == null){
            instance = this;
        }else{
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        GameObject spawnPointGroup = GameObject.Find("SpawnPointGroup");
        if(spawnPointGroup != null){
            points = spawnPointGroup.GetComponentsInChildren<Transform>();
        }    
        ws = new WaitForSeconds(createTime);
        CreatePool();
        StartCoroutine(this.CreateMonster());
    }

    void CreatePool(){
        for(int i=0; i<maxPool; i++){
           //몬스터 생성
            GameObject monster = Instantiate<GameObject>(monsterPrefab);
            monster.name = "Monster_" + i.ToString("00"); //Monster_00, Monster_01, ....
            monster.SetActive(false);

            //오브젝트 풀에 추가
            monsterPool.Add(monster);
        }
    }

    IEnumerator CreateMonster(){
        while(!isGameOver){
            yield return ws;
            int idx = Random.Range(1,points.Length);
            
            //몬스터 오브젝트 풀에서 추출
            foreach(GameObject _monster in monsterPool){
                if(_monster.activeSelf == false){
                    _monster.transform.position = points[idx].position;
                    Vector3 dir = points[0].position - points[idx].position;
                    Quaternion rot = Quaternion.LookRotation(dir);
                    _monster.transform.rotation = rot;
                    _monster.SetActive(true);
                    break;
                    }
            }                
        }
    }
}
