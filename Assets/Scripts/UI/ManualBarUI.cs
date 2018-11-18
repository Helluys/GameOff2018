public class ManualBarUI : BarUI {

    public override float value { get { return manualValue; } }

    public override float maxValue { get { return manualMaxValue; } }

    public float manualMaxValue;
    public float manualValue;

}
