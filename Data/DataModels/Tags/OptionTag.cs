
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

    public enum ResourceGain
    {
        
        FOOD, // (OptionTag.ResourceGain)0;
        WATER, // (OptionTag.ResourceGain)1;

        ITEM, // (OptionTag.ResourceGain)2;

        HEALTHCONDITION, // ((OptionTag.ResourceGain)3;

        WORLDEVENT, // (OptionTag.ResourceGain)4;

        SPECIAL // (OptionTag.ResourceGain)5;

    }
    public enum ResourceCost
    {
        
        FOOD, // (OptionTag.ResourceCost)0;
        WATER, // (OptionTag.ResourceCost)1;

        ITEM, // (OptionTag.ResourceCost)2;

        HEALTHCONDITION, // (OptionTag.ResourceCost)3;
        WORLDEVENT, // (OptionTag.ResourceCost)4;

        SPECIAL // (OptionTag.ResourceCost)5;

    }

}