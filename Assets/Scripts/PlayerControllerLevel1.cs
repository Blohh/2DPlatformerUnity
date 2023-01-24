using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevel1 : MonoBehaviour
{
    public float moveSpeed = 6.0f;
    public float jumpForce = 0.25f;
    public float brakingSpeed = 3f;
    private float killOffset = 0.2f;
    private int maxKeyNumber = 3;
    private Vector3 startPosition;
    public LayerMask groundLayer;
    private Rigidbody2D rigidBody;
    public Animator animator;
    private bool isWalking;
    private bool isFacingRight;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (transform.position.y < -10)
            {
                Die();

            }
            isWalking = false;

            if (Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D)))
            {
                if (rigidBody.transform.parent != null)
                    Unlock();

                moveRight();
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A)))
            {
                if (rigidBody.transform.parent != null)
                    Unlock();

                moveLeft();
            }
            else if (rigidBody.transform.parent == null)
            {
                slowDown();
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                if (rigidBody.transform.parent != null)
                    Unlock();

                Jump();
            }

            animator.SetBool("isGrounded", isGrounded());
            animator.SetBool("isWalking", isWalking);
        }
        
    }

    private void slowDown()
    {
        if (rigidBody.velocity.x > 0)
        {
            isWalking = true;
            rigidBody.velocity = new Vector2(Mathf.Max(0.0f, rigidBody.velocity.x-brakingSpeed), rigidBody.velocity.y);
        }
        else if (rigidBody.velocity.x < 0)
        {
            isWalking = true;
            rigidBody.velocity = new Vector2(Mathf.Max(0.0f, rigidBody.velocity.x + brakingSpeed), rigidBody.velocity.y);
        }
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, 0.5f, groundLayer.value);
    }
    void moveLeft()
    {
        isWalking = true;
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
        isWalking = true;
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

    void Jump()
    {
        if (isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.instance.AddCoins();
            other.gameObject.SetActive(false);

        }
        else if (other.CompareTag("FinishLine"))
        {
            int keyNumber = GameManager.instance.GetCurrentKeysCount();
            if (keyNumber == maxKeyNumber)
            {
                Debug.Log("Congratulations! You've cleared this level!");
            }
            else
            {
                Debug.Log("You still need to get: " + (maxKeyNumber - keyNumber) +" keys!");

            }

        }
        else if (other.CompareTag("Enemy"))
        {
            if (this.transform.position.y < other.transform.position.y + killOffset)
            {
                Die();
            }
            else
            {
                GameManager.instance.AddEnemys();
            }


        }
        else if (other.CompareTag("Key"))
        {
            GameManager.instance.AddKeys();
            other.gameObject.SetActive(false);
        }

        else if (other.CompareTag("Heart"))
        {
            GameManager.instance.AddLives();
            other.gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
         if (other.CompareTag("MovingPlatform"))
        {
            rigidBody.isKinematic = true;
            rigidBody.transform.parent = other.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            Unlock();
        }
    }
    private void Unlock()
    {
        rigidBody.isKinematic = false;
        rigidBody.transform.parent = null;
    }
    private void Die()
    {
        GameManager.instance.DeleteLives();
        this.transform.position = startPosition;
    }
}
