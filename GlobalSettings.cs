//File with settings like text file location, api keys, model names
namespace myProject
{
    public static class GlobalSettings
    {
        public static string GeminiApiKey => Environment.GetEnvironmentVariable("GEMINI_API_KEY")
        ?? throw new InvalidOperationException(
        "Environment variable GEMINI_API KEY is not set." 
        );
       
        public static string TextFilePath =>
            Environment.GetEnvironmentVariable("TEXT_FILE_PATH")
            ?? throw new InvalidOperationException(
                "Environment variable TEXT_FILE_PATH is not set."
            );

        public static string VectorFilePath => 
            Environment.GetEnvironmentVariable("VECTOR_FILE_PATH")
            ?? throw new InvalidOperationException(
                "Environment variable VECTOR_FILE_PATH is not set."
            );

    }      
    
}
