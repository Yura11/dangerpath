using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class onlinecarmove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 250f;
    [SerializeField] private AudioSource Soundaffect;
    [SerializeField] private AudioSource Soundaffect1;
    [SerializeField] private AudioSource Soundaffect2;

    private bool isButtonPressed;
   // private PhotonView View;

    private void Start()
    {
        if (Soundaffect1) Soundaffect1.Play();
        isButtonPressed = false;
       // View = GetComponent<PhotonView>();
    }

    private void Update()
    {
       /* if (View && View.IsMine)
        {
            HandleMovement();
            HandleSoundEffects();
        }*/
    }

    private void HandleMovement()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * -rotationSpeed * Time.deltaTime;

        transform.Translate(0f, move, 0f);
        transform.Rotate(0f, 0f, rotation);
    }

    private void HandleSoundEffects()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isButtonPressed = true;
            if (Soundaffect) Soundaffect.Play();
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isButtonPressed = false;
            if (Soundaffect) Soundaffect.Stop();
            if (Soundaffect2) Soundaffect2.Play();
        }

        if (isButtonPressed && Soundaffect && !Soundaffect.isPlaying)
        {
            Soundaffect.Play();
            if (Soundaffect2) Soundaffect2.Stop();
        }
        else if (!isButtonPressed && Soundaffect && Soundaffect.isPlaying)
        {
            Soundaffect.Stop();
            if (Soundaffect2) Soundaffect2.Play();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FaltTag"))
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
