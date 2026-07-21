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

[Pool(typeof(RegentCardPool))]
public class Syphon() : MultiplayerShenanigensCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
        ArgumentNullException.ThrowIfNull(cardPlay.Target.Player, "cardPlay.Target.Player");

        int? syphonedEnergy = cardPlay.Target.Player.PlayerCombatState?.Energy;

        Owner.PlayerCombatState?.GainStars(syphonedEnergy * 2 ?? 0);

        cardPlay.Target.Player.PlayerCombatState?.LoseEnergy(syphonedEnergy ?? 0);

        return Task.CompletedTask;
    }
}
