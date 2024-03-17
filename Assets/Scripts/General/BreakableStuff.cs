using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(MeshRenderer))]
public class BreakableStuff : MonoBehaviour
{
    private AudioSource _audio;
    private MeshRenderer _mesh;
    // Start is called before the first frame update
    void Start()
    {
        GameMaster.Instance.Attack += OnBreak;
        _audio = GetComponent<AudioSource>();
        _audio.Stop();
        _mesh = GetComponent<MeshRenderer>();
    }

    void OnBreak()
    {
        _audio.Play();
        _mesh.enabled = false;
    }
}
