using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    private float lastHMove = 0.0f;
    private float lastVMove = 0.0f;

    private Vector2 movement;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";

    [SerializeField] private Transform Weapon;
    [SerializeField] private float WeaponForce = 2f;
    [SerializeField] private PooledObject Bullets;

    [System.Serializable]
    public class PooledObject {
        //Declare
        public GameObject Prefab;
        public int NumberOfObjects;
        public float Lifetime = 3.5f;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Instantiate global object pools
        zsPublicStatics.Bullets = new zsPublicStatics.gudcObject();

        //Setup object pools
        SetupObjectPoolingForBullets(zsPublicStatics.Bullets, Bullets);

    }

    void SetupObjectPoolingForBullets(zsPublicStatics.gudcObject ObjectPool, PooledObject PooledObjectToBe) {
        //Expand
        ObjectPool.Lifetimes = new float[PooledObjectToBe.NumberOfObjects];
        ObjectPool.GameObjects = new GameObject[PooledObjectToBe.NumberOfObjects];
        ObjectPool.Rigidbodies = new Rigidbody2D[PooledObjectToBe.NumberOfObjects];
        ObjectPool.CircleColliders = new CircleCollider2D[PooledObjectToBe.NumberOfObjects];
        //Loop
        for (int i = 0; i < PooledObjectToBe.NumberOfObjects; i++) {
            //Instantiate
            ObjectPool.GameObjects[i] = Instantiate(PooledObjectToBe.Prefab, null);
            //Set other
            ObjectPool.Rigidbodies[i] = ObjectPool.GameObjects[i].GetComponent<Rigidbody2D>();
            ObjectPool.CircleColliders[i] = ObjectPool.GameObjects[i].GetComponent<CircleCollider2D>();
            //Turn off
            ObjectPool.CircleColliders[i].enabled = false;
            ObjectPool.GameObjects[i].SetActive(false);
        }
    }

    void Update()
    {
        // input
        movement.x = Input.GetAxisRaw(horizontal);
        movement.y = Input.GetAxisRaw(vertical);

        anim();

        shoot();

        CheckBulletLifetime();

    }

    void CheckBulletLifetime() {

        //Loop
        for (int i = 0; i < Bullets.NumberOfObjects; i++) {
            //Check if lifetimes is greater than zero
            if (zsPublicStatics.Bullets.Lifetimes[i] > 0f) {
                //Subtract
                zsPublicStatics.Bullets.Lifetimes[i] -= Time.deltaTime;
                //Check if time up
                if (zsPublicStatics.Bullets.Lifetimes[i] <= 0f) {
                    //Disable bullet
                    EnableOrDisableBullet(i, false);
                }
            }
        }
    }

    void FixedUpdate()
    {
        // movement
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
    }
    
    // animate
    void anim()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);


        if (movement.x == 0 && movement.y == 0)
        {
            animator.SetBool("Moving", false);
            animator.SetFloat("Horizontal", lastHMove);
            animator.SetFloat("Vertical", lastVMove);

        }
        else {
            lastHMove = movement.x;
            lastVMove = movement.y;
            animator.SetBool("Moving", true);

        }
    }

    void shoot() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //Declare
            int intPoolIndex = zsPublicStatics.gGetNextObjectPoolIndex(zsPublicStatics.Bullets);
            //Reset line lifetime
            zsPublicStatics.Bullets.Lifetimes[intPoolIndex] = Bullets.Lifetime;
            //Position
            zsPublicStatics.Bullets.GameObjects[intPoolIndex].transform.position = Weapon.position;
            //Enable or disable bullet
            EnableOrDisableBullet(intPoolIndex, true);
            //Make bullet move
            zsPublicStatics.Bullets.Rigidbodies[intPoolIndex].AddForce(Weapon.transform.forward * WeaponForce,ForceMode2D.Force);
        }
    }

    void EnableOrDisableBullet(int intPoolIndex, bool blnEnable) {
        //Enable or disable collider
        zsPublicStatics.Bullets.CircleColliders[intPoolIndex].enabled = blnEnable;
        //Turn on or off game object
        zsPublicStatics.Bullets.GameObjects[intPoolIndex].SetActive(blnEnable);
        //Reset any forces applied
        zsPublicStatics.Bullets.Rigidbodies[intPoolIndex].velocity = Vector2.zero;
    }
    
}
