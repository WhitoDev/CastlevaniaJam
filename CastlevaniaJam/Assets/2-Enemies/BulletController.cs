using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour 
{
    public GameObject destroyEffect;
    public LayerMask collideWith;

    void Start()
    {
        StartCoroutine(SetToTrigger());
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = false;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (((collideWith.value & (1 << col.gameObject.layer)) > 0))
        {
            GameObject flame = Instantiate(destroyEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(flame, 1f);
            Destroy(this.gameObject);
        }
    }

    IEnumerator SetToTrigger()
    {
        yield return new WaitForFixedUpdate();
        GetComponent<Collider2D>().isTrigger = true;
        yield break;
    }
}
