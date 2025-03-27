using HtmlAgilityPack;

namespace Rider_ConsoleApp;

// ReSharper disable UnusedAutoPropertyAccessor.Global
public static class FilteredV1
{
    public static List<Dictionary<string, string>> Start(string filePath, char separator)
    {
        //string filePath = "data/filtered.csv";
        var parsedData = ParseCsv(filePath, separator);


        return parsedData;
    }

    static List<Dictionary<string, string>> ParseCsv(string filePath, char separator)
    {
        var data = new List<Dictionary<string, string>>();
        var lines = File.ReadAllLines(filePath);
        //var headers = lines[0].Split('\t').Skip(1).ToArray(); // Skip the first column (Field names)
        //var headers = lines[0].Split(separator).Skip(1).ToArray(); // Skip the first column (Field names)

        for (int i = 0; i < lines.Length; i++)
        {
            var values = lines[i].Split(separator);
            var fieldName = values[0]; // The field name (e.g., "Brand", "Model")
            if (string.IsNullOrEmpty(fieldName))
                continue;
            for (int j = 1; j < values.Length; j++)
            {
                if (data.Count < j) data.Add(new Dictionary<string, string>());
                data[j - 1][fieldName] = values[j];
            }
        }

        return data.Where(item => item.ContainsKey("Model") && string.IsNullOrEmpty(item["Model"]) == false).ToList();
    }

    public static async Task DownloadImageAsync(string url, string filePath)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            httpClient.DefaultRequestHeaders.Referrer = new Uri(url);
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fileStream);
                    }

                    Console.WriteLine($"Image saved: {filePath}");
                }
                else
                {
                    Console.WriteLine($"Failed to download image from {url}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading image from {url}: {ex.Message}");
            }
        }
    }

    public static async Task DownloadAllImagesFromWebpageAsync(string webpageUrl, string downloadDirectory)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            httpClient.DefaultRequestHeaders.Referrer = new Uri(webpageUrl); // Set the Referer

            try
            {
                var response = await httpClient.GetAsync(webpageUrl);
                response.EnsureSuccessStatusCode(); // Ensure we got a successful response (will throw if not 2xx)

                var htmlContent = await response.Content.ReadAsStringAsync();

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                // Select all <img> tags
                var imageNodes = htmlDocument.DocumentNode.SelectNodes("//img[@src]");

                if (imageNodes != null)
                {
                    Console.WriteLine($"Found {imageNodes.Count} images on {webpageUrl}");

                    // Create the download directory if it doesn't exist
                    Directory.CreateDirectory(downloadDirectory);

                    foreach (var imgNode in imageNodes)
                    {
                        var imageUrl = imgNode.GetAttributeValue("src", null);

                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            // Make the URL absolute if it's relative
                            Uri absoluteUrl;
                            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
                            {
                                absoluteUrl = new Uri(new Uri(webpageUrl), imageUrl);
                            }
                            else
                            {
                                absoluteUrl = new Uri(imageUrl);
                            }

                            // Extract the image file name from the URL
                            var fileName = Path.GetFileName(absoluteUrl.LocalPath);
                            var filePath = Path.Combine(downloadDirectory, fileName);

                            Console.WriteLine($"Downloading image from: {absoluteUrl}");

                            // Add User-Agent and Referer for the image request as well (important!)
                            using (var imageHttpClient = new HttpClient())
                            {
                                imageHttpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                                imageHttpClient.DefaultRequestHeaders.Referrer = new Uri(webpageUrl);
                                await DownloadImageAsync(absoluteUrl.ToString(), filePath);
                            }

                            await Task.Delay(TimeSpan.FromSeconds(1)); // Be polite
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"No images found on {webpageUrl}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error accessing webpage {webpageUrl}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}