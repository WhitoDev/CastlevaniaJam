using UnityEngine;
using System.Collections;

public class TestHit : MonoBehaviour 
{
    public LayerMask HitAgainst;
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("HitboxWhipBelmont"))
        {            
            GameObject.Destroy(this.transform.parent.gameObject);
        }
    }
}
