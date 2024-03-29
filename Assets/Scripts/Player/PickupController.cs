using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public PlayerMovement player;
    public Transform equipPosition;
    public Transform Hand;
    public float distance = 10f;
    Vector3 offset = new Vector3(0f,0f,0.2f);
    GameObject currentWeapon;
    GameObject wp;
    bool canGrab;

    private void Update()
    {
        CheckWeapons();
        if (canGrab)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(currentWeapon != null)
                {
                    Drop();
                }
                PickUp();
            }
        }
        if (currentWeapon != null)
        {
            equipPosition.transform.position = Hand.transform.position;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }
        }
    }

    private void CheckWeapons()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
        {
            if(hit.transform.tag == "CanGrab")
            {
                canGrab = true;
                wp = hit.transform.gameObject;       
            }
        }
        else
        {
            canGrab=false;
        }
    }

    private void PickUp()
    {
        player.animator.SetGunPickedUp(true);
        currentWeapon = wp;
        currentWeapon.transform.position = equipPosition.position;
        currentWeapon.transform.parent = equipPosition;
        currentWeapon.layer = 8;
        currentWeapon.transform.localEulerAngles = Camera.main.transform.forward;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;
        player.animator.PickupAnimation();
    }
    private void Drop()
    {
        player.animator.SetGunPickedUp(false);
        currentWeapon.transform.parent = null;
        currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
        currentWeapon.GetComponent<Rigidbody>().velocity += Camera.main.transform.up * 6 + Camera.main.transform.forward * 6;
        Vector3 torque;
         torque.x = Random.Range(-200, 200);
        torque.y = Random.Range(-200, 200);
        torque.z = Random.Range(-200, 200);
        currentWeapon.GetComponent<Rigidbody>().AddTorque(torque);
        currentWeapon.layer = 7;
        currentWeapon = null;
        player.animator.PickupAnimation();
    }
}
