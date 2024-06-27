using System.Reflection.Metadata.Ecma335;

namespace libs;

using Newtonsoft.Json;

public static class FileHandler
{
    private static string filePath;
    private readonly static string envVar = "GAME_SETUP_PATH";


    // added second JSON file for saved game
    private static string savedFilePath;
    private readonly static string envVarSavedGame = "GAME_SETUP_PATH_SAVED";


    private static string dialogFilePath;
    private readonly static string envVarDialog = "GAME_DIALOG_SETUP_PATH";


    private static string tutorialFilePath;
    private readonly static string envVarTutorial = "GAME_TUTORIAL_SETUP_PATH";

    static FileHandler()
    {
        Initialize();
    }

    private static void Initialize()
    {
        if(Environment.GetEnvironmentVariable(envVar) != null){
            filePath = Environment.GetEnvironmentVariable(envVar);
        };
        if(Environment.GetEnvironmentVariable(envVarSavedGame) != null){
            savedFilePath = Environment.GetEnvironmentVariable(envVarSavedGame);
        };
        if(Environment.GetEnvironmentVariable(envVarDialog) != null){
            dialogFilePath = Environment.GetEnvironmentVariable(envVarDialog);
        };
        if(Environment.GetEnvironmentVariable(envVarTutorial) != null){
            tutorialFilePath = Environment.GetEnvironmentVariable(envVarTutorial);
        };
    }

    public static dynamic ReadJson()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(filePath);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {filePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    // read saved saved game json file
    public static dynamic ReadSavedJson()
    {
        if (string.IsNullOrEmpty(savedFilePath))
        {
            throw new InvalidOperationException("Saved JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(savedFilePath);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {savedFilePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    public static dynamic ReadDialogJson()
    {
        if (string.IsNullOrEmpty(dialogFilePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(dialogFilePath);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {dialogFilePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }

    public static dynamic ReadTutorialJson()
    {
        if (string.IsNullOrEmpty(tutorialFilePath))
        {
            throw new InvalidOperationException("JSON file path not provided in environment variable");
        }

        try
        {
            string jsonContent = File.ReadAllText(tutorialFilePath);
            dynamic jsonData = JsonConvert.DeserializeObject(jsonContent);
            return jsonData;
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException($"JSON file not found at path: {tutorialFilePath}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error reading JSON file: {ex.Message}");
        }
    }
}
