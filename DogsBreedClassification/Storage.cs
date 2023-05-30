namespace DogsBreedClassification;

public class Storage
{
    /*
    #region Singleton
    public static Storage? Instance;

    
    private Storage(){}
    
    public static Storage? getInstance()
    {
        if (Instance == null)
            Instance = new Storage();
        return Instance;
    }
    #endregion
    */

   

    public static bool isTrainLoaded = false;
    public static bool isTestLoaded = false;

    public static bool isModelLoadedOrLearned = false;

}