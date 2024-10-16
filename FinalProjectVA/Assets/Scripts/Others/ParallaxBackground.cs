using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    //Start position for the parallax game object
    Vector2 startPosition;

    //Start Z value of the parallax game object
    float startZ;

    //Distance that the camera has moved from the start pos of the parallax object
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    //If object is in front of target, use near clip plane. Or behind target, use far clip plane
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    //
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //When the target moves, move the parallax object the same distance times a multiplier
        Vector2 newPosition = startPosition + camMoveSinceStart * parallaxFactor;
        //The X/Y pos changes based on target travel speed times the parallax factor, but Z stays consistent
        transform.position = new Vector3(newPosition.x, newPosition.y, startZ);
    }
}
