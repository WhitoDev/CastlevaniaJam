using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    public Animator animatorController;
    public GameObject bullet;
    public int totalHealth = 2;
    public int health = 2;

    public float minDistanceToAttack = 2.0f;
    private float xDistanceToPlayer;

    #region AnimationHashes
    int hsIdle = Animator.StringToHash("Base Layer.Skeleton_Idle");
    int hsThrowBone = Animator.StringToHash("Base Layer.Skeleton_Throw");
    #endregion

    [ExecuteInEditMode]
	void Start () 
    {
        GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponentInChildren<Renderer>().receiveShadows = false;
        animatorController = GetComponentInChildren<Animator>();
    }
		
	void Update () 
    {
        CheckPlayerProximity();
        if(xDistanceToPlayer <= minDistanceToAttack)
        {
            //Face Player if idle
            int hsState = animatorController.GetCurrentAnimatorStateInfo(0).fullPathHash;
            if(hsState == hsIdle)
            {
                float xScale =  Mathf.Sign(this.transform.position.x - GameManager.Instance.Player.transform.position.x);
                transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
            }

        }
        SetAnimations();
    }

    void SetAnimations()
    {
        
    }

    void CheckPlayerProximity()
    {
        xDistanceToPlayer = Mathf.Abs( transform.position.x - GameManager.Instance.Player.transform.position.x);
    }

}
