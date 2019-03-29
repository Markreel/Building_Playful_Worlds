using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    bool isGrounded = false;
    Vector3 velocity;

    CharacterController charController;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float _vert = Input.GetAxis("Vertical");
        float _hor = Input.GetAxis("Horizontal");

        if (_vert != 0 || _hor != 0)
        {
            Vector3 velXZ = (_vert * transform.forward + _hor * transform.right).normalized * speed;
            velocity = new Vector3(velXZ.x, velocity.y, velXZ.z);
        }

        else
        {
            velocity = Vector3.zero; //-= Vector3.Scale(velocity, new Vector3(1, 0, 1)); //* (1 - noMoveFriction));
        }

        transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.x, 0);
    }

    private void FixedUpdate()
    {
        charController.Move(velocity * Time.deltaTime);
    }
}
