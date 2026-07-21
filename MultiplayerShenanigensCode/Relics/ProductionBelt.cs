using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Runs;
using MultiplayerShenanigens.MultiplayerShenanigensCode.Powers;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Relics;

[Pool(typeof(EventRelicPool))]
public class ProductionBelt : MultiplayerShenanigensRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count > 1;
    }

    public override async Task AfterCardGeneratedForCombat(CardModel card, bool addedByPlayer)
    {
        if (card.IsClone || card.IsDupe)
            return;

        if (!addedByPlayer)
            return;

        if (card.Type != CardType.Status)
            return;

        Player? teammate = Owner.Creature.CombatState?.Players.Where(p => p != Owner).TakeRandom(1, MegaCrit.Sts2.Core.Random.Rng.Chaotic).First();

        if (teammate == null)
        {
            MainFile.Logger.Warn($"[ProductionBelt] No teammate found to Move status card {card.Title} to.");
            return;
        }

        CardModel copy = card.CreateClone();
        copy._owner = teammate;

        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(copy, PileType.Discard));

        await CardPileCmd.RemoveFromCombat(card);
    }
}
