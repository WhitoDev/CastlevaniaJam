using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour 
{
    public GameObject destroyEffect;

    void Start()
    {
        StartCoroutine(SetToTrigger());
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        GameObject flame = Instantiate(destroyEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(flame, 1f);        
        if(col.tag == "Player")
        {

        }
        Destroy(this.gameObject);
    }

    IEnumerator SetToTrigger()
    {
        yield return new WaitForFixedUpdate();
        GetComponent<Collider2D>().isTrigger = true;
        yield break;
    }
}
