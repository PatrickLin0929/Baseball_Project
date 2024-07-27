using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoTeams_CameraController : MonoBehaviour
{
    int distance = -10;
    float lift = 1.5f;

    //string targetGameObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(TwoTeam_SharedData.dynamicViewPoint && !TwoTeam_SharedData.startTesting) {
            if(TwoTeam_SharedData.pitchThrown) {
                transform.position = GameObject.Find("CubePitcher").transform.position + new Vector3(0, lift, distance);
            } else {
                transform.position = GameObject.Find("CubeHitter").transform.position + new Vector3(0, lift, distance);
            }
        } else {
            transform.position = new Vector3( 0.11f, -0.64f, -21.71f );
        }
    }
}