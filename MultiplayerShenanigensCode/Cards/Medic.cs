using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Cards;

[Pool(typeof(IroncladCardPool))]
public class Medic() : MultiplayerShenanigensCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyPlayer)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>() { new HealVar(4m), new HpLossVar(2) };
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>() { CardKeyword.Exhaust };
       
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

        await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        await CreatureCmd.Heal(cardPlay.Target, DynamicVars.Heal.BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2m);
    }
}
