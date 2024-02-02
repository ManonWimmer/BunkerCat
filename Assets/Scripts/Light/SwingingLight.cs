using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering.Universal;
using UnityEditor.Rendering;

public class SwingingLight : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public float swingSpeed = 1f;
    public float suspensionHeight = 10f;
    public float swingAmplitude = 20f;
    private Vector2 rotationPoint;
    private bool catInLight;
    private new Light2D light;

    public float timeBeforeChange = .5f;

    private float currentTime = 0f;
    private int colorIndex = 0;
    private Color[] colors = new Color[] { Color.white, Color.yellow, Color.red };

    // ----- VARIABLES ----- //

    private void Start()
    {
        light = transform.GetComponent<Light2D>();
    }

    private void Update()
    {
        float swingAngle = Mathf.Sin(Time.time * swingSpeed) * swingAmplitude;
        transform.rotation = Quaternion.Euler(0, 0, swingAngle);
        rotationPoint = new Vector2(transform.position.x, transform.position.y + suspensionHeight);

        // Increment the current time
        currentTime += Time.deltaTime;

        // Check if it's time to change the color
        if (currentTime >= timeBeforeChange && catInLight)
        {
            // Reset the current time
            currentTime = 0f;

            // Increment the color index
            colorIndex++;
            if (colorIndex >= colors.Length)
            {
                colorIndex = 0;
            }

            // Change the light color
            light.color = colors[colorIndex];

            if (light.color == Color.red)
            {
                PlayerHealthController.GetInstance().DealDamage();
            }
        }
        else if (!catInLight)
        {
            if (light.color == Color.red || light.color == Color.yellow)
            {
                colorIndex--;
            }

            light.color = colors[colorIndex];
        }
    }

        

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("chat dans la lumière");
            catInLight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("chat pas dans la lumière");
            catInLight = false;
        }

    }
}
