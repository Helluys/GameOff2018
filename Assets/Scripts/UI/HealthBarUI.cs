public class HealthBarUI : BarUI {

    public override float value {
        get {
            return GameManager.instance.GetPlayer().instanceStatistics.health;
        }
    }

    public override float maxValue {
        get {
            return GameManager.instance.GetPlayer().sharedStatistics.maxHealth;
        }
    }
}
