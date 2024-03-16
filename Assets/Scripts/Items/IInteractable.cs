using UnityEngine;

public interface IInteractable
{
    public void Take();

    public void PutDown();

    public void Throw();

    public void Use();

    //Unfreeze the item's time
    public void Play();

    //stop the item's time
    public void Stop();

    public GameObject GetItem();
}
