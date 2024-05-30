using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesHolder : MonoBehaviour
{
    public Color[] colors;
    ParticleSystem.MainModule[] modules;
    public static ParticlesHolder instance;
    private void Awake()
    {
        instance = this;
        modules = new ParticleSystem.MainModule[transform.childCount];

        for(int i = 0;i<transform.childCount;i++)
        {
            modules[i] = transform.GetChild(i).GetComponent<ParticleSystem>().main;
            // module.startColor = colors[i];
        }
    }

    void Update()
    {
        for(int i = 0;i<transform.childCount;i++)
        {
            modules[i].startColor = colors[i];
        }
    }

}
