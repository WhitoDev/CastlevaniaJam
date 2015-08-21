using UnityEngine;
using System.Collections;

public class ColliderSender : MonoBehaviour 
{
    public LayerMask triggerWith;
    void OnTriggerStay2D(Collider2D col)
    {
        //  ((mask.value & (1 << layer)) > 0)
        if (((triggerWith.value & (1 << col.gameObject.layer)) > 0))
            SendMessageUpwards("ChildTriggerStay2D", new object[] { gameObject.name, col }); // Crear una nueva clase para enviar en el Mensaje!!
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (((triggerWith.value & (1 << col.gameObject.layer)) > 0))
        {
            SendMessageUpwards("ChildTriggerEnter2D", new object[] { gameObject.name, col });
        }
    }
}
