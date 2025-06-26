using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using VaroniaBackOffice;

[DefaultExecutionOrder(-100)]
public class VrInput : VrItem
{

 #if VBO_Input
    
    [Header("Smoothing")]
    public float rotationSmoothing = 0.05f; // Très rapide

    
    private Quaternion lastCameraRotation;
    private Quaternion targetRotation;

    
    
   public override void Start()
    {
        base.Start();
        StartCoroutine(CheckReady());
    }



    IEnumerator CheckReady()
    {
        yield return new WaitUntil(() => VREmulator.cameraPlayer != null);
        yield return new WaitUntil(() => VaroniaInput.Instance != null);
        yield return new WaitUntil(() =>  VaroniaInput.Instance.Tracking != null);

       
        
        IsInit = true;
    }



    public override void Update()
    {
            base.Update();
            
            
            

            if (Application.isFocused && VREmulator.IsActive)
            {

// Update mouse input system
                    MouseHook.Update();

// Handle left mouse button (primary action)
                    if (MouseHook.GetMouseButtonDown(0))
                            VaroniaInput.Instance.EventPrimaryDown.Invoke();

                    if (MouseHook.GetMouseButtonUp(0))
                            VaroniaInput.Instance.EventPrimaryUp.Invoke();

// Handle right mouse button (secondary action)
                    if (MouseHook.GetMouseButtonDown(1))
                            VaroniaInput.Instance.EventSecondaryDown.Invoke();

                    if (MouseHook.GetMouseButtonUp(1))
                            VaroniaInput.Instance.EventSecondaryUp.Invoke();

// Handle middle mouse button (reload action)
                    if (MouseHook.GetMouseButtonDown(2))
                            VaroniaInput.Instance.EventReloadDown.Invoke();

                    if (MouseHook.GetMouseButtonUp(2))
                            VaroniaInput.Instance.EventReloadUp.Invoke();

// Finalize mouse input processing
                    MouseHook.LateUpdate();

            }
            
    }
    
    
   public void LateUpdate()
    {
        
        if(!IsInit || VREmulator.cameraPlayer==null || !VREmulator.uI.activeSelf )
            return;
        



// Get camera rotation for reference
        Quaternion cameraRotation = VREmulator.cameraPlayer.transform.rotation;
// Calculate pivot compensation to counteract pivot rotation
        Quaternion pivotCompensation = Quaternion.Inverse(VaroniaInput.Instance.Pivot.localRotation);
// Apply desired rotation with 90-degree Y offset
        Quaternion desiredRotation = Quaternion.Euler(fakeRot + new Vector3(0, 90, 0));

// Set final rotation combining camera, desired rotation, and pivot compensation
        VaroniaInput.Instance.Tracking.rotation = cameraRotation * desiredRotation * pivotCompensation;


        
        
        // Calculate pivot offset in world space
        Vector3 pivotWorldOffset = VaroniaInput.Instance.Tracking.TransformVector(VaroniaInput.Instance.Pivot.localPosition);
// Transform fake gun position to camera space
        Vector3 fakeGunPosInCameraSpace = VREmulator.cameraPlayer.transform.TransformVector(fakePos);

        
// Set tracked object position relative to camera with offsets
        VaroniaInput.Instance.Tracking.position = 
            VREmulator.cameraPlayer.transform.position + 
            (VREmulator.cameraPlayer.transform.forward * 0.5f) + 
            fakeGunPosInCameraSpace - 
            pivotWorldOffset;



    }
   
   #endif
}
