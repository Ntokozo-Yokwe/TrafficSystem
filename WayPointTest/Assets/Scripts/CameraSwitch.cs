using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera camScene;
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;

    AudioListener camSceneAudio;
    AudioListener cam1Audio;
    AudioListener cam2Audio;
    AudioListener cam3Audio;

    void Start()
    {
        camSceneAudio = camScene.GetComponent<AudioListener>();
        cam1Audio = cam1.GetComponent<AudioListener>();
        cam2Audio = cam2.GetComponent<AudioListener>();
        cam3Audio = cam3.GetComponent<AudioListener>();

        camSceneAudio.enabled = true;
        camScene.enabled = true;
        cam1Audio.enabled = false;
        cam2Audio.enabled = false;
        cam3Audio.enabled = false;
        cam1.enabled = false;
        cam2.enabled = false;
        cam3.enabled = false;
    }
    
    public void SwitchCamera(int x)
    {
        deactivateall();

        if (x == 1)
        {
            camScene.enabled = true;
            camSceneAudio.enabled = true;
            cam1Audio.enabled = false;
            cam2Audio.enabled = false;
            cam3Audio.enabled = false;
        }
        else if (x == 2)
        {
            cam1.enabled = true;
            cam1Audio.enabled = true;
            camSceneAudio.enabled = false;
            cam2Audio.enabled = false;
            cam3Audio.enabled = false;
        }
        else if (x == 3)
        {
            cam2.enabled = true;
            cam2Audio.enabled = true;
            camSceneAudio.enabled = false;
            cam1Audio.enabled = false;
            cam3Audio.enabled = false;
        }
        else
        {
            cam3.enabled = true;
            cam3Audio.enabled = true;
            camSceneAudio.enabled = false;
            cam1Audio.enabled = false;
            cam2Audio.enabled = false;
        }

        
    }

    public void deactivateall()
    {
        camScene.enabled = false;
        cam1.enabled = false;
        cam2.enabled = false;
        cam3.enabled = false;
    }
}
