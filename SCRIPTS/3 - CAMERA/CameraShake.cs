using UnityEngine;
using Cinemachine;
using System.ComponentModel;

public class CameraShake : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    public void Shake(float intensity = 1f)
    {
        if(impulseSource != null)
        {
            impulseSource.GenerateImpulse(intensity);
        }
    }
}