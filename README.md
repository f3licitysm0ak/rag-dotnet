This is a .NET console application that models a simple RAG pipeline. 
Users will ask a question, and receive an LLM-generated response grounded in relevant context from a specific text document. 

BEFORE RUNNING


Set environment variables:


GEMINI_API_KEY - API key for Gemini model, needed for embeddings and text generation


TEXT_FILE_PATH - path to source text document. There is an included small sample file cats.txt in this repo. 


VECTOR_FILE_PATH - path to desired location of vector file store

