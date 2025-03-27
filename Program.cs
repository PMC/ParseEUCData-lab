// ReSharper disable RedundantUsingDirective
// See https://aka.ms/new-console-template for more information

using System.Collections.Immutable;
using System.Text.Json;
using Rider_ConsoleApp;

//var data = FilteredV1.Start("data/EUC comparison(Main Table) (1).csv", ';');

var data = FilteredV1.Start("data/EUC_1.csv", ';');
var websiteDownloadTasks = new List<Task>();
var allreadyCheckedURLs = new Dictionary<string, bool>();
foreach (var dictionary in data)
{
    foreach (var s in dictionary)
    {
        if (s.Value.Contains("http", StringComparison.CurrentCultureIgnoreCase))
        {
            try
            {
                var model = dictionary["Model"];
                var brand = dictionary["Brand"];
                if (allreadyCheckedURLs.TryAdd(s.Value, true))
                {
                    // Add the task to the list
                    websiteDownloadTasks.Add(
                        FilteredV1.DownloadAllImagesFromWebpageAsync(s.Value, $"data/{brand}-{model}/"));

                    // Check if we have reached the maximum number of concurrent tasks
                    if (websiteDownloadTasks.Count >= 10)
                    {
                        // Wait for any of the tasks to complete
                        Task completedTask = await Task.WhenAny(websiteDownloadTasks);

                        // Remove the completed task from the list
                        websiteDownloadTasks.Remove(completedTask);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}

// make sure all tasks are done before continuing
await Task.WhenAll(websiteDownloadTasks);


// Convert the list to JSON with pretty printing
var options = new JsonSerializerOptions
{
    WriteIndented = true, // This is the key for pretty formatting
};
// Convert the list to JSON
string jsonOutput = JsonSerializer.Serialize(data, options);
File.WriteAllText("data/result.json", jsonOutput);