using System;
using NaughtyAttributes;
using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using VaroniaBackOffice;
using Slider = UnityEngine.UI.Slider;

public enum ItemControl { None,LeftHand, RightHand, Trk }


public class VREmulator : MonoBehaviour
{

    public static bool IsActive;
    

    [HideInInspector]
    public Camera cameraPlayer;

    public GameObject uI;

    private VRSwitcher _vrSwitcher;
    
    private MouseLookHybrid _mouseLook;
    
    private VRFPSController _fpsController;
 
    public Slider slider;

    public  TextMeshProUGUI heightValue;
    
    public ItemControl itemControl = ItemControl.None;
    
    
    private  VrInput _vrInput;
    
   
   
    
    
  public  void TakeControl(int _itemControl)
    {
        itemControl = (ItemControl)_itemControl;
    }
    
  
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(CheckMainCamera());
        
#if HAS_XR_MANAGEMENT
        _vrSwitcher = FindFirstObjectByType<VRSwitcher>();
        _vrSwitcher.onVRSwitch.AddListener(SwitchUI);
        #endif
        _mouseLook = GetComponent<MouseLookHybrid>();
        _fpsController = GetComponent<VRFPSController>();
      
        uI.SetActive(false);

        slider.value = 1.8f;
        
        SearchItemCtrl();

    }


    void SearchItemCtrl()
    {
        #if VBO_Input
        _vrInput = GetComponent<VrInput>();
        #endif
    }
    
    
    private void Update()
    {
        if(uI.activeSelf)
        DebugVaronia.Instance.AdvDebugMove = false;
        
        if ( KeyboardHook.GetKeyDown(KeyCode.Tab) && Application.isFocused)
           SwitchUI();
        
       
if (KeyboardHook.GetKeyDown(KeyCode.Escape) && uI.activeSelf && Application.isFocused)
    _mouseLook.enabled = !_mouseLook.enabled;
        
        heightValue.text = "Set Height ("+slider.value.ToString("F2")+")";
        
        MoveItem();

    }



    void MoveItem()
    {
        VrItem temp_ItemControl = null;
        
        switch (itemControl)
        {
            case ItemControl.LeftHand:
            //    temp_ItemControl = left_hand_transform;
                break;
            case ItemControl.RightHand:
               // temp_ItemControl = right_hand_transform;
                break;
            case ItemControl.Trk:
                temp_ItemControl = _vrInput;
                break;
        }
        
        if(temp_ItemControl == null) return;
        
        
#if ENABLE_INPUT_SYSTEM
        // Nouveau système d'input
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            
            if(keyboard.numpadEnterKey.isPressed)
                temp_ItemControl.ResetPos();
            
            if (!keyboard.rightAltKey.isPressed)
            {
            if (keyboard.numpad8Key.isPressed)
            {
                temp_ItemControl.Forward();
            }
            
                if (keyboard.numpad2Key.isPressed)
                {
                    temp_ItemControl.Back();
                }

                if (keyboard.numpad4Key.isPressed)
                {
                    temp_ItemControl.Left();
                }

                if (keyboard.numpad6Key.isPressed)
                {
                    temp_ItemControl.Right();
                }

                if (keyboard.numpad9Key.isPressed)
                {
                    temp_ItemControl.Up();
                }

                if (keyboard.numpad3Key.isPressed)
                {
                    temp_ItemControl.Down();
                }
            }
            

            if (keyboard.rightAltKey.isPressed)
            {
                if (keyboard.numpad8Key.isPressed)
                {
                    temp_ItemControl.RotateUp();
                }
                if (keyboard.numpad2Key.isPressed)
                {
                    temp_ItemControl.RotateDown();
                }
                if (keyboard.numpad4Key.isPressed)
                {
                    temp_ItemControl.RotateLeft();
                }
                if (keyboard.numpad6Key.isPressed)
                {
                    temp_ItemControl.RotateRight();
                }
                
                if (keyboard.numpad9Key.isPressed)
                {
                    temp_ItemControl.RotateForward();
                }
                if (keyboard.numpad3Key.isPressed)
                {
                    temp_ItemControl.RotateBackward();
                }
                
            }
        }
#else
        // Old input system

  if (Input.GetKey(KeyCode.KeypadEnter))
        {
                temp_ItemControl.ResetPos();
        }


   if (!Input.GetKey(KeyCode.RightAlt))
        {

        if (Input.GetKey(KeyCode.Keypad8))
        {
            temp_ItemControl.Forward();
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            temp_ItemControl.Back();
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            temp_ItemControl.Left();
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            temp_ItemControl.Right();
        }
        if (Input.GetKey(KeyCode.Keypad9))
        {
            temp_ItemControl.Up();
        }
        if (Input.GetKey(KeyCode.Keypad3))
        {
            temp_ItemControl.Down();
        }
    }
        if (Input.GetKey(KeyCode.RightAlt))
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                temp_ItemControl.RotateUp();
            }
            if (Input.GetKey(KeyCode.Keypad2))
            {
                temp_ItemControl.RotateDown();
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                temp_ItemControl.RotateLeft();
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                temp_ItemControl.RotateRight();
            }
        }
#endif

    }
    
    
    void SwitchUI()
    {
      
#if HAS_XR_MANAGEMENT
        if(VRSwitcher.vrEnabled)
        uI.SetActive(false);
        else uI.SetActive(!uI.activeSelf);
        #else
        uI.SetActive(!uI.activeSelf);
#endif

        ActiveKeyboardMouse(uI.activeSelf);
        if (uI.activeSelf)
        {
            SetHeight(1.8f);
            Application.targetFrameRate = 90;
            IsActive = true;
        }
        else
        {
            IsActive = false;;
            Application.targetFrameRate = -1;

        }
        
        _mouseLook.enabled = uI.activeSelf;
        VaroniaGlobal.HideDebugCanvas = uI.activeSelf;
        
        
        
    }


    private void ActiveKeyboardMouse(bool active)
    {
        _mouseLook.enabled = active;  
        _fpsController.enabled = active;
    }
    
    
    public void SetHeight(float height)
    {
        cameraPlayer.transform.position = new Vector3(cameraPlayer.transform.position.x, height, cameraPlayer.transform.position.z);
    }

    
    
    private IEnumerator CheckMainCamera()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (cameraPlayer == null)
                cameraPlayer = Camera.main;

        }
    }
}
 
