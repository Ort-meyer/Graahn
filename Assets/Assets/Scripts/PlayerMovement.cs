using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float m_moveSpeed = 3.0f;
    public float m_gravity = 2.0f;
    public float m_jumpSpeed = 8.0f;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Jump and gravity
        CharacterController charController = GetComponent<CharacterController>();
        if(charController.isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                charController.Move(new Vector3(0,1,0) * m_jumpSpeed * Time.deltaTime);
            }
        }
        else
        {
            charController.Move(new Vector3(0, -1, 0) * m_gravity * Time.deltaTime);
        }

        // Side movement
        Vector3 sideMovement = new Vector3();
        if(Input.GetKey(KeyCode.D))
        {
            sideMovement += new Vector3(1, 0, 0) * m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            sideMovement += new Vector3(-1, 0, 0) * m_moveSpeed * Time.deltaTime;
        }

        charController.Move(sideMovement);

    }
}
