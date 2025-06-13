using UnityEngine;

public class SimulatorSettings : MonoBehaviour,IAddonConfigurable
{
  public static VrsimulatorSettings settings;
   
   public void ApplyScriptableConfig(ScriptableObject config)
   {
      settings = config as VrsimulatorSettings;
   }
   
}
