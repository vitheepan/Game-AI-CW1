using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public NavMeshAgent agent;

    public GameObject bulletPrefab;
    public Transform firePoint;

    Vector3 movementDirection;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movementDirection = movement.normalized;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            transform.LookAt(transform.position + movementDirection);
            agent.Move(movement * agent.speed * Time.deltaTime);
        }
     
        //if (Input.GetMouseButton(0))
        //{
            
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit))
            //{
                // Set the destination of the NavMesh Agent to the target location
                //agent.SetDestination(hit.point);
            //}
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * 1000);
        Destroy(bullet, 2f);
    }
}
