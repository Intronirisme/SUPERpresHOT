using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float CameraSensitivity = 3;
    public float LookUpLimit = 89;
    public float MaxWalkSpeed = 3;
    public float Acceleration = 1.9f;

    Vector2 _lastMoveInput = Vector2.zero;
    Vector2 _lastLookInput = Vector2.zero;
    Vector2 _viewRotation = Vector2.zero;

    //References
    private Transform _camRoot;
    private CharacterController _moveComp;

    void Awake()
    {
        _camRoot = transform.Find("CameraRoot");
        _moveComp = GetComponent<CharacterController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Posses();
    }
    public void Posses()
    {
        _camRoot.transform.rotation = Quaternion.identity;
        Camera.main.transform.parent = _camRoot.transform;
        Camera.main.transform.position = Vector3.zero;
        Camera.main.transform.rotation = Quaternion.identity;
    }

    public void UnPosses()
    {
        Camera.main.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Move : " + _lastMoveInput);
        UpdateLook();
    }

    private void UpdateLook()
    {
        float newPitch = Mathf.Clamp(_viewRotation.y + _lastLookInput.y * Time.deltaTime * CameraSensitivity,
            -LookUpLimit, LookUpLimit
        );
        float newYaw = _viewRotation.x + _lastLookInput.x * Time.deltaTime * CameraSensitivity;
        newYaw = newYaw > 180 ? newYaw - 360 : newYaw < -180 ? newYaw + 360 : newYaw;
        _viewRotation = new Vector2(newYaw, newPitch);
        _camRoot.transform.rotation = Quaternion.Euler(new Vector3(_viewRotation.y, _viewRotation.x, 0));
    }

    private void UpdateMove()
    {

    }


    public void Move(InputAction.CallbackContext context)
    {
        _lastMoveInput = context.ReadValue<Vector2>();
    }

    public void Look(InputAction.CallbackContext context)
    {
        _lastLookInput = context.ReadValue<Vector2>();
    }
}
