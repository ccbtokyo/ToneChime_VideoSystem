using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyOSCReciever : MonoBehaviour
{
    Light light;
    // CookieTextureAnimation animationScript;
    public ParticleSystem particle;
    Vector2 startSize,startSpeed;
    //public Material particleMaterial;
    Renderer particleRender;
    
    private void Start()
    {
        light = GetComponent<Light>();
        // animationScript = GetComponent<CookieTextureAnimation>();

        startSize = new Vector2(particle.main.startSize.constantMin, particle.main.startSize.constantMax);
        startSpeed = new Vector2(particle.main.startSpeed.constantMin, particle.main.startSpeed.constantMax);

        particleRender = particle.transform.GetComponent<Renderer>();

        if (debug == false)
            return;
        //Debug.Log(particle.main.startSizeMultiplier);
        //Debug.Log(particle.main.startSizeXMultiplier);
        //Debug.Log(particle.main.startSizeYMultiplier);
        //Debug.Log(particle.main.startSizeZMultiplier);

        // Debug.Log(particle.main.startSize.constantMin);
        // Debug.Log(particle.main.startSize.constantMax);
    }

    public void _receive_simpleIntensityChanger(float value)
    {
        //Debug.Log(value);
        light.intensity = value.Map(0,1,0,15);
    }

    public bool canTrigger = true;


    public void _particle(float v)
    {
        if (canTrigger && v >= 0.6f)
        {
            if(particle !=null)
                particle.Play();
            canTrigger = false; // 発火後は再発火を防ぐためにフラグを下ろす
        }
        else if (!canTrigger && v <= 0.59f)
        {


            canTrigger = true;
        }


        var module = particle.main;
        Color c = module.startColor.color;
        //module.startColor = new Color(c.r, c.b, c.g, v);

        /////////////
        ///
        // particleRender.material.color = new Color(c.r, c.g, c.b, v);
        var module_ = particle.main;
        var startCol = module_.startColor.color;
        startCol.a = v;
        module_.startColor = new ParticleSystem.MinMaxGradient(startCol);
        /////////////
        ///

        //Debug.Log("velocity _ " + v);

    }

    int latest_trigger_value = 0;
    public void _particle_trigger(int v)
    {
        Debug.Log("trigger _ " + v);

        if (v == 0)
            particle.Clear();

        if(latest_trigger_value != v)
        {

            if (v == 3)
                emitParticle_L();
            else if (v == 2)
                emitParticle_M();
            else if (v == 1)
                emitParticle_S();
        }

        latest_trigger_value = 0;
    }

    public bool debug = false;
    private void Update()
    {
        if(debug==true)
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                emitParticle_L();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                emitParticle_M();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                emitParticle_S();
            }
        }
    }

    //void emitParticle(float size)
    //{
    //    particle.Clear();

    //    //size
    //    var module = particle.main;
    //    module.startSize = new ParticleSystem.MinMaxCurve(startSize.x/size, startSize.y/size);
    //    module.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed.x, startSpeed.y);
    //    particle.Play();
    //}

    void emitParticle_L()
    {
        particle.Clear();

        //size
        var module = particle.main;
        module.startSize = new ParticleSystem.MinMaxCurve(startSize.x, startSize.y);
        module.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed.x, startSpeed.y);
        particle.Play();
    }

    void emitParticle_M()
    {
        particle.Clear();
        var module = particle.main;
        module.startSize = new ParticleSystem.MinMaxCurve(startSize.x/2, startSize.y/2);
        module.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed.x/10, startSpeed.y/10);

        particle.Play();
    }

    void emitParticle_S()
    {
        particle.Clear();
        var module = particle.main;
        module.startSize = new ParticleSystem.MinMaxCurve(startSize.x / 4, startSize.y / 4);
        module.startSpeed = new ParticleSystem.MinMaxCurve(startSpeed.x / 20, startSpeed.y / 20);

        particle.Play();
    }




    // public void _playByInputVal(float v)
    // {
    //     Debug.Log(v);

    //     if (canTrigger && v >= 0.6f)
    //     {
    //         // 発火する処理をここに書く
    //         //light.intensity = 15;
    //         //light.intensity = v.Map(0, 0.6f, 0, 15);
    //         animationScript._stop_images();

    //         animationScript._play_images();
    //         canTrigger = false; // 発火後は再発火を防ぐためにフラグを下ろす
    //     }
    //     //else if (v < 0.4f)
    //     //{

    //     //}
    //     else if (!canTrigger && v <= 0.59f)
    //     {
    //         // valueが0.1以下になったら、再度発火可能にする
    //         //StartCoroutine(DimLightCoroutine(light, 1.2f));
    //         //light.intensity = v.Map(0, 0.6f, 0, 15);

    //         canTrigger = true;
    //     }

    //     light.intensity = v.Map(0, 0.6f, 0, 15);


    // }

    // IEnumerator DimLightCoroutine(Light light, float duration)
    // {
    //     float startIntensity = light.intensity;
    //     float time = 0;

    //     while (time < duration)
    //     {
    //         light.intensity = Mathf.Lerp(startIntensity, 0, time / duration);
    //         time += Time.fixedDeltaTime;
    //         yield return null; // Wait until the next frame
    //     }

    //     light.intensity = 0; // Ensure the intensity is set to 0 at the end

    //     animationScript._stop_images();

    // }
}
