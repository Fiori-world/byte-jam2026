
namespace Data.DataModels;

public class EventOption{

    // Properties of our eventoptions

    // This is a foreign key to our Events table
    Guid EventId {get; set;}
    int OptionId {get; set;}
    string OptionDescription {get; set;}
    List<OptionTag> Tags {get; set;}
    
}