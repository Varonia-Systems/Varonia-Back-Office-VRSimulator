using UnityEngine;

public class VrsimulatorSettings : ScriptableObject
{
    [Header("Parameter")]
    public Vector3  FakePosInit = new Vector3(0.17f,-0.19f,-0.26f),FakeRotInit;
    public float MouseSensitivity = 45;
    public float MoveSpeed = 2;
    
}

