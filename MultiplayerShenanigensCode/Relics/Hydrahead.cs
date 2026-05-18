using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Relics;


[Pool(typeof(EventRelicPool))]
public class Hydrahead : MultiplayerShenanigensRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
    public override bool IsAllowed(IRunState runState)
    {
        return runState.Players.Count > 1;
    }

    public override async Task AfterObtained()
    {
        switch (Owner.Character)
        {
            case Ironclad:
                await RelicCmd.Obtain<AncientUrn>(Owner);
                break;
            case Silent:
                await RelicCmd.Obtain<GiftThatKeepsOnGiving>(Owner);
                break;
            case Regent:
                await RelicCmd.Obtain<MinionFactory>(Owner);
                break;
            case Necrobinder:
                await RelicCmd.Obtain<BoneShield>(Owner);
                break;
            case Defect:
                await RelicCmd.Obtain<ProductionBelt>(Owner);
                break;

            default:
                MainFile.Logger.Warn($"Unknown character class {Owner.Character.GetType().Name} for Hydrahead relic. No additional relic will be granted.");
                throw new InvalidOperationException("Unknown character class for Hydrahead relic.");
        }
    }
}
