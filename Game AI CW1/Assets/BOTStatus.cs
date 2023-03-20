using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOTStatus : MonoBehaviour
{
    public Text statusText;
    public BOT1 botOne;

    // Update is called once per frame
    private void Update()
    {
        if (botOne.playerInAttackRange)
        {
            statusText.text = "STATUS: ATTACK";
        }
        else if (botOne.playerInSightRange)
        {
            statusText.text = "STATUS: CHASE";
        }
        else
        {
            statusText.text = "STATUS: PATROL";
        }
    }
}
