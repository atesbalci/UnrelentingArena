
public class Roll : Buff {
    public float speed { get; set; }

    public Roll(Player player)
        : base(player, 0) {
    }

    public override void Update() {
        player.currentSpeed = speed;
        player.canCast = false;
        base.Update();
    }
}