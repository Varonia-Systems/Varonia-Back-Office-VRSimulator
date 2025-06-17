using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[DefaultExecutionOrder(0)]
public class MouseLookHybrid : MonoBehaviour
{
    private VREmulator _vrEmulator;
   
    private float _xRotation = 0f;
    private float _yRotation = 0f; // Ajout pour gérer la rotation Y

    void Start()
    {
        _vrEmulator = GetComponent<VREmulator>();

        // Vérification de sécurité
        if (_vrEmulator == null)
        {
            Debug.LogError("VREmulator component not found on " + gameObject.name);
            enabled = false;
            return;
        }

        if (_vrEmulator.cameraPlayer == null)
        {
            Debug.LogError("cameraPlayer is not assigned in VREmulator on " + gameObject.name);
            enabled = false;
            return;
        }

        
    }

    void Update()
    {
        // Vérification de sécurité supplémentaire
        if (_vrEmulator?.cameraPlayer == null) return;

        Vector2 lookDelta = Vector2.zero;

#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null)
        {
            lookDelta = Mouse.current.delta.ReadValue();
        }
#else
        lookDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
#endif

        float mouseX = lookDelta.x * SimulatorSettings.settings.MouseSensitivity * Time.deltaTime;
        float mouseY = lookDelta.y *  SimulatorSettings.settings.MouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _yRotation += mouseX;

        // Application cohérente de la rotation
        _vrEmulator.cameraPlayer.transform.localRotation =
            Quaternion.Euler(_xRotation, _yRotation, 0f);
    }


    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}