using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private float _pickUpRange = 10f;
    private float _throwForce = 10f;

    private IInteractable _itemInHand = null;
    private List<IInteractable> _itemsInLayer = new List<IInteractable>(); //add a list for each timer layer ?

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeItem();
        }
        if (Input.GetMouseButtonDown(0))
        {
            UseItem();
            PlayItem();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ThrowItem();
        }
    }


    private void TakeItem()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _pickUpRange))
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out IInteractable intertactable))
                {
                    intertactable.Take();
                    _itemInHand = intertactable;

                    //watch which time layer the hand is so we know in which list we put the item
                    _itemsInLayer.Add(_itemInHand);
                }
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
        }
        else
        {
            Debug.Log("No item in hand to be thrown");
        }
    }

    private void PlayItem()
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
