
public class Roll : IndefiniteBuff {
    public float speed { get; set; }

    public Roll(Player player)
        : base(player) {
    }

    public override void Update() {
        base.Update();
        if (!active)
            return;
        player.currentSpeed = speed;
        player.canCast = false;
    }
}