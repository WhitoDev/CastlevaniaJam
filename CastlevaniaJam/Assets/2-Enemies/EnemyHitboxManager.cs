using UnityEngine;
using System.Collections;

public class EnemyHitboxManager : MonoBehaviour 
{

    public LayerMask hitLayer;
    public ParticleSystem destroyEffect;

    [ExecuteInEditMode]
    void Start()
    {
        this.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("HitboxWhipBelmont"))
        {

            GameObject.Destroy(this.transform.parent.gameObject);
        }
    }
}
