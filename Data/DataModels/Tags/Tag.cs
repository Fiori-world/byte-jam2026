namespace Data.DataModels.Tags;
public abstract class Tag
{

// Constructor for INHERTIANCE ONLY! Do not try to initialize a Tag Object
    public Tag(string probabilityString, string qualityString)
    {
        // WARNING: NO VALIDATION. PARAMS MUST BE PERFECT
        Probability = (Tag.Probability)(probabilityString.ToUpper);
        Quality = (Tag.Probability)(qualityString.ToUpper);

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