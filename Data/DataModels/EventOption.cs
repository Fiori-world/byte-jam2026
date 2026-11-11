using Data.DataModels.Tags;
namespace Data.DataModels;

public class EventOption{

    // Properties of our eventoptions

    // This is a foreign key to our Events table
    public Guid EventId {get; set;}
    public int OptionId {get; set;}
    public string OptionDescription {get; set;} = "";
    public List<OptionTag> Tags {get; set;} = [];
    
}