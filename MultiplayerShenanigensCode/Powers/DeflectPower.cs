using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using static Godot.HttpRequest;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Powers;

public class DeflectPower : MultiplayerShenanigensPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
            return Task.CompletedTask;

        // damage was already halved in ModifyDamageMultiplicative so this is the correct amount to divide over teammates
        Creature? playerCreature = Owner.PetOwner?.Creature;

        if (playerCreature == null)
            return Task.CompletedTask;

        List<Player> players = CombatState.Players.Where(p => p != Owner.PetOwner).ToList();

        decimal dividedAmount = result.TotalDamage / players.Count;

        foreach (Player teammate in players)
            CreatureCmd.Damage(choiceContext, teammate.Creature, dividedAmount, ValueProp.Unpowered, Owner);

        return Task.CompletedTask; // 6
    }

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target == Owner)
            return amount / 2m; // first call, when target is osty

        if (target == Owner.PetOwner?.Creature)
            return amount * 2m; // second call, for overflow damage after osty is dead, we want to make sure the player takes the full damage they would have without deflect, not the halved damage

        return amount;
    }
}
