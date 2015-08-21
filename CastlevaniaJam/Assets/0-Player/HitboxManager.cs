using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class HitboxManager : MonoBehaviour 
{
    public List<PolygonCollider2D> WhipHitboxes;

	void Start () 
    {
        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponent<Renderer>().receiveShadows = false;
	}

	void Update () 
    {
	
	}

    public void ActivateHitbox(int id)
    {
        WhipHitboxes[id].enabled = true;
    }

    public void DeactivateHitbox(int id)
    {
        if(id == -1)
        {
            foreach(PolygonCollider2D col in WhipHitboxes)
            {
                col.enabled = false;
            }
            return;
        }
        WhipHitboxes[id].enabled = false;
    }
}
