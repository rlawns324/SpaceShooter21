using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
     public Transform firePos;
     public GameObject bulletPrefab;
     public AudioClip fireSfx;
     private new AudioSource audio;
     public MeshRenderer muzzleFlash;
     public float fireRange = 10.0f;
     private RaycastHit hit;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * fireRange, Color.green);
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
            if(Physics.Raycast(firePos.position, firePos.forward, out hit, fireRange, 1<<8)){
                hit.collider.gameObject.GetComponent<MonsterCtrl>().OnDamage();
            }
        }
    }

    void Fire(){
        Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        audio.PlayOneShot(fireSfx);
        StartCoroutine(ShowMuzzleFlash());
    }

    //Coroutine
    IEnumerator ShowMuzzleFlash(){
        float rot = Random.Range(0.0f, 360.0f);
        muzzleFlash.transform.localRotation = Quaternion.Euler(0, 0, rot);

        float scale = Random.Range(0.5f,1.5f);
        muzzleFlash.transform.localScale = Vector3.one * scale;

        Vector2 offset = new Vector2(Random.Range(0,2), Random.Range(0,2)) * 0.5f;
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);
        muzzleFlash.enabled = true;
        yield return new WaitForSeconds(0.2f);
        muzzleFlash.enabled = false;
    }
}
