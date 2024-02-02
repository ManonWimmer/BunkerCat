using UnityEngine;

public class Scaler : MonoBehaviour
{
    // ----- VARIABLES ----- //
    public bool started = false;
    public bool back = false;
    public bool finished = false;

    public float scaleSpeed = 2f;
    private float initialScaleY;
    public float endScaleY;
    // ----- VARIABLES ----- //

    private void Start()
    {
        initialScaleY = transform.localScale.y;
        gameObject.SetActive(false);
        scaleSpeed = 2f;
    }

    void Update()
    {
        if (started && !finished)
        {
            scaleSpeed = scaleSpeed * 1.001f;
            if ((endScaleY - transform.localScale.y) < .1f && !back) // Finito
            {
                back = true;
            }
            else if ((transform.localScale.y - initialScaleY) < .1f && back)
            {
                finished = true;
                gameObject.SetActive(false);
            }

            if (transform.CompareTag("Platforms") || transform.CompareTag("Light"))
            { // Si c'est une plateforme, peut se déplacer
                if (!back)
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + Time.deltaTime * scaleSpeed, transform.localScale.z);
                }
                else // Retour
                {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - Time.deltaTime * scaleSpeed, transform.localScale.z);
                }

            }

        }
    }

    private void SetStartScale()
    {
        transform.localScale = new Vector2 (transform.localScale.x, initialScaleY);
    }

    public void StartScale()
    {
        SetStartScale();
        gameObject.SetActive(true);
        started = true;
        finished = false;
        back = false;
    }
}
