using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BreakableStuff : MonoBehaviour
{
    private AudioSource _audio;
    private GameObject _mesh;
    private GameObject _vfx;
    // Start is called before the first frame update
    void Start()
    {
        GameMaster.Instance.Attack += OnBreak;
        _audio = GetComponent<AudioSource>();
        _audio.Stop();
        _mesh = transform.GetChild(0).gameObject;
        _vfx = transform.GetChild(1).gameObject;
        _vfx.SetActive(false);
    }

    void OnBreak()
    {
        _audio.Play();
        _mesh.SetActive(false);
        _vfx.SetActive(true);
    }
}
