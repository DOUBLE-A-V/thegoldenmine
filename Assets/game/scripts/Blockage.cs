using UnityEngine;

public class Blockage : Breakable
{
    public override void OnDamage()
    {
        M.game.UpdateMainGoalText();
    }
    public override void OnDeath()
    {
        M.game.mainGoalText.text = "blockage\nremoved!";
        StartCoroutine(M.game.Win());
    }
}
