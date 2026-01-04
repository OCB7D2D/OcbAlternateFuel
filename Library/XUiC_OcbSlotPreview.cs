// ####################################################################
// Class to provide placeholder for {fuelResourceSprite}
// Which is read from parent controller's CustomAttributes
// Which are in turn passed by `XUiFromXml.parseWindow` patch
// ####################################################################

public class XUiC_OcbSlotPreview : XUiC_SlotPreview
{

    // ####################################################################
    // ####################################################################

    string fuelSprite = "resourceWood";

    // ####################################################################
    // ####################################################################

    public override void Init()
    {
            XUiController parent = this;
            while (parent != null)
            {
                if (parent.CustomAttributes.TryGetValue(
                    "fuel_sprite", out string sprite))
                {
                    fuelSprite = sprite;
                }
                parent = parent.Parent;
            }
            base.Init();
    }

    // ####################################################################
    // ####################################################################

    public override bool GetBindingValueInternal(ref string _value, string _bindingName)
    {
        switch (_bindingName)
        {
            case "fuelSprite":
                _value = fuelSprite;
                return true;
            default:
                return base.GetBindingValueInternal(ref _value, _bindingName);
        }
    }

    // ####################################################################
    // ####################################################################

}
