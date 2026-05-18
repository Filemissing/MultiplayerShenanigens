using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Cards;

[Pool(typeof(ColorlessCardPool))]
public class Yoink() : MultiplayerShenanigensCard(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(cardPlay.Target.Player, "cardPlay.Target.Player");

        CardSelectorPrefs prefs = new CardSelectorPrefs(SelectionScreenPrompt, 0, 1);
        var cards = await CardSelectCmd.FromHand(choiceContext, cardPlay.Target.Player, prefs, null, this);

        CardModel? cardToYoink = cards.FirstOrDefault();

        ArgumentNullException.ThrowIfNull(cardToYoink, "cardToYoink");

        await CardCmd.Exhaust(choiceContext, cardToYoink);

        CardModel newCopy = CardFactory.GetDistinctForCombat(Owner, new[] { cardToYoink }, 1, rng: MegaCrit.Sts2.Core.Random.Rng.Chaotic).FirstOrDefault()!;

        if (newCopy != null)
            await CardPileCmd.AddGeneratedCardToCombat(newCopy, PileType.Hand, addedByPlayer: true);
    }
}
