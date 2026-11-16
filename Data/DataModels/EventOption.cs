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
    public int[] Value {get; set;} = [];

    public void ApplyTo(GameState state)
    {
        var tags = Tags.OrderBy(t => t.TagId).ToList();

        for (int i = 0; i < tags.Count; i++)
        {
            int? gain = Value.Length > i * 2 ? Value[i * 2] : null;
            int ? cost = Value.Length > i* 2 + 1 ? Value[i * 2 + 1] : null;
        }
    }

    
}