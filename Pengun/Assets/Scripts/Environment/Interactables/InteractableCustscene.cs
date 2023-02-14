using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class InteractableCustscene : MonoBehaviour
{
    public Transform teleportTarget;
    private Collider2D playerCollider;
    public static bool canInteractCutscene = false;

    //Attach Canvas as child for worldSpace
    public Canvas canvas;

    public Canvas videoCanvas;
    public VideoPlayer cutsceneVideoPlayer;

    private void Start()
    {
        canvas.enabled = false;
        videoCanvas.enabled = false;
        cutsceneVideoPlayer.loopPointReached += EndCutscene;
    }

    private void Update()
    {
        if (playerCollider != null)
        {
            if (Input.GetButtonDown("Melee"))
            {
                cutsceneVideoPlayer.enabled = true;
                videoCanvas.enabled = true;
                cutsceneVideoPlayer.Play();
            }
        }

        if (Input.GetButtonDown("Melee") && cutsceneVideoPlayer.isPlaying)
        {
            cutsceneVideoPlayer.time = cutsceneVideoPlayer.length;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteractCutscene = true;
            canvas.enabled = true;
            playerCollider = collision;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteractCutscene = false;
            canvas.enabled = false;
            playerCollider = null;
        }
    }

    private void TeleportPlayer(Collider2D newCollision)
    {
        newCollision.transform.position = teleportTarget.position;
    }

    private void EndCutscene(VideoPlayer source)
    {
        videoCanvas.enabled = false;
        cutsceneVideoPlayer.enabled = false;
        TeleportPlayer(playerCollider);
    }
}
