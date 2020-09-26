using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runLeft;
    public AnimationClip runRight;
}
public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float turnSpeed = 1000.0f;
    public PlayerAnim playerAnim;
    private Animation anim;
    private float initHp = 100.0f;
    private float currHp = 100.0f; //(curHp/initHp 생명 bar 만들때 사용)
    
    //이벤트 처리 - 델리게이트(대리자)- 변수(함수를 저장)-함수포인터와 유사
    //델리게이트 원형을 정의
    //public delegate (함수원형)
    //public static event (델리게이트명) (이벤트명 = 변수명)
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play(playerAnim.idle.name);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");        
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        transform.Translate(dir.normalized * Time.deltaTime * moveSpeed);
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * r);
        PlayAnim(h,v);
    }


    void PlayAnim(float h, float v){
        if (v >= 0.1f){
            anim.CrossFade(playerAnim.runForward.name, 0.3f);
        }
        else if(v <= -0.1f){
            anim.CrossFade(playerAnim.runBackward.name, 0.3f);
        }
        else if (h>= 0.1f){
            anim.CrossFade(playerAnim.runRight.name, 0.3f);
        }
        else if (h <= -0.1f){
            anim.CrossFade(playerAnim.runLeft.name, 0.3f);
        }
        else{
            anim.CrossFade(playerAnim.idle.name, 0.3f);

        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("PUNCH")){
            currHp -= 10.0f;
            if(currHp <= 0.0f){
                PlayerDie();
            }
        }
    }

    void PlayerDie(){
        GameManager.isGameOver = true;
        
        //Raise events
        OnPlayerDie();
        // event방식이면 아래 코드가 필요없다.
        // GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        // foreach(GameObject monster in monsters){
        //     monster.SendMessage("YouWin", SendMessageOptions.DontRequireReceiver);
        // }
    }
}
