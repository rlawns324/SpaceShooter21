using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;
    private int hitCount = 0;
    public Texture[] textures;
    private MeshRenderer _renderer;
    
    public AudioClip expSfx;
    private AudioSource _audio;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _renderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        int idx = Random.Range(0, textures.Length);
        _renderer.material.mainTexture = textures[idx];
    }

    private void OnCollisionEnter(Collision other) {
        if(other.collider.CompareTag("BULLET")){ //if other.gameObject.tag == "BULLET" 처럼 하면 GC때문에 퍼포먼스 저하
            hitCount++;
            if(hitCount == 3){
                ExpBarrel();

            }
        }
    }
    
    void ExpBarrel(){
        _audio.PlayOneShot(expSfx);

        GameObject expObj = Instantiate(expEffect, this.transform.position, Quaternion.identity);
        Destroy(expObj,3.0f);
        Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 1500.0f);
        Destroy(this.gameObject, 4.0f);
    }
}

