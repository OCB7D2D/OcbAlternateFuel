// ####################################################################
// Class for `WorkstationFuelGrid` to set specific requirements
// for the type of fuel a workstation will accept.
// ####################################################################

public class XUiC_OcbWorkstationFuelGrid : XUiC_WorkstationFuelGrid
{

    // ####################################################################
    // ####################################################################

    public FastTags<TagGroup.Global> fuelTags = FastTags<TagGroup.Global>.Parse("biofuel");

    // ####################################################################
    // Parse tag requirements from parent controller's CustomAttributes
    // As passed by the Harmony patch from outer window group reference
    // ####################################################################

    public override void Init()
    {
        XUiController parent = this;
        while (parent != null)
        {
            if (parent.CustomAttributes.TryGetValue(
                "fuel_tags", out string tags))
            {
                fuelTags = FastTags<TagGroup.Global>.Parse(tags);
            }
            parent = parent.Parent;
        }
        base.Init();
    }

    // ####################################################################
    // Parse tag requirements as given in the `WindowFuel`
    // This is an alternative way to overwrite the `fuel_tags`
    // Note: currently not used in any of our demo code at all!
    // ####################################################################

    public override bool ParseAttribute(string name, string value, XUiController _parent)
    {
        if (name == "fuel_tags") fuelTags = FastTags<TagGroup.Global>.Parse(value);
        return base.ParseAttribute(name, value, _parent);
    }

    // ####################################################################
    // Check if `klass` matches all tag requirements
    // ####################################################################

    public bool IsValidFuel(ItemClass klass)
    {
        return klass.HasAllTags(fuelTags);
    }

    // ####################################################################
    // Check item class requirements on `AddItem`
    // ####################################################################

    public override bool AddItem(ItemClass klass, ItemStack stack)
    {
        if (IsValidFuel(klass))
        {
            return base.AddItem(klass, stack);
        }
        return false;
    }

    // ####################################################################
    // Called via HarmonyPatch from children ItemStack CanSwap
    // ####################################################################

    public bool CanSawp(ItemStack stack)
    {
        return IsValidFuel(stack.itemValue.ItemClass);
    }

    // ####################################################################
    // ####################################################################

}
