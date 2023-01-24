using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float XMin = 7.0f, XMax = 9.0f;
    public float YMin = 1.0f, YMax = 2.0f;
    public float moveSpeedHorizontal = 0.005f, moveSpeedVertical = 0.005f;
    public bool isMovingHorizontally = false;
    public bool isMovingRight = true;
    public bool isMovingUp = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (isMovingHorizontally)
            {
                StartCoroutine(HorizontalMovement());
                ;
            }
            else
            {
                StartCoroutine(VerticalMovement());

            }
        }
            
    }

    private IEnumerator VerticalMovement()
    {
        if (isMovingUp)
        {
            if (this.transform.position.y < YMax)
            {
                moveUp();
            }
            else
            {
                yield return new WaitForSeconds(1);
                moveDown();
            }
        }
        else
        {
            if (this.transform.position.y > YMin)
            {
                moveDown();
            }
            else
            {
                yield return new WaitForSeconds(1);
                moveUp();
            }
        }
    }

    private IEnumerator HorizontalMovement()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x < XMax)
            {
                moveRight();
            }
            else
            {
                yield return new WaitForSeconds(1);
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
                yield return new WaitForSeconds(1);
                moveRight();
            }
        }
    }

    void moveLeft()
    {
        isMovingRight = false;
        transform.Translate(-moveSpeedHorizontal, 0.0f, 0.0f);
    }
    void moveRight()
    {
        isMovingRight = true;
        transform.Translate(moveSpeedHorizontal, 0.0f, 0.0f);
    }
    void moveUp()
    {
        isMovingUp = true;
        transform.Translate(0.0f, moveSpeedVertical, 0.0f);
    }
    void moveDown()
    {
        isMovingUp = false;
        transform.Translate(0.0f, -moveSpeedVertical, 0.0f);
    }
}
