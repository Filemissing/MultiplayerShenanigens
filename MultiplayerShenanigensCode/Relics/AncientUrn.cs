using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Relics;

[Pool(typeof(EventRelicPool))]
public class AncientUrn : MultiplayerShenanigensRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;

    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count > 1;
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (causedByEthereal)
            return;

        Player? teammate = Owner.Creature.CombatState?.Players.Where(p => p != Owner).TakeRandom(1, MegaCrit.Sts2.Core.Random.Rng.Chaotic).First();

        if (teammate == null)
        {
            MainFile.Logger.Warn($"[AncientUrn] No teammate found to Move exhausted card {card.Title} to.");
            return;
        }

        CardModel copy = card.CreateClone();
        copy._owner = teammate;
        if (copy != null)
            await CardPileCmd.Add(copy, PileType.Hand);
    }
}
