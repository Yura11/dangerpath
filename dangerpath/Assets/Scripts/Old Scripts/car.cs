using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class carmove : MonoBehaviour
{
    private float move, moveSpeed, rotation, rotationSpeed;
    [SerializeField] private AudioSource Soundaffect;
    [SerializeField] private AudioSource Soundaffect1;
    [SerializeField] private AudioSource Soundaffect2;
    private bool isButtonPressed;

 //   PhotonView View;

    // Start is called before the first frame update
    void Start()
    {
        Soundaffect1.Play();
        moveSpeed = 5f;
        rotationSpeed = 250f;   
        isButtonPressed = false;
       // View = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
       
            move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            rotation = Input.GetAxis("Horizontal") * -rotationSpeed * Time.deltaTime;

            transform.Translate(0f, move, 0f);
            transform.Rotate(0f, 0f, rotation);

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isButtonPressed = true;
                Soundaffect.Play(); // Play the sound effect
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                isButtonPressed = false;
                Soundaffect.Stop(); // Stop the sound effect
                Soundaffect2.Play();
            }

            if (isButtonPressed && !Soundaffect.isPlaying)
            {
                Soundaffect.Play(); // Play the sound effect if button is pressed and sound is not already playing
                Soundaffect2.Stop();
            }
            else if (!isButtonPressed && Soundaffect.isPlaying)
            {
                Soundaffect.Stop(); // Stop the sound effect if button is released and sound is currently playing
                Soundaffect2.Play();
            
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "falt2" || collision.gameObject.name == "falt" || collision.gameObject.name == "falt3")
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}

