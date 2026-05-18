using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Relics;

[Pool(typeof(EventRelicPool))]
public class GiftThatKeepsOnGiving : MultiplayerShenanigensRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;

    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count > 1;
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player == Owner)
            return 10;
        else
            return count - 2;
    }
}
