using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;


public class GameMaster : MonoBehaviour
{
    public float AssaultDuration = 2;
    public float PreparationDelay = 30;

    private List<IInteractable> _timeLayer0 = new List<IInteractable>();
    private List<IInteractable> _timeLayer1 = new List<IInteractable>();
    private List<IInteractable> _timeLayer2 = new List<IInteractable>();

    private List<IInteractable> GetLayer(int Layer)
    {
        switch (Layer)
        {
            case 0:
                return _timeLayer0;

            case 1:
                return _timeLayer1;

            case 2:
                return _timeLayer2;
        }
        Debug.LogError("Invalid Layer : " + Layer);
        return new List<IInteractable>();
    }
    public void AddObject(IInteractable Object, int Layer, bool Used)
    {
        RemoveObject(Object);
        List<IInteractable> layer = GetLayer(Layer);
        if (layer.Count == 0) return;
        layer.Add(Object);
    }
    public void RemoveObject(IInteractable Object)
    {
        if (_timeLayer0.Contains(Object)) _timeLayer0.Remove(Object);
        if (_timeLayer1.Contains(Object)) _timeLayer1.Remove(Object);
        if (_timeLayer2.Contains(Object)) _timeLayer2.Remove(Object);
    }
    public void ResumeLayer(int Layer)
    {
        List<IInteractable> layer = GetLayer(Layer);
        if (layer.Count == 0) return;
        foreach (IInteractable obj in layer) obj.Play();
    }

    public void FreezeLayer(int Layer)
    {
        List<IInteractable> layer = GetLayer(Layer);
        if (layer.Count == 0) return;
        foreach (IInteractable obj in layer) obj.Stop();
    }

    public void StartGame()
    {
        StartCoroutine(StartSequence());
    }
    IEnumerator StartSequence()
    {
        //Kalm
        yield return new WaitForSeconds(3.0f);
        //Panik
        StartAssault();
        yield return new WaitForSeconds(AssaultDuration);
        //BigBrain
        WorldFreeze();
        yield return new WaitForSeconds(PreparationDelay);
        WorldResume();
    }

    private void StartAssault()
    {

    }

    private void WorldFreeze()
    {

    }

    private void WorldResume()
    {

    }

}
