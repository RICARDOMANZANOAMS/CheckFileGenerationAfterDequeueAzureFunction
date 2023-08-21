# CheckFileGenerationAfterDequeueAzureFunction
This repository contains the azure function in C# to verify if the file dequeued was written in the blob storage. If the file was written correctly, it returns a json with succesfully. Otherwise, error. If the file is not still processed, it appears the message not exist
