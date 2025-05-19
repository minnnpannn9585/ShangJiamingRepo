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
