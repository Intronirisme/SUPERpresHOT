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
    public Vector2 LookUpLimit = new Vector2(-10, 89);
    public float MaxWalkSpeed = 3;
    public Vector2 AccelBrake = new Vector2(1.1f, 2f);
    public float AlignSpeed = 15f;

    bool _isMoving = false;
    Vector3 _velocity = Vector3.zero;

    Vector2 _lastMoveInput = Vector2.zero;
    Vector2 _lastLookInput = Vector2.zero;
    Vector2 _viewRotation = Vector2.zero;

    //References
    private Transform _camRoot;
    private Transform _body;
    private CharacterController _moveComp;

    void Awake()
    {
        _camRoot = transform.Find("CameraRoot");
        _body = transform.Find("Body");
        _moveComp = GetComponent<CharacterController>();
        Posses();
    }

    private void Start()
    {
        Posses();
    }

    public void Posses()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _camRoot.transform.rotation = Quaternion.identity;
        Camera.main.transform.parent = _camRoot.transform;
        Camera.main.transform.position = _camRoot.transform.position;
        Camera.main.transform.rotation = _camRoot.transform.rotation;
    }

    public void UnPosses()
    {
        Camera.main.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLook();
        UpdateMove();
        UpdateFall();
        UpdateRotation();
        _moveComp.SimpleMove(_velocity);
    }

    private void UpdateLook()
    {
        float newPitch = Mathf.Clamp(_viewRotation.y - _lastLookInput.y * Time.deltaTime * CameraSensitivity,
            LookUpLimit.x, LookUpLimit.y
        );
        float newYaw = _viewRotation.x + _lastLookInput.x * Time.deltaTime * CameraSensitivity;
        newYaw = newYaw > 180 ? newYaw - 360 : newYaw < -180 ? newYaw + 360 : newYaw;
        _viewRotation = new Vector2(newYaw, newPitch);
        _camRoot.rotation = Quaternion.Euler(new Vector3(_viewRotation.y, _viewRotation.x, 0));
    }

    private void UpdateMove()
    {
        float speed = Mathf.Clamp(_velocity.magnitude + Time.deltaTime *
            (_isMoving ? AccelBrake.x : -AccelBrake.y),
            0, MaxWalkSpeed
        );
        Vector2 rotatedInput = _lastMoveInput.Rotate(_camRoot.rotation.eulerAngles.y);
        _velocity.z = rotatedInput.x * speed;
        _velocity.x = rotatedInput.y * speed;
    }

    private void UpdateFall()
    {
        if (!_moveComp.isGrounded) _velocity.y -= 9.81f * Time.deltaTime;
        else if (_velocity.y < 0) _velocity.y = 0;
    }

    private void UpdateRotation()
    {
        if (_velocity.magnitude < .01f) return;
        float yaw = _body.rotation.eulerAngles.y;
        float targetYaw = Mathf.Atan2(_velocity.normalized.x, _velocity.normalized.z) * Mathf.Rad2Deg;
        float delta = Mathf.Abs(targetYaw - yaw);
        _body.rotation = Quaternion.Euler(new Vector3(0,
            Mathf.LerpAngle(yaw, targetYaw, Mathf.Clamp01((AlignSpeed * Time.deltaTime) / delta)), 0
        ));
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        input = new Vector2(input.y, input.x);
        _isMoving = input.magnitude > 0;
        if ( _isMoving ) _lastMoveInput = input;
    }

    public void Look(InputAction.CallbackContext context)
    {
        _lastLookInput = context.ReadValue<Vector2>();
    }
}
