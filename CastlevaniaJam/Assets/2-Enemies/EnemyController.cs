using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private Animator animatorController;
    public GameObject bullet;
    public ParticleSystem destroyEffect;
    public int totalHealth = 2;
    public int health = 2;

    private bool inAggroRange;
    private bool gotHit;

    #region AnimationHashes
    int hsState;
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
        hsState = animatorController.GetCurrentAnimatorStateInfo(0).fullPathHash;
        if(gotHit)
        {
            TakeDamage();
        }

        if (hsState == hsIdle && inAggroRange)
        {
            float xScale = Mathf.Sign(this.transform.position.x - GameManager.Instance.Player.transform.position.x);
            transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
            StartCoroutine(ThrowBone());
        }        
        SetAnimations();
        ResetTriggers();
    }

    void SetAnimations()
    {
        animatorController.SetBool("throwBoneTrigger", throwBoneTrigger); throwBoneTrigger = false;
    }

    IEnumerator ThrowBone()
    {
        throwBoneTrigger = true;
        yield return new WaitForSeconds(0.2f);
        GameObject bone = Instantiate(bullet, transform.position + new Vector3(0, 1f, 0), Quaternion.identity) as GameObject;
        bone.GetComponent<Rigidbody2D>().AddForce(new Vector2(50f * -transform.localScale.x, 100f));
        bone.GetComponent<Rigidbody2D>().AddTorque(5f);
        Destroy(bone, 2f);
        yield break;
    }

    void TakeDamage()
    {

        destroyEffect.startSpeed = Mathf.Abs(destroyEffect.startSpeed) * transform.localScale.x;
        destroyEffect.Emit(3);
    }

    void ResetTriggers()
    {
        inAggroRange = false;
        gotHit = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    }

    void ChildTriggerStay2D(string gameObjName)
    {
        if (gameObjName == "AggroRange")
        {
            inAggroRange = true;
        }
    }

    void ChildTriggerEnter2D(string gameObjName)
    {
        if (gameObjName == "Sprite")
        {
            gotHit = true;
        }
    }
}
