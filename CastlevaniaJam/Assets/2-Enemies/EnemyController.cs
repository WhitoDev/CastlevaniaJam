using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private Animator animatorController;
    public GameObject bullet;
    public GameObject destroyEffect;
    public int totalHealth = 2;
    public int health = 2;

    public float minDistanceToAttack = 2.0f;
    private float xDistanceToPlayer;

    #region AnimationHashes
    int hsIdle = Animator.StringToHash("Base Layer.Skeleton_Idle");
    int hsThrowBone = Animator.StringToHash("Base Layer.Skeleton_Throw");
    #endregion

    #region Triggers
    bool throwBoneTrigger;
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
                StartCoroutine(ThrowBone());
            }


        }
        SetAnimations();
    }

    void SetAnimations()
    {
        animatorController.SetBool("throwBoneTrigger", throwBoneTrigger); throwBoneTrigger = false;
    }

    void CheckPlayerProximity()
    {
        xDistanceToPlayer = Mathf.Abs( transform.position.x - GameManager.Instance.Player.transform.position.x);
    }

    IEnumerator ThrowBone()
    {
        throwBoneTrigger = true;
        yield return new WaitForSeconds(0.2f);
        GameObject bone = Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject;
        bone.GetComponent<Rigidbody2D>().AddForce(new Vector2(50f * -transform.localScale.x, 100f));
        bone.GetComponent<Rigidbody2D>().AddTorque(5f);
        Destroy(bone, 2f);
        yield break;
    }
}
