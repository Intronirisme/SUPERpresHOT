using UnityEngine;

public class Pistol : MonoBehaviour, IInteractable
{
    public bool canBeUsed;

    public void Take()
    {
        Debug.Log(name + " has been taken");
    }

    public void PutDown()
    {
        Debug.Log(name + " has been put down");
    }

    public void Throw()
    {
        Debug.Log(name + " has been throwned");

        Stop();
    }

    public void Use()
    {
        if (canBeUsed)
        {
            Debug.Log(name + " has been used");
        }
    }

    public void Play()
    {
        Debug.Log(name + " has been unfreezed");
    }

    public void Stop()
    {
        Debug.Log(name + " has been freezed");
    }
}
