

namespace Data.DataModels;

public class Event{

    // Properties of our event class


    Guid EventId {get; set;}
    string EventDescription {get; set;}
    List<EventTag> Tags {get; set;}
    List<EventOptions> Options {get; set;}
    
}