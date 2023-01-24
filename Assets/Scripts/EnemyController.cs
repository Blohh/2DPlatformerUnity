using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    public float XMin = 7.0f, XMax=9.0f;
    public float moveSpeed = 5.0f;
    private bool isFacingRight, isMovingRight;
    private bool isDead;
    private float killOffset = 0.2f;
    public Animator animator;
    private Vector3 pausedVelocity;
    private void Awake()
    {
        isFacingRight = false;
        isMovingRight = false;
        isDead = false;
        rigidBody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentGameState == GameState.GS_PAUSEMENU)
        {
            pausedVelocity = rigidBody.velocity;
            rigidBody.velocity = new Vector3( 0, 0, 0);
        }
        if (!isDead && GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            rigidBody.velocity = new Vector3(0, 0, 0);
            if (isMovingRight)
            {
                if (this.transform.position.x < XMax)
                {
                    moveRight();   
                }
                else
                {
                    moveLeft();
                }
            }
            else
            {
                if (this.transform.position.x > XMin)
                {
                    moveLeft();
                }
                else
                {
                    moveRight();
                }
            }
        }
        animator.SetBool("isDead", isDead);
    }
    void moveLeft()
    {
        isMovingRight = false;
        if (isFacingRight)
        {
            Flip();
        }
        if (rigidBody.velocity.x > -moveSpeed)
        {
            rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.left * 0.6f, ForceMode2D.Impulse);
        }
    }
    void moveRight()
    {
        isMovingRight = true;
        if (!isFacingRight)
        {
            Flip();
        }
        if (rigidBody.velocity.x < moveSpeed)
        {
            rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
            rigidBody.AddForce(Vector2.right * 0.6f, ForceMode2D.Impulse);
        }
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        isMovingRight = !isMovingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(this.transform.position.y + killOffset < other.transform.position.y)
            {
                isDead = true;
                StartCoroutine(KillOnAnimationEnd());
            }
            
        }
    }
    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
    }


}
