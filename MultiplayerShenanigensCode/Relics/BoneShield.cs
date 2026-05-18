using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MultiplayerShenanigens.MultiplayerShenanigensCode.Powers;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Relics;

[Pool(typeof(EventRelicPool))]
public class BoneShield : MultiplayerShenanigensRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count > 1;
    }

    public override async Task AfterSummon(PlayerChoiceContext choiceContext, Player summoner, decimal amount)
    {
        Creature? osty = summoner.Osty;

        if(osty == null)
        {
            MainFile.Logger.Warn($"[BoneShield] No Osty found for player to apply DeflectPower to.");
            return;
        }

        if (!osty.HasPower<DeflectPower>())
            await PowerCmd.Apply<DeflectPower>(osty, 1m, Owner.Creature, null);
    }
}