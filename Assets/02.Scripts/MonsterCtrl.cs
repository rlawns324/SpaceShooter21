using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum STATE{
    IDLE,
    TRACE,
    ATTACK,
    DEAD
}
public class MonsterCtrl : MonoBehaviour
{
    [System.NonSerialized] //inspector에서 숨기기
    public Transform monsterTr;
    [HideInInspector]
    public Transform playerTr;
    private NavMeshAgent agent;
    private Animator anim;
    public STATE state = STATE.IDLE;
    public bool isDead = false;
    public float attackDist = 2.0f;
    public float traceDist = 10.0f;
    private int hashAttack;
    private int hashHit;
    private int hashDie = Animator.StringToHash("Die");
    public float hp = 100.0f;
    private int hashDance = Animator.StringToHash("PlayerDie");

    private void OnEnable() { //event연결시에 많이 사용함
        PlayerCtrl.OnPlayerDie += this.YouWin;
    }

    private void OnDisable() {
        PlayerCtrl.OnPlayerDie -= this.YouWin;
    }
    void Start()
    {
        monsterTr = this.GetComponent<Transform>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("PLAYER"); //연결돼있지 않은 오브젝트이기때문에 우선 검색을 해야함
        if(playerObj != null){
            playerTr = playerObj.GetComponent<Transform>();
        }
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        hashAttack = Animator.StringToHash("IsAttack");
        hashHit = Animator.StringToHash("Hit");
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());
    }

    //매 프레임마다 update에서 상태값을 체크하지 않고 coroutine에서 0.3초마다 상태체크하도록 최적화
    IEnumerator CheckMonsterState(){
        while(!isDead){
            float distance = Vector3.Distance(monsterTr.position, playerTr.position);
            if(distance <= attackDist){
                state = STATE.ATTACK;
            }else if(distance <= traceDist){
                state = STATE.TRACE;
            }else{
                state = STATE.IDLE;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator MonsterAction(){
        while(!isDead){
            switch(state){
                case STATE.IDLE:
                    anim.SetBool("IsTrace", false);
                    agent.isStopped = true;
                    break;
                case STATE.TRACE:
                    anim.SetBool(hashAttack, false);
                    anim.SetBool("IsTrace", true);
                    agent.SetDestination(playerTr.position);
                    agent.isStopped = false;
                    break;
                case STATE.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;
                case STATE.DEAD:
                    break;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.collider.CompareTag("BULLET")){
            Destroy(other.gameObject);
            anim.SetTrigger(hashHit);
            hp -= 20.0f;
            if(hp <= 0.0f){
                MonsterDie();
            }
        }
    }

    void MonsterDie(){
        GetComponent<CapsuleCollider>().enabled = false;
        agent.isStopped = true;
        StopAllCoroutines();
        anim.SetTrigger(hashDie);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Monster Hit = " + other.gameObject.name);
    }

    public void YouWin(){
        StopAllCoroutines();
        agent.isStopped = true;
        anim.SetTrigger(hashDance);
    }
}
