using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMenu : MonoBehaviour
{
    [SerializeField] private Transform backgroundLayer; // The background layer to move.
    [SerializeField] private float parallaxFactor; // The proportion of the camera's movement to move the background layer by.
    [SerializeField] private float loopDuration; // The duration of each loop in seconds.

    private float layerStartPositionX; // The starting X position of the background layer.
    private float loopStartTime; // The time when the current loop started.

    void Start()
    {
        layerStartPositionX = backgroundLayer.position.x;
        loopStartTime = Time.time;
    }

    void Update()
    {
        float loopTime = Time.time - loopStartTime;
        float loopPhase = Mathf.PingPong(loopTime / loopDuration, 1f);

        float parallax = layerStartPositionX + parallaxFactor * loopPhase;
        Vector3 backgroundTarget = new Vector3(parallax, backgroundLayer.position.y, backgroundLayer.position.z);
        backgroundLayer.position = backgroundTarget;

        if (loopTime >= loopDuration * 2f)
        {
            loopStartTime = Time.time;
        }
    }
}
