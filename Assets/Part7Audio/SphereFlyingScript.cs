using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SphereFlyingScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    private Vector3 input = Vector3.zero;

    public void MoveHorizontal(InputAction.CallbackContext context)
    {
        input.x = context.ReadValue<float>();
    }

    public void MoveVertical(InputAction.CallbackContext context)
    {
        input.y = context.ReadValue<float>();
    }

    private void LateUpdate()
    {
        transform.position += input * speed * Time.deltaTime;
        //input = Vector3.zero;
    }
}
