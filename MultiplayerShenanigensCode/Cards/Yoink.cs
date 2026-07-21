using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public class Yoink() : MultiplayerShenanigensCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(cardPlay.Target.Player, "cardPlay.Target.Player");

        // this gives the choice to the targeted player, not the player that played the card
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1);
        var cards = await CardSelectCmd.FromHand(choiceContext, cardPlay.Target.Player, prefs, null, this);

        CardModel? cardToYoink = cards.FirstOrDefault();

        if (cardToYoink == null)
            return; // silent return if no card was selected, hand is probably empty

        CardModel clone = cardToYoink.CreateClone();
        clone._owner = Owner;
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(clone, PileType.Hand));

       await CardPileCmd.RemoveFromCombat(cardToYoink);
    }
}
