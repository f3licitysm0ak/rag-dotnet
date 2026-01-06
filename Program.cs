using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using Google.GenAI;
using Google.GenAI.Types;
using myProject;

//define functions
double cosine_similarity(float[] a, float[] b)
{
    float dotProduct = 0f;
    for (int i = 0; i < a.Length; i++)
    {
        dotProduct += a[i] * b[i];
    }

    float normA = 0f;
    float normB = 0f;
    foreach(float num in a)
    {
        normA += num * num;
    }
    foreach(float num in b)
    {
        normB += num * num;
    }
    normA = MathF.Sqrt(normA);
    normB = MathF.Sqrt(normB);

    return dotProduct / (normA * normB);

}


//load in dataset 
var apiKey = GlobalSettings.GeminiApiKey;
var llmService = new LLMService(apiKey);
string textFilePath = GlobalSettings.TextFilePath;
string vectorFilePath = GlobalSettings.VectorFilePath;


FileVectorStore vectorStore = new FileVectorStore(vectorFilePath); //making FileVectorStore object with specified filePath

Dictionary <string, float[]> vector_db = vectorStore.load();

//checking if embeddings are being made for the first time
if (vector_db.Count == 0)
{
    foreach (string line in System.IO.File.ReadLines(textFilePath, System.Text.Encoding.UTF8))
    {   
    //make an embedding and add to dictionary
    var embedding = await llmService.CreateEmbeddingAsync(line);
    vector_db.Add(line, embedding);
    }

    vectorStore.save(vector_db);   
} 



//user input flow
Console.Write("Enter a question about cats: ");
string query = Console.ReadLine();
var queryvec =await llmService.CreateEmbeddingAsync(query);


//compute all similarities
Dictionary <string, double> similarity_db = new Dictionary<string, double>();
foreach(var element in vector_db)
{
    double computed_similarity = cosine_similarity(queryvec, element.Value); //find cosine similarity
    similarity_db.Add(element.Key, computed_similarity);
}

//sort to get highest similarity first 
var sorted = similarity_db.OrderByDescending(elem => elem.Value);

var topSimilarItems = sorted.Take(5).Select(elem => elem.Key).ToArray(); //taking only keys because those are the actual data chunks (not embeddings)

//generate LLM response grounded in topSimilarItems
var client = new Client(apiKey: GlobalSettings.GeminiApiKey);
string contextData = string.Join("\n", topSimilarItems);
//query we already defined earlier 
string prompt = "Please use only the following context to answer the question:\n" + contextData + "\n" + query;

var response = await client.Models.GenerateContentAsync(
    model: "gemini-2.5-flash", contents: prompt
);
Console.WriteLine(response.Candidates[0].Content.Parts[0].Text);



