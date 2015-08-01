using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public GameObject Bullet;

    [ExecuteInEditMode]
	void Start () 
    {
        GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponentInChildren<Renderer>().receiveShadows = false;
    }
		
	void Update () {}
}
