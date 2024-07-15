using System;
using UnityEngine;
using UnityEngine.Playables;
using static Enums;

public class PlayerScript : MonoBehaviour
{
    public float fixedDistance = 1000f;
    public static Action OnPlayerHit;
    public float yHeight = 0f;
    public float zDistance = -350;
    public float moveSpeed = 10f;
    public bool freeMovement = false;
    public bool gameOver = false;
    public float amplitude = 1.0f;  
    public float frequency = 1.0f; 
    public ControlScheme controlScheme;

    private Vector3 originalPosition;
    public GameManager gameManager;
    public AudioSource engineSound;

    public SaveData saveData;
    private KeyBinding keyBindings;

    public float detectionRadius = 10.0f;
    public LayerMask asteroidLayer;
    public float alertCooldown = 2.0f; // Time between alerts
    private float lastAlertTime;


    void Start()
    {
        saveData = SaveSystem.LoadGame();
        gameManager = GameManager.Instance;
        originalPosition = transform.position;

        controlScheme = saveData.controlType;
        keyBindings = saveData.keyBindings;
        lastAlertTime = -alertCooldown;
        moveSpeed = saveData.moveSpeed;

        if (!saveData.playedTutorial)
        {
            gameManager.StartTutorial();
        }
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
        if (!saveData.infinteMode)
        {
            if (collider.CompareTag("Enemy"))
            {
                AudioManager.Instance.PlaySoundEffect("Impact");
                gameOver = true;
                OnPlayerHit?.Invoke();

            }
        }
        // Check for player
      
    }

    void OnTriggerExit(Collider collider)
    {
        if (!saveData.infinteMode)
        {
            if (collider.CompareTag("Enemy"))
            {
                AudioManager.Instance.PlaySoundEffect("Impact");
                gameOver = true;
                OnPlayerHit?.Invoke();

            }
        }
         
    }

    void Update()
    {
        if (!gameOver)
        {
            DetectAsteroids();

            Vector3 targetPosition = transform.position;
            switch (controlScheme)
            {
                case ControlScheme.MouseFree:
                    targetPosition = GetMouseFreePosition();
                    break;
                case ControlScheme.MouseFixedX:
                    targetPosition = GetMouseFixedXPosition();
                    break;
                case ControlScheme.KeyboardFixedX:
                    targetPosition = GetKeyboardFixedXPosition();
                    break;
                case ControlScheme.FreeMovementKeyboard:
                    targetPosition = GetFreeMovementKeyboardPosition();
                    break;
            }

            //// Calculate floating offset using sine waves for smooth floating motion
            //float newY = originalPosition.y + amplitude * Mathf.Sin(Time.time * frequency);
            //float newX = originalPosition.x + amplitude * Mathf.Cos(Time.time * frequency * 0.5f);
            //float newZ = originalPosition.z + amplitude * Mathf.Sin(Time.time * frequency * 0.3f);

            //// Apply floating offset to the target position
            //Vector3 floatingOffset = new Vector3(newX, newY, newZ);
            //Vector3 finalPosition = targetPosition + floatingOffset;

            // Clamp the position to keep the player within the camera view
            Vector3 clampedPosition = ClampPositionToViewport(targetPosition);

            transform.position = clampedPosition;

        }
        else
        {
            engineSound.Stop();
            engineSound.loop = false;
        }
    }


    Vector3 CalculateFloatingOffset()
    {
        float newY = amplitude * Mathf.Sin(Time.time * frequency);
        float newX = amplitude * Mathf.Cos(Time.time * frequency * 0.5f);
        float newZ = amplitude * Mathf.Sin(Time.time * frequency * 0.3f);

        return new Vector3(newX, newY, newZ);
    }

    Vector3 GetMouseFreePosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        return ray.origin + (ray.direction.normalized * fixedDistance);

        //Vector3 mouseScreenPos = Input.mousePosition;
        //Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        //Vector3 targetPos = ray.origin + (ray.direction.normalized * fixedDistance);

        //Vector3 delta = targetPos - transform.position;
        //delta = Vector3.ClampMagnitude(delta, moveSpeed * Time.deltaTime);

        //return transform.position + delta;
    }

    Vector3 GetMouseFixedXPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        Vector3 objPos = ray.origin + (ray.direction.normalized * fixedDistance);
        return new Vector3(objPos.x, yHeight, zDistance);

        //Vector3 mouseScreenPos = Input.mousePosition;
        //Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        //Vector3 targetPos = ray.origin + (ray.direction.normalized * fixedDistance);
        //targetPos = new Vector3(targetPos.x, yHeight, zDistance);

        //Vector3 delta = targetPos - transform.position;
        //delta = Vector3.ClampMagnitude(delta, moveSpeed * Time.deltaTime);

        //return transform.position + delta;
    }

    Vector3 GetKeyboardFixedXPosition()
    {
        float moveX = 0f;

        if (Input.GetKey(keyBindings.moveLeft))
        {
            moveX = -moveSpeed * Time.deltaTime;
            //Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime
        }
        else if (Input.GetKey(keyBindings.moveRight))
        {
            moveX = moveSpeed * Time.deltaTime;
        }

        Vector3 newPosition = transform.position + new Vector3(moveX, 0, 0);
        return new Vector3(newPosition.x, yHeight, zDistance);
    }

    Vector3 GetFreeMovementKeyboardPosition()
    {
        //float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(keyBindings.moveLeft))
        {
            moveX = -moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(keyBindings.moveRight))
        {
            moveX = moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(keyBindings.moveUp))
        {
            moveY = moveSpeed * Time.deltaTime;

        }
        else if (Input.GetKey(keyBindings.moveDown))
        {
            moveY = -moveSpeed * Time.deltaTime;

        }

        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0);
        return newPosition;
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

    void DetectAsteroids()
    {
        if (saveData.audioAlerts)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, asteroidLayer);

            if (hits.Length > 0 && Time.time - lastAlertTime >= alertCooldown)
            {
                AudioManager.Instance.PlaySoundEffect("Incoming");
                lastAlertTime = Time.time;
            }
        }
       
    }
}
