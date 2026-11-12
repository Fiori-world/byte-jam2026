
namespace Data.DataModels.Tags;
public class OptionTag : Tag
{
    // Class properties
    public ResourceGain tagResourceGain {get; set;}
    public ResourceCost tagResourceCost {get; set;}
    // Review Tag Constructor for syntax
    public OptionTag(string probabilityString, string qualityString, string resourceGainString, string resourceCostString) : base(probabilityString, qualityString)
    {
        tagResourceGain = Enum.Parse<ResourceGain>(resourceGainString, ignoreCase: true);
   
        tagResourceCost = Enum.Parse<ResourceCost>(resourceCostString, ignoreCase: true);
    }

    public OptionTag() : base()
    {
        
    }

    public void ApplyTo(GameState state)
    {
        int gain = (int)tagQuality +1;
        int cost = 5 - (int)tagQuality;
        if (state.Morale <12) gain = Math.Max(0, gain -1);
        
        state.ChangeResource(tagResourceGain.ToString(), +gain);
        state.ChangeResource(tagResourceCost.ToString(), -cost);

        state.ChangeMorale(tagQuality switch
        {
            Quality.HORRIBLE   => -8,
            Quality.BAD        => -3,
            Quality.GOOD       => +3,
            Quality.INCREDIBLE => +8,
            _                  => 0
        });
    }

    public enum ResourceGain
    {
        
        FOOD, // (OptionTag.ResourceGain)0;
        WATER, // (OptionTag.ResourceGain)1;

        ITEM, // (OptionTag.ResourceGain)2;

        HEALTHCONDITION, // ((OptionTag.ResourceGain)3;

        MORALE, // (OptionTag.ResourceGain)4;

        WORLDEVENT, // (OptionTag.ResourceGain)5;

        SPECIAL // (OptionTag.ResourceGain)6;

    }
    public enum ResourceCost
    {
        
        FOOD, // (OptionTag.ResourceCost)0;
        WATER, // (OptionTag.ResourceCost)1;

        ITEM, // (OptionTag.ResourceCost)2;

        HEALTHCONDITION, // (OptionTag.ResourceCost)3;

        MORALE, // (OptionTag.ResourceCost)4;

        WORLDEVENT, // (OptionTag.ResourceCost)5;

        SPECIAL // (OptionTag.ResourceCost)6;

    }

    

}

