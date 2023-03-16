using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
            agent.Move(movement * agent.speed * Time.deltaTime);
        }

        if (Input.GetMouseButton(0))
        {
            // Cast a ray from the mouse position to the terrain to get the target location
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // Set the destination of the NavMesh Agent to the target location
                agent.SetDestination(hit.point);
            }
        }

    }
}
