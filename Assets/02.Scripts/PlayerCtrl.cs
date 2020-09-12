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
        if (v >= 0.1f)
        {
            anim.CrossFade(playerAnim.runForward.name, 0.3f);
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runBackward.name, 0.3f);
        }
        else if (h>= 0.1f)
        {
            anim.CrossFade(playerAnim.runRight.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runLeft.name, 0.3f);
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);

        }

    }
}
