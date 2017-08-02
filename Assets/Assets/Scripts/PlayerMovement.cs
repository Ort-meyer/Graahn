using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    float m_moveSpeed = 3.0f;
    float m_gravity = 2.0f;
    float m_jumpSpeed = 8.0f;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
