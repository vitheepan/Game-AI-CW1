using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOTStatus : MonoBehaviour
{
    public Text statusText;
    
    public BOT1FSM npc;

    // Update is called once per frame
    void Update()
    {
        statusText.text = "State: " + npc.currentState.ToString();
    }
}
