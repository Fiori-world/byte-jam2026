namespace Data.DataModels.Tags;
public class EventTag : Tag
{
    // Class properties
    ResourceAffected tagResourceAffected;
    // Review Tag Constructor for syntax
    public EventTag(string probabilityString, string qualityString, string resourceAffectedString) : base(probabilityString, qualityString)
    {
        tagResourceAffected = (EventTag.ResourceAffected)(resourceAffectedString.ToUpper);
    }

    public enum ResourceAffected
    {
        
        FOOD, // (EventTag.ResourceAffected)0;
        WATER, // (EventTag.ResourceAffected)1;

        ITEM, // (EventTag.ResourceAffected)2;

        HEALTHCONDITION, // (EventTag.ResourceAffected)3;

        WORLDEVENT, // (EventTag.ResourceAffected)4;

        SPECIAL // (EventTag.ResourceAffected)5;

    }

}