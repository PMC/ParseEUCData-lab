// ReSharper disable RedundantUsingDirective
// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Text.Json;
using Rider_ConsoleApp;

// filter csv data
var data = FilteredV1.Start("data/EUC_1.csv", ';');

// download images
//await FilteredV1.HandleImageDownloads(data);

// Convert the list to JSON with pretty printing
var options = new JsonSerializerOptions
{
    WriteIndented = true, // This is the key for pretty formatting
};
// Convert the list to JSON
string jsonOutput = JsonSerializer.Serialize(data, options);
File.WriteAllText("data/result.json", jsonOutput);