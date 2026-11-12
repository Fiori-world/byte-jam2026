
using System.ComponentModel.DataAnnotations;

namespace Data.DataModels.Tags;
public abstract class Tag
{

    // Class properties...
    public Probability tagProbability {get; set;}
    public Quality tagQuality {get; set;}
    [Key]
    public int TagId { get; set; }

    protected Tag()
    {
        
    }

// Constructor for INHERTIANCE ONLY! Do not try to initialize a Tag Object
    public Tag(string probabilityString, string qualityString)
    {
        // WARNING: NO VALIDATION. PARAMS MUST BE PERFECT
        tagProbability = Enum.Parse<Probability>(probabilityString, ignoreCase: true);
        tagQuality = Enum.Parse<Quality>(qualityString, ignoreCase: true);

    }
    public enum Probability
    {
        COMMON, // (Tag.Probability)0;
        UNCOMMON, // (Tag.Probability)1;
        RARE // (Tag.Probability)2;
    }     

     public enum Quality
    {
        HORRIBLE, // (Tag.Quality)0;
        BAD, // (Tag.Quality)1;
        NEUTRAL, // (Tag.Quality)2;
        GOOD, // (Tag.Quality)3;
        INCREDIBLE // (Tag.Quality)4;
    };

    

}