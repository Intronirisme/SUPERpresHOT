using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }
    public float AssaultDuration = 2;
    public float PreparationDelay = 30;

    public UnityAction Attack;
    public UnityAction Freeze;
    public UnityAction Unfreeze;

    private List<Item> _timeLayer0 = new List<Item>();
    private List<Item> _timeLayer1 = new List<Item>();
    private List<Item> _timeLayer2 = new List<Item>();

    private AudioSource _audioPlayer;

    void Start()
    {
        _audioPlayer = GetComponent<AudioSource>();
        if (Instance == null) Instance = this;
        else Destroy(this);
        StartCoroutine(StartSequence());
    }
    private List<Item> GetLayer(int Layer)
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
        return new List<Item>();
    }
    public void AddObject(Item Item, int Layer)
    {
        RemoveObject(Item);
        List<Item> layer = GetLayer(Layer);
        layer.Add(Item);
    }
    public void RemoveObject(Item Item)
    {
        if (_timeLayer0.Contains(Item)) _timeLayer0.Remove(Item);
        if (_timeLayer1.Contains(Item)) _timeLayer1.Remove(Item);
        if (_timeLayer2.Contains(Item)) _timeLayer2.Remove(Item);
    }
    public void ResumeLayer(int Layer)
    {
        List<Item> layer = GetLayer(Layer);
        if (layer.Count == 0) return;
        Debug.Log("resume");
        foreach (Item obj in layer) obj.Unfreeze();
    }

    public void FreezeLayer(int Layer)
    {
        List<Item> layer = GetLayer(Layer);
        if (layer.Count == 0) return;
        foreach (Item obj in layer) obj.Freeze();
    }

    public void ToogleLayer(int Layer)
    {
        List<Item> layer = GetLayer(Layer);
        if (layer.Count == 0) return;
        if (layer[0].gameObject.layer == LayerMask.NameToLayer("Frozen"))
        {
            ResumeLayer(Layer);
        }
        else
        {
            FreezeLayer(Layer);
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartSequence());
    }
    IEnumerator StartSequence()
    {
        _audioPlayer.Play();
        //Kalm
        yield return new WaitForSeconds(3.0f);
        //Panik
        StartAssault();
        yield return new WaitForSeconds(AssaultDuration);
        //BigBrain
        WorldFreeze();
        _audioPlayer.Stop();
        yield return new WaitForSeconds(PreparationDelay);
        WorldResume();
    }

    private void StartAssault()
    {
        Attack();
    }

    private void WorldFreeze()
    {
        Freeze();
    }

    private void WorldResume()
    {
        Unfreeze();
    }

}
