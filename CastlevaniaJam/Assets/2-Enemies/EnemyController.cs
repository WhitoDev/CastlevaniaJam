using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{

    private Animator animatorController;
    public GameObject bullet;
    public GameObject destroyEffect;
    public int totalHealth = 2;
    public int health;

    public Material normalMaterial;
    public Material gettingHitMaterial;

    private bool inAggroRange;
    private bool gotHit;
    private bool isHit;

    private int thrownBones;

    #region AnimationHashes
    int hsState;
    int hsIdle = Animator.StringToHash("Base Layer.Skeleton_Idle");
    int hsThrowBone = Animator.StringToHash("Base Layer.Skeleton_Throw");
    #endregion

    #region Triggers
    bool throwBoneTrigger;
    #endregion

    [ExecuteInEditMode]
    void Start()
    {
        GetComponentInChildren<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        GetComponentInChildren<Renderer>().receiveShadows = false;
        animatorController = GetComponentInChildren<Animator>();
        health = totalHealth;
        thrownBones = 0;
    }

    void Update()
    {
        hsState = animatorController.GetCurrentAnimatorStateInfo(0).fullPathHash;
        if (gotHit && !isHit)
        {
            StartCoroutine(TakeDamage());
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
        if(thrownBones >= 3) yield break;
        throwBoneTrigger = true;
        yield return new WaitForSeconds(0.2f);
        GameObject bone = Instantiate(bullet, transform.position + new Vector3(0, 1f, 0), Quaternion.identity) as GameObject;
        bone.GetComponent<Rigidbody2D>().AddForce(new Vector2(80f * -transform.localScale.x / (thrownBones + 1), 100f));
        bone.GetComponent<Rigidbody2D>().AddTorque(5f);
        Destroy(bone, 2f);
        thrownBones++;
        if (thrownBones >= 3)
        {
            yield return new WaitForSeconds(2);
            thrownBones = 0;
        }
        yield break;
    }

    IEnumerator TakeDamage()
    {
        isHit = true;
        health--;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.material = gettingHitMaterial;

        if (health <= 0)
        {
            var effect = Instantiate(destroyEffect, transform.position, transform.localScale.x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0)) as GameObject;
            effect.GetComponent<ParticleSystem>().Emit(3);
            Destroy(effect, 2);
            Destroy(this.gameObject);
        }

        yield return new WaitForSeconds(0.3f);
        sr.material = normalMaterial;
        gotHit = false;
        isHit = false;
        yield break;
    }

    void ResetTriggers()
    {
        inAggroRange = false;
        gotHit = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    }

    void ChildTriggerStay2D(ChildColliderInfo info)
    {
        if (info.colliderGameObj.name == "AggroRange")
        {
            inAggroRange = true;
        }
    }

    void ChildTriggerEnter2D(ChildColliderInfo info)
    {
        if (info.colliderGameObj.name == "Sprite")
        {
            if (!gotHit)
                gotHit = true;
        }
    }
}
