using System;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float fixedDistance = 1000f;
    public static Action OnPlayerHit;
    public float yHeight = 0f;
    public float zDistance = -350;
    public bool freeMovement = false;
    public bool gameOver = false;
    public float amplitude = 1.0f;  // The maximum displacement from the original position
    public float frequency = 1.0f;  // The speed of the oscillation

    private Vector3 originalPosition;
    public GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        // Store the original position of the object
        originalPosition = transform.position;
    }

    void OnEnable()
    {
        GameManager.OnPauseGame += PauseGame;
        GameManager.OnResumeGame += ResumeGame;
    }

    void OnDisable()
    {
        GameManager.OnPauseGame -= PauseGame;
        GameManager.OnResumeGame -= ResumeGame;
    }

    void OnTriggerEnter(Collider collider)
    {
        // Check for player
        if (collider.CompareTag("Enemy"))
        {
            //var shieldActive = GameManager.Instance.shieldActive;
            //if (shieldActive)
            //{

            //    GameManager.OnShieldHit?.Invoke();
            //}
            //else
            //{
            //    gameOver = true;
            //    OnPlayerHit?.Invoke();
            //}
            Debug.Log("ShieldInactive");
            gameOver = true;
            OnPlayerHit?.Invoke();

        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Check for player
        if (collider.CompareTag("Enemy"))
        {
            //var shieldActive = GameManager.Instance.shieldActive;
            //if (shieldActive)
            //{
            //    Debug.Log("ShieldActive");
            //    GameManager.OnShieldHit?.Invoke();
            //}
            //else
            //{
            //    Debug.Log("ShieldInactive");
            //    gameOver = true;
            //    OnPlayerHit?.Invoke();
            //}

            Debug.Log("ShieldInactive");
            gameOver = true;
            OnPlayerHit?.Invoke();

        }
    }

    void Update()
    {
        if (!gameOver)
        {
            // Get the mouse position in screen space
            Vector3 mouseScreenPos = Input.mousePosition;

            // Convert the screen space mouse position to world space
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
            Vector3 objPos = ray.origin + (ray.direction.normalized * fixedDistance);

            Vector3 targetPosition;

            if (freeMovement)
            {
                targetPosition = objPos;
            }
            else
            {
                targetPosition = new Vector3(objPos.x, yHeight, zDistance);
            }

            // Calculate floating offset using sine waves for smooth floating motion
            float newY = originalPosition.y + amplitude * Mathf.Sin(Time.time * frequency);
            float newX = originalPosition.x + amplitude * Mathf.Cos(Time.time * frequency * 0.5f);
            float newZ = originalPosition.z + amplitude * Mathf.Sin(Time.time * frequency * 0.3f);

            // Apply floating offset to the target position
            Vector3 floatingOffset = new Vector3(newX, newY, newZ);
            Vector3 finalPosition = targetPosition + floatingOffset;

            // Clamp the position to keep the player within the camera view
            Vector3 clampedPosition = ClampPositionToViewport(finalPosition);

            transform.position = clampedPosition;
        }
    }

    void PauseGame()
    {
        gameOver = true;
    }

    void ResumeGame()
    {
        gameOver = false;
    }

    Vector3 ClampPositionToViewport(Vector3 position)
    {
        Camera cam = Camera.main;

        Vector3 minViewportPos = cam.ViewportToWorldPoint(new Vector3(0, 0, fixedDistance));
        Vector3 maxViewportPos = cam.ViewportToWorldPoint(new Vector3(1, 1, fixedDistance));

        float clampedX = Mathf.Clamp(position.x, minViewportPos.x, maxViewportPos.x);
        float clampedY = Mathf.Clamp(position.y, minViewportPos.y, maxViewportPos.y);
        float clampedZ = Mathf.Clamp(position.z, minViewportPos.z, maxViewportPos.z);

        return new Vector3(clampedX, clampedY, clampedZ);
    }
}
