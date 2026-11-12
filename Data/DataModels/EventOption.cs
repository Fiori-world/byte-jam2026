using System.ComponentModel.DataAnnotations;
using Data.DataModels.Tags;
namespace Data.DataModels;

public class EventOption{

    // Properties of our eventoptions

    // This is a foreign key to our Events table
    public Guid EventId {get; set;}
    [Key]
    public int OptionId {get; set;}
    public string OptionDescription {get; set;} = "";
    public List<OptionTag> Tags {get; set;} = [];
    public string ResultDescription { get; set; } = "";

    public void ApplyTo(GameState state)
    {
        foreach (var tag in Tags)
            tag.ApplyTo(state);
    }
    
}