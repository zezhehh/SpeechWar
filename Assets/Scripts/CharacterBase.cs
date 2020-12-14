﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Separate class for speech input
public static class MoveStatus
{
    public const string Still = "stop";
    public const string Left = "left";
    public const string Right = "right";
    public const string Up = "up";
    public const string Down = "down";
}

public class CharacterBase : MonoBehaviour
{

    [SerializeField] private Transform pfBullet;     // Is needed to instantiate bullet objects
    public float speed;
    public float rotationSpeed;
    public float range;        // Indicates after how many seconds the bullet disappears
    public int power;          // Indicates how much health a bullet takes
    string moveStatus;  // Will be used for speech control

    [HideInInspector] public bool mode;              // Whether moving or aiming, true = moving, false = aiming

    void Start()
    {
        moveStatus = MoveStatus.Still;
    }

    // Updates move status, is done in SpeechController
    public void UpdateMoveStatus(string status)
    {
        moveStatus = status;
        Debug.Log("Updated moveStatus to " + status);
    }

    // Method to switch between aiming and moving
    private void SwitchMode()
    {
        mode = !mode;
    }

    // Move x,y position on the map
    private void MovePosition()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, yDirection, 0.0f);

        transform.position += moveDirection * speed;
    }

    // Move direction on the map
    private void MoveDirection()
    {
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // forward is z-axis (towards us)

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    // Catch-all method that calls the right function depending on what mode we are in
    public void MoveCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchMode();
        }

        if (mode)
        {
            MovePosition();
        }
        else if (!mode)
        {
            MoveDirection();
        }
    }

    public void ShootBullet()
    {
        Vector3 currentPosition = transform.position + (transform.right * 1); // Spawn bullet in front of character, 1 might be subject to change
        Transform bulletTransform = Instantiate(pfBullet, currentPosition, Quaternion.identity); // Create new bullet prefab

        Quaternion bulletDirection = transform.localRotation;
        bulletTransform.GetComponent<BulletShot>().Setup(bulletDirection, range, power);
    }

    void Update()
    {
        MoveCharacter();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShootBullet();
        }
    }
}