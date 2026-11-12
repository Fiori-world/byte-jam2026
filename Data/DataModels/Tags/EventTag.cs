namespace Data.DataModels.Tags;
public class EventTag : Tag
{
    // Class properties
    public ResourceAffected tagResourceAffected {get; set;}
    // Review Tag Constructor for syntax
    public EventTag(string probabilityString, string qualityString, string resourceAffectedString) : base(probabilityString, qualityString)
    {
        tagResourceAffected = Enum.Parse<ResourceAffected>(resourceAffectedString, ignoreCase: true);
    }

    public EventTag() : base()
    {
        
    }
    public enum ResourceAffected
    {
        
        FOOD, // (EventTag.ResourceAffected)0;
        WATER, // (EventTag.ResourceAffected)1;

        ITEM, // (EventTag.ResourceAffected)2;

        HEALTHCONDITION, // (EventTag.ResourceAffected)3;

        MORALE, // (EventTag.ResourceAffected)4;

        WORLDEVENT, // (EventTag.ResourceAffected)5;

        SPECIAL // (EventTag.ResourceAffected)6;

    }

}