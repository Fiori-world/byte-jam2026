namespace Data.DataModels.Tags;
public class OptionTag : Tag
{
    // Review Tag Constructor for syntax
    public OptionTag(string probabilityString, string qualityString, string resourceGainString, string resourceCostString) : base(probabilityString, qualityString)
    {
        ResourceGain = (OptionTag.ResourceGain)(resourceGainString.ToUpper);
        ResourceCost = (OptionTag.ResourceCost)(resourceCostString.ToUpper);
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