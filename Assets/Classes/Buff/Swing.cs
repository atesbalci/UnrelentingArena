
public class Swing : IndefinateBuff {

    public Swing(Player player)
        : base(player) {
    }

    public override void Update() {
        base.Update();
        if (!active)
            return;
        player.canCast = false;
    }
}