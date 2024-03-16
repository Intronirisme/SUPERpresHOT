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

    bool _isMoving = false;
    Vector3 _moveDir = Vector3.zero;
    Vector3 _velocity = Vector3.zero;

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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _camRoot.transform.rotation = Quaternion.identity;
        Camera.main.transform.parent = _camRoot.transform;
        Camera.main.transform.position = _camRoot.transform.position;
        Camera.main.transform.rotation = Quaternion.identity;
    }

    public void UnPosses()
    {
        Camera.main.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLook();

        _velocity = UpdateMove(_velocity);
        _velocity = UpdateFall(_velocity);
        _moveComp.Move(_velocity * Time.deltaTime);
    }

    private void UpdateLook()
    {
        float newPitch = Mathf.Clamp(_viewRotation.y - _lastLookInput.y * Time.deltaTime * CameraSensitivity,
            LookUpLimit.x, LookUpLimit.y
        );
        float newYaw = _viewRotation.x + _lastLookInput.x * Time.deltaTime * CameraSensitivity;
        newYaw = newYaw > 180 ? newYaw - 360 : newYaw < -180 ? newYaw + 360 : newYaw;
        _viewRotation = new Vector2(newYaw, newPitch);
        _camRoot.transform.rotation = Quaternion.Euler(new Vector3(_viewRotation.y, _viewRotation.x, 0));
    }

    private Vector3 UpdateMove(Vector3 velocity)
    {
        /*        float velocity = Mathf.Clamp(_velocity.magnitude + Time.deltaTime *
                    (_isMoving ? AccelBrake.x : -AccelBrake.y),
                    0, MaxWalkSpeed
                );*/
        //_velocity = _moveDir * velocity;
        //Debug.Log(_velocity.magnitude);
        //_moveComp.Move(_velocity * Time.deltaTime);
        return velocity;
    }

    Vector3 UpdateFall(Vector3 velocity)
    {
        if (!_moveComp.isGrounded)
        {
            velocity += Time.deltaTime * Vector3.down * 9.81f;
        }
        else if (velocity.y < 0)
        {
            velocity.y = 0;
        }
        return velocity;
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
