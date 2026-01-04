using HarmonyLib;
using System.Reflection;
using System.Xml.Linq;

public class OcbAlternateFuel : IModApi
{

    // ####################################################################
    // ####################################################################

    public void InitMod(Mod mod)
    {
        if (GameManager.IsDedicatedServer) return;
        Log.Out("OCB Harmony Patch: " + GetType().ToString());
        Harmony harmony = new Harmony(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    // ####################################################################
    // Call parent controller's `CanSwap` to check slot requirements
    // ####################################################################

    [HarmonyPatch(typeof(XUiC_ItemStack), "CanSwap")]
    public class XUiC_ItemStack_CanSwap_Patch
    {
        static void Postfix(XUiC_ItemStack __instance, ref bool __result, ItemStack _stack)
        {
            if (__result && __instance.Parent is XUiC_OcbWorkstationFuelGrid grid)
            {
                __result = grid.CanSawp(_stack);
            }
        }
    }

    // ####################################################################
    // Pass attributes from outer window to inner window's controller
    // This way you can configure same window with different settings
    // ####################################################################

    [HarmonyPatch(typeof(XUiFromXml), "parseWindow")]
    public class XUiFromXml_parseWindow_Patch
    {
        static void Postfix(string _name,
            XElement _windowCallingElement,
            XElement _windowContentElement,
            XUiWindowGroup _windowGroup,
            ref XUiV_Window __result)
        {
            // Overwrite attributes from calling element to result window
            // This technically also allows to overwrite the controller!
            foreach (XAttribute attr in _windowCallingElement.Attributes())
            {
                __result.Controller.CustomAttributes[attr.Name.ToString()] = attr.Value;
            }
        }
    }

    // ####################################################################
    // ####################################################################

    [HarmonyPatch(typeof(Vehicle), "GetFuelItem")]
    public class Vehicle_GetFuelItem_Patch
    {
        static bool Prefix(Vehicle __instance, ref string __result)
        {
            __result = string.Empty;
            if (__instance.HasEnginePart())
            {
                if (__instance.itemValue.ItemClass.Properties.Values
                    .TryGetValue("fuelItem", out string fuelItem))
                {
                    __result = fuelItem;
                }
            }
            return false;
        }
    }

    // ####################################################################
    // ####################################################################

}

