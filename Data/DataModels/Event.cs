using Data.DataModels.Tags;
namespace Data.DataModels;

public class Event{

    // Properties of our event class


    public Guid EventId {get; set;}
    public string EventDescription {get; set;} = "";
    public List<EventTag> Tags {get; set;} = [];
    public List<int> Options {get; set;} = [];
   
    
}