using UnityEngine;
using System.Collections;

public class ColliderSender : MonoBehaviour
{
    public LayerMask triggerWith;
    void OnTriggerStay2D(Collider2D col)
    {        
        if (((triggerWith.value & (1 << col.gameObject.layer)) > 0))
            SendMessageUpwards("ChildTriggerStay2D", new ChildColliderInfo() { colliderGameObj = this.gameObject, colliderTrigger = col });
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (((triggerWith.value & (1 << col.gameObject.layer)) > 0))
        {
            SendMessageUpwards("ChildTriggerEnter2D", new ChildColliderInfo() { colliderGameObj = this.gameObject, colliderTrigger = col });
        }
    }
}
