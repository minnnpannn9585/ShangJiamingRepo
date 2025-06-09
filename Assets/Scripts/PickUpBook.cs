using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

public class PickUpBook : MonoBehaviour
{
    bool isInsideCollider = false;
    public GameObject book;
    public int bookCount = 0;
    private FirstPersonController fpc;
    public Text bookCountText;
    //public float bounceForce = 200f; // 反弹力度
    public float bounceDistance = 0.5f; // 反弹距离
    public GameObject redFrame;
    bool canHitObstacle = true; // 是否可以碰撞障碍物

    private void Start()
    {
        fpc = transform.parent.GetComponent<FirstPersonController>();
        bookCountText.text = "Book Count: " + bookCount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            isInsideCollider = true;
            book = other.gameObject;
        }
        else if (other.CompareTag("Obstacle") && canHitObstacle)
        {
            Debug.Log("hit");
            // 轻微反弹效果
            Vector3 bounceDirection = new Vector3(transform.position.x - other.transform.position.x, 0, transform.position.z - other.transform.position.z);
            fpc.GetComponent<CharacterController>().Move(bounceDirection.normalized * bounceDistance);
            canHitObstacle = false; // 禁止再次碰撞障碍物
            StartCoroutine(TurnOnCanHitObs()); // 1秒后恢复可以碰撞障碍物的状态

            redFrame.SetActive(true); // 显示红色边框
            StartCoroutine(TurnOffRedFrame());
        }
    }

    IEnumerator TurnOnCanHitObs()
    {
        yield return new WaitForSeconds(1f);
        canHitObstacle = true; // 恢复可以碰撞障碍物的状态
    }

    IEnumerator TurnOffRedFrame()
    {
        yield return new WaitForSeconds(0.2f);
        redFrame.SetActive(false); // 关闭红色边框
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            isInsideCollider = false;
            book = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            if (isInsideCollider)
            {
                if (bookCount < 4)
                {
                    Destroy(book);
                    bookCount++;
                    fpc.m_WalkSpeed--;
                    bookCountText.text = "Book Count: " + bookCount;
                }
                    
            }
        }
    }
}
