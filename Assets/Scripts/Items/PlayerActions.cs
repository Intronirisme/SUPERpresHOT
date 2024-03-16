using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    private float _pickUpRange = 10f;
    private float _throwForce = 10f;

    private IInteractable _itemInHand = null;
    private List<IInteractable> _itemsInLayer = new List<IInteractable>(); //add a list for each timer layer ?

    private LayerMask _itemLayer;

    private void Start()
    {
        _itemLayer = LayerMask.GetMask("Item");
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TakeItem();
        }
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            UseItem();
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Aiming with item");
        }
        else if (context.canceled)
        {
            ThrowItem();
        }
    }

    public void TimeSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ItemTimeSwitch();
        }
    }

    private void TakeItem()
    {
        Camera cam = GetComponentInChildren<Camera>();
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, _pickUpRange, _itemLayer)) //we check if there is an item in the ray
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out IInteractable intertactable))
                {
                    if (_itemInHand != null) //if we already have an item we first put down the item in hand
                    {
                        _itemInHand.PutDown();
                        _itemInHand = null;
                    }

                    intertactable.Take();
                    _itemInHand = intertactable;
                }
            }
        }
        else //if we don't touch anything we put down the item
        {
            if (_itemInHand != null)
            {
                _itemInHand.PutDown();
                _itemInHand = null;
            }
        }
    }

    private void UseItem()
    {
        if (_itemInHand != null)
        {
            _itemInHand.Use();
        }
        else
        {
            Debug.Log("No item in hand");
        }
    }

    private void ThrowItem()
    {
        if (_itemInHand != null)
        {
            _itemInHand.Throw();
            _itemsInLayer.Add(_itemInHand); //watch which time layer the hand is so we know in which list we put the item

            _itemInHand = null;
        }
        else
        {
            Debug.Log("No item in hand to be thrown");
        }
    }

    private void ItemTimeSwitch() //time layer as an argument
    {
        if (_itemsInLayer.Count > 0)
        {
            foreach (var item in _itemsInLayer)
            {
                item.Play();
            }

            _itemsInLayer.Clear();
        }
    }
}
