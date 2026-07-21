using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MultiplayerShenanigens.MultiplayerShenanigensCode.Relics;

namespace MultiplayerShenanigens.MultiplayerShenanigensCode.Ancients;

public class Hydra : CustomAncientModel
{
    public override string? CustomMapIconPath => "res://MultiplayerShenanigens/images/ancients/Hydra-map.png";
    public override string? CustomMapIconOutlinePath => "res://MultiplayerShenanigens/images/ancients/Hydra-map-outline.png";
    public override string? CustomRunHistoryIconPath => "res://MultiplayerShenanigens/images/ancients/Hydra-history.png";
    public override string? CustomRunHistoryIconOutlinePath => "res://MultiplayerShenanigens/images/ancients/Hydra-history-outline.png";
    public override string? CustomScenePath => "res://MultiplayerShenanigens/scenes/hydra.tscn";

    protected override OptionPools MakeOptionPools => new OptionPools(
        [
            AncientOption<MassiveScroll>(), // just filler
            AncientOption<LoomingFruit>(), // just filler
            AncientOption<Hydrahead>(), // get class specific relic
        ]
    );

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3 && Owner?.RunState.Players.Count > 1;
    }

    public override bool ShouldForceSpawn(ActModel act, AncientEventModel? rngChosenAncient)
    {
        if (act.ActNumber() == 3 && Owner?.RunState.Players.Count > 1)
            return true;
        return false;
    }
}
