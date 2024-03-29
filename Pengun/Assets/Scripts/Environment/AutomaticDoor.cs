using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    public float openDistance = 2f;
    public float openSpeed = 2f;
    public float closeSpeed = 3f;

    private float targetY;
    private bool isOpening;
    private bool isClosing;
    private bool isOpen;
    private bool soundPlayed;

    public Transform doorTransform;
    AudioSource audioSource;
    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        targetY = doorTransform.position.y + openDistance;
    }

    private void Update()
    {
        if (isOpening)
        {
            doorTransform.position = Vector2.MoveTowards(doorTransform.position, new Vector2(doorTransform.position.x, targetY), openSpeed * Time.deltaTime);
            if (!soundPlayed)
            {
                PlayOpenSound();
            }
            if (doorTransform.position.y == targetY)
            {
                isOpen = true;
                isOpening = false;
                soundPlayed = false;
            }
        }
        else if (isClosing)
        {
            doorTransform.position = Vector2.MoveTowards(doorTransform.position, new Vector2(doorTransform.position.x, targetY - openDistance), closeSpeed * Time.deltaTime);
            if (!soundPlayed)
            {
                PlayCloseSound();
            }
            if (doorTransform.position.y == targetY - openDistance)
            {
                isOpen = false;
                isClosing = false;
                soundPlayed = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isOpen)
            {
                isOpening = true;
                isClosing = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOpening)
            {
                isClosing = true;
                isOpening = false;
            }
            else if (isOpen)
            {
                isClosing = true;
                isOpen = false;
            }
        }
    }

    private void PlayOpenSound()
    {
        audioSource.clip = openDoorSound;
        audioSource.Play();
        soundPlayed = true;
    }

    private void PlayCloseSound()
    {
        audioSource.clip = closeDoorSound;
        audioSource.Play();
        soundPlayed = true;
    }
}
