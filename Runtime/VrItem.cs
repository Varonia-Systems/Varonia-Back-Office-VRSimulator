using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using VaroniaBackOffice;

public class VrItem : MonoBehaviour
{
    [ReadOnly]
    public Vector3 fakePos;
  [ReadOnly]
    public Vector3 fakeRot;
    protected VREmulator VREmulator;
    
    
    protected Transform TrackedObj;
    
    protected bool IsInit;
    
    
    
   public virtual void  Start()
    {
        VREmulator = GetComponent<VREmulator>();

        ResetPos();
    }


    public virtual void Update()
    {
        if(!IsInit || VREmulator.cameraPlayer==null || VREmulator.uI.activeSelf )
            return;

    }


    public virtual void Up()
    {
        fakePos.y += 0.01f;
    }

    public virtual void Down()
    {
        fakePos.y -= 0.01f;
    }
    
    public virtual void Left()
    {
        fakePos.x -= 0.01f;
    }

    public virtual void Right()
    {
        fakePos.x += 0.01f;
    }
    
    public virtual void Forward()
    {
        fakePos.z += 0.01f;
    }

    public virtual void Back()
    {
        fakePos.z -= 0.01f;
    }
   
    public virtual void RotateUp()
    {
        fakeRot.y += 1f;
    }

    public virtual void RotateDown()
    {
        fakeRot.y -= 1f;
    }

    public virtual void RotateLeft()
    {
        fakeRot.x -= 1f;
    }

    public virtual void RotateRight()
    {
        fakeRot.x += 1f;
    }

    public virtual void RotateForward()
    {
        fakeRot.z += 1f;
    }

    public virtual void RotateBackward()
    {
        fakeRot.z -= 1f;
    }
    
    public virtual void ResetPos()
    {
       fakePos =  SimulatorSettings.settings.FakePosInit;
       fakeRot =  SimulatorSettings.settings.FakeRotInit;
    }
    
    
}
