using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Runs;
using System.Security.Cryptography.X509Certificates;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Relics;

[Pool(typeof(EventRelicPool))]
public class MinionFactory : MultiplayerShenanigensRelic
{
    public override RelicRarity Rarity => RelicRarity.Event;
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count > 1;
    }

    Type[] validCards = new[]
    {
        typeof(Begone),
        typeof(Charge),
        typeof(Guards)
    };

    //public override async Task AfterCardEnteredCombat(CardModel card)
    //{
    //    if (card.Owner != Owner)
    //        return;

    //    if (card.IsClone || card.IsDupe)
    //        return;

    //    if (!card.Tags.Contains(CardTag.Minion))
    //        return;

    //    Player? teammate = Owner.Creature.CombatState?.Players.Where(p => p != Owner).TakeRandom(1, MegaCrit.Sts2.Core.Random.Rng.Chaotic).First();

    //    if (teammate == null)
    //    {
    //        MainFile.Logger.Warn($"[MinionFactory] No teammate found to copy minion card {card.Title} to.");
    //        return;
    //    }

    //    //CardPile hand = PileType.Hand.GetPile(teammate);

    //    //CardModel? selectedCard = hand.Cards.TakeRandom(1, MegaCrit.Sts2.Core.Random.Rng.Chaotic).FirstOrDefault();

    //    //if (selectedCard == null)
    //        //return;

    //    //selectedCard.RemoveFromState();

    //    CardModel dupe = card.CreateDupe();
    //    dupe._owner = teammate;
    //}

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner)
            return;

        if (!validCards.Contains(cardPlay.Card.GetType()))
            return;

        Player? teammate = Owner.Creature.CombatState?.Players.Where(p => p != Owner).TakeRandom(1, MegaCrit.Sts2.Core.Random.Rng.Chaotic).First();

        if (teammate == null)
        {
            MainFile.Logger.Warn($"[MinionFactory] No teammate found to copy minion card {cardPlay.Card.Title} to.");
            return;
        }

        CardModel dupe = cardPlay.Card.CreateDupe();
        dupe._owner = teammate;
        await CardCmd.AutoPlay(context, dupe, null);
    }
}