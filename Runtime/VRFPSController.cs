using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class VRFPSController : MonoBehaviour
{
    
    private VREmulator _vrEmulator;
    

    
    
    
    void Start()
    {
        _vrEmulator = GetComponent<VREmulator>();
        
        
        
    }
    
    void Update()
    {
        HandleMovement();

        Size();
        
   
    }


   private enum HeightAction
    {
        Normal =0,
        Jump = 1,
        Crouch = 2,
    }
    
    
    private float currentHeight; // Variable de classe pour persister l'état

void Size()
{
    
    if (!_vrEmulator || !_vrEmulator.cameraPlayer) return;

    
    float baseHeight = _vrEmulator.slider.value;
    HeightAction action = HeightAction.Normal;
    
    
#if ENABLE_INPUT_SYSTEM
        // Nouveau système d'input
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.ctrlKey.isPressed)
                action = HeightAction.Crouch;
            
            if (keyboard.spaceKey.isPressed)
                action = HeightAction.Jump;
            
        }
        
#else
        // Ancien système d'input
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) 
            action = HeightAction.Crouch;
        
        if (Input.GetKey(KeyCode.Space)) 
            action = HeightAction.Jump;
#endif

    
    float targetHeight = baseHeight;
    if(action == HeightAction.Jump)
        targetHeight += baseHeight * 0.4f;
    else if(action == HeightAction.Crouch)
        targetHeight -= baseHeight * 0.3f;
    
    // Initialiser currentHeight si nécessaire
    if (currentHeight == 0) currentHeight = baseHeight;
    
    currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * 5f);
    
    _vrEmulator.cameraPlayer.transform.position = new Vector3(
        _vrEmulator.cameraPlayer.transform.position.x,
        currentHeight,
        _vrEmulator.cameraPlayer.transform.position.z
    );
}
    
    
    void HandleMovement()
    {
        
        if (!_vrEmulator || !_vrEmulator.cameraPlayer) return;
        
        Vector2 inputVector = GetMovementInput();
    
        // Utiliser l'orientation de la CAMÉRA pour calculer la direction
        Transform cameraTransform = _vrEmulator.cameraPlayer.transform;
        Vector3 direction = cameraTransform.right * inputVector.x + cameraTransform.forward * inputVector.y;
        direction.y = 0;
        direction = Vector3.ClampMagnitude(direction, 1f);
    
        // Application de la vitesse
        float currentSpeed =  SimulatorSettings.settings.MoveSpeed;;
      
        // Déplacement de la caméra
        _vrEmulator.cameraPlayer.transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
    }

    
    Vector2 GetMovementInput()
    {
        Vector2 input = Vector2.zero;
        
#if ENABLE_INPUT_SYSTEM
        // Nouveau système d'input
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.leftArrowKey.isPressed) input.x -= 1f;     // Gauche
            if (keyboard.rightArrowKey.isPressed) input.x += 1f;    // Droite
            if (keyboard.downArrowKey.isPressed) input.y -= 1f;     // Reculer
            if (keyboard.upArrowKey.isPressed) input.y += 1f;       // Avancer
        }
#else
        // Ancien système d'input
        if (Input.GetKey(KeyCode.LeftArrow)) input.x -= 1f;     // Gauche
        if (Input.GetKey(KeyCode.RightArrow)) input.x += 1f;    // Droite
        if (Input.GetKey(KeyCode.DownArrow)) input.y -= 1f;     // Reculer
        if (Input.GetKey(KeyCode.UpArrow)) input.y += 1f;       // Avancer
#endif
        
        return input;
    }
    
}