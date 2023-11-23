using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FSM.ViewModel;
using FSM.View;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mail;
using Microsoft.Maui.Devices.Sensors;
using Newtonsoft.Json;
using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Maps.Directions.Response;

namespace FSM.Services
{
    public static class Common
    {
        private static string username = "vineethpv199@gmail.com";
        private static string password = "ATATT3xFfGF0as7SkDWwoSI_HYu71GM_8uUBHNYAMuAuDa2CK-wsCFMC7wI4Cqz5AMM6SW8ztGBgVcwt5epttgAY_mP8GdptzUBsYjEVP0VDxJmy077vzkiUSiCu5_STKQEneSi1KId7zO7TqsWxHmOSaOxYw1umvCAGV7Hjr1U5AsltJNLvFaU=E88727B3";
        private static string baseUrl = "https://vineethpv.atlassian.net";
        public static async Task<List<IssueModel>> GetAllIssues()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    var req = $"{baseUrl}/rest/api/2/search?jql= assignee = \"{username}\"";
                    var response = await client.GetAsync(req);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {

                            dynamic issueData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                            var issues = issueData.issues;
                            List<IssueModel> issueList = new List<IssueModel>();
                            foreach (var issue in issues)
                            {
                                string issueKey = issue.key;
                                string issueSummary = issue.fields.summary;
                                string reporter = issue.fields.reporter.displayName;
                                string assignee= issue.fields.assignee.displayName;
                                string createddate = issue.fields.created;
                                string taskstatus = issue.fields.status.name;
                                string description = issue.fields.description;
                                ImageSource avatar = await GetAvatar(issue.fields.assignee.avatarUrls);
                                List<AttachmentModel> attachments = await GetAllAttachments(issueKey);
                                List<CommentModel> comments = await GetAllComments(issueKey);
                                issueList.Add(new IssueModel { IssueKey = issueKey, IssueSummary = issueSummary, Reporter = reporter, CreatedDate = createddate, Status = taskstatus, Description = description, Attachments = attachments, Comments = comments, AssigneeAvatar = avatar,Assignee=assignee, IsStage1ScopeOfworkCompleted = false,IsDropDuctCompleted=false });
                            }
                            var observableIssues = new List<IssueModel>(issueList);
                            return observableIssues;
                            //return issueList; 
                        }
                        else
                        {
                            Console.WriteLine("Error: Empty response content.");
                            return new List<IssueModel>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return new List<IssueModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                List<IssueModel> issues = await GetAllIssuesWithRetries();
                return issues;
            }
        }
        public static async Task<List<IssueModel>> GetAllIssuesByStatus(string status)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    var req = $"{baseUrl}/rest/api/2/search?jql=status=\"{status}\" AND assignee = \"{username}\"";                    
                    var response = await client.GetAsync(req);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {

                            dynamic issueData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                            var issues = issueData.issues;
                            List<IssueModel> issueList = new List<IssueModel>();
                            foreach (var issue in issues)
                            {
                                string issueKey = issue.key;
                                string issueSummary = issue.fields.summary;
                                string reporter = issue.fields.reporter.displayName;
                                string createddate = issue.fields.created;
                                string taskstatus = issue.fields.status.name;
                                string description=issue.fields.description;                                
                                ImageSource avatar =await GetAvatar(issue.fields.assignee.avatarUrls);
                                List<AttachmentModel> attachments =await GetAllAttachments(issueKey);
                                List<CommentModel> comments = await GetAllComments(issueKey);
                                issueList.Add(new IssueModel { IssueKey = issueKey, IssueSummary = issueSummary, Reporter = reporter, CreatedDate = createddate, Status = taskstatus,Description=description,Attachments = attachments,Comments=comments,AssigneeAvatar= avatar });
                            }
                            var observableIssues = new List<IssueModel>(issueList);
                            return observableIssues;
                            //return issueList; 
                        }
                        else
                        {
                            Console.WriteLine("Error: Empty response content.");
                            return  new List<IssueModel>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return new List<IssueModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                List<IssueModel> issues = await GetAllIssuesByStatusWithRetries(status);
                return issues;
            }
        }
        public static async Task<List<AttachmentModel>> GetAllAttachments(string issueKey)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    var req = $"{baseUrl}/rest/api/2/issue/{issueKey}/?fields=attachment";
                    var response = await client.GetAsync(req);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            dynamic attachmentData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                            var attachments = attachmentData.fields.attachment;
                            List<AttachmentModel> attachmentList = new List<AttachmentModel>();
                            foreach (var attachment in attachments)
                            {
                                string id = attachment.id;
                                string filename = attachment.filename;
                                long sizeInBytes = attachment.size;
                                string author= attachment.author.displayName;
                                string dateadded = attachment.created;
                                double sizeInKB = Math.Ceiling((double)sizeInBytes / 1024);
                                bool isImage = IsImageFile(filename);
                                    attachmentList.Add(new AttachmentModel { Id = id, FileName = filename, Size = sizeInKB.ToString()+" KB", DateAdded = dateadded,Author=author,IsImage= isImage,IsDocument=!isImage });
                            }
                            return attachmentList;
                        }
                        else
                        {
                            Console.WriteLine("Error: Empty response content.");
                            return new List<AttachmentModel>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return new List<AttachmentModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                List<AttachmentModel> attachments = await GetAllAttachmentsWithRetries(issueKey);
                return attachments;
            }
        }
        public static async Task<byte[]> GetFileBytes(string attachmentId)
        {
            try
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    string attachmentUrl = $"{baseUrl}/rest/api/2/attachment/content/{attachmentId}";
                    var attachmentResponse = await client.GetAsync(attachmentUrl);
                    if (attachmentResponse.IsSuccessStatusCode)
                    {
                        return await attachmentResponse.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Error downloading attachment: {attachmentResponse.Content.ReadAsStringAsync().Result}");
                        return null;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                byte[] comments = await GetFileBytesWithRetries(attachmentId);
                return comments;
            }
            
        }
        public static async Task<List<CommentModel>> GetAllComments(string issueKey)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    var req = $"{baseUrl}/rest/api/2/issue/{issueKey}/comment";
                    var response = await client.GetAsync(req);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(content))
                        {
                            dynamic CommentData = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                            var Comments = CommentData.comments;
                            List<CommentModel> CommentList = new List<CommentModel>();
                            foreach (var Comment in Comments)
                            {
                                ContentType contenttype;
                                ImageSource imageSource=null;
                                string id = Comment.id;
                                string attachmentId = null;
                                string Bodycontent = Comment.body;
                                string author = Comment.author.displayName;
                                string dateadded = Comment.created;
                                bool isImage = IsImage(Bodycontent);
                                if (isImage)
                                {
                                    contenttype = ContentType.Image;
                                    var fileDetails = getFileDetails(Bodycontent, author, dateadded);
                                    var attachmentResult = await GetMatchingAttachmentBytes(fileDetails, issueKey);
                                    imageSource = attachmentResult.Item1;
                                    attachmentId = attachmentResult.Item2;                                                                        
                                }                                    
                                else
                                {
                                    contenttype = ContentType.Text;
                                }                                                              
                                CommentList.Add(new CommentModel { Id = id, TextContent = Bodycontent, DateAdded = dateadded, Author = author,Type= contenttype,ImageSource= imageSource,IsImage=isImage,IsText=!isImage,AttachmentId=attachmentId });
                            }
                            return CommentList;
                        }
                        else
                        {
                            Console.WriteLine("Error: Empty response content.");
                            return new List<CommentModel>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return new List<CommentModel>();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                List<CommentModel> comments = await GetAllCommentsWithRetries(issueKey);
                return comments;
            }
        }

        public static async Task<List<IssueModel>> GetAllIssuesWithRetries(int maxRetries = 5)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    var issues = await GetAllIssues();
                    return issues;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Retry {retries + 1} failed. Error: {ex.Message}");
                    retries++;
                    await Task.Delay(2000); 
                }
            }

            Console.WriteLine($"All retry attempts failed.");
            return new List<IssueModel>();
        }
        public static async Task<List<IssueModel>> GetAllIssuesByStatusWithRetries(string status, int maxRetries = 5)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    var issues = await GetAllIssuesByStatus(status);
                    return issues;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Retry {retries + 1} failed. Error: {ex.Message}");
                    retries++;
                    await Task.Delay(2000);
                }
            }

            Console.WriteLine($"All retry attempts failed.");
            return new List<IssueModel>();
        }
        public static async Task<List<AttachmentModel>> GetAllAttachmentsWithRetries(string issueKey, int maxRetries = 5)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    var attachments = await GetAllAttachments(issueKey);
                    return attachments;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Retry {retries + 1} failed. Error: {ex.Message}");
                    retries++;
                    await Task.Delay(2000);
                }
            }

            Console.WriteLine($"All retry attempts failed.");
            return new List<AttachmentModel>();
        }
        public static async Task<List<CommentModel>> GetAllCommentsWithRetries(string issueKey, int maxRetries = 5)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    var comments = await GetAllComments(issueKey);
                    return comments;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Retry {retries + 1} failed. Error: {ex.Message}");
                    retries++;
                    await Task.Delay(2000);
                }
            }

            Console.WriteLine($"All retry attempts failed.");
            return new List<CommentModel>();
        }
        public static async Task<byte[]> GetFileBytesWithRetries(string attachmentId, int maxRetries = 5)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    var issues = await GetFileBytes(attachmentId);
                    return issues;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Retry {retries + 1} failed. Error: {ex.Message}");
                    retries++;
                    await Task.Delay(2000);
                }
            }

            Console.WriteLine($"All retry attempts failed.");
            return null;
        }
        public static bool IsImage(string body)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(body) && body.Contains("|"))
            {
                string[] parts = body.Split('|');
                string lastPart = parts.LastOrDefault();

                if (!string.IsNullOrEmpty(lastPart) && (lastPart.Contains("width") || lastPart.Contains("height")))
                {
                    result = true;
                }
            }
            return result;
        }
        public static FileModel getFileDetails(string input,string author,string dateadded)
        {
            FileModel fileModel=new FileModel();
            try
            {
                string pattern = @"!(?<filename>[\s\w.\-\(\)]+)\|width=(?<width>\d+),height=(?<height>\d+)!";
                Match match = Regex.Match(input, pattern);
                if (match.Success)
                {
                    fileModel.FileName = match.Groups["filename"].Value;
                    fileModel.width = match.Groups["width"].Value;
                    fileModel.height = match.Groups["height"].Value;
                    fileModel.Author = author;
                    fileModel.DateAdded = dateadded;
                }
                else
                {
                    Console.WriteLine("No match found.");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return fileModel;
        }
        public static async Task<(ImageSource,string)> GetMatchingAttachmentBytes(FileModel fileModel,string issueKey)
        {
            ImageSource imageSource=null;
            List<AttachmentModel> attachments = await GetAllAttachments(issueKey);
            foreach (AttachmentModel attachment in attachments)
            {
                if (attachment.FileName == fileModel.FileName && attachment.Author == fileModel.Author)
                {
                    byte[] imageBytes = await GetFileBytes(attachment.Id);
                    if (imageBytes != null)
                    {
                        imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        return (imageSource, attachment.Id);
                    }
                    
                }
            }
            return (null,null);

        }
        public static async Task<ImageSource> GetAvatar(dynamic req)
        {
            ImageSource imageSource = null;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string avatarUrl = req["48x48"];
                    var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                    var response = await client.GetAsync(avatarUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                        if (imageBytes != null)
                        {
                            imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                imageSource = await GetAvatarWithRetries(req);
            }
            return imageSource;
        }
        public static async Task<ImageSource> GetAvatarWithRetries(dynamic req, int maxRetries = 5)
        {
            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    var imagesource = await GetAvatar(req);
                    return imagesource;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Retry {retries + 1} failed. Error: {ex.Message}");
                    retries++;
                    await Task.Delay(2000);
                }
            }

            Console.WriteLine($"All retry attempts failed.");
            return null;
        }
        public static bool IsImageFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }

            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".ico", ".svg", ".webp" };

            return imageExtensions.Contains(extension.ToLower());
        }
        public static async Task<string> GenerateLiveLocationSharingLink(LocationEx startingLocation, LocationEx destinationLocation)
        {
            // Create a new DirectionsRequest object.
            var directionsRequest = new DirectionsRequest()
            {
                // Set the origin and destination to the device's current location.
                Origin = startingLocation,
                Destination = destinationLocation,

                // Set the travel mode to driving.
                TravelMode = GoogleApi.Entities.Maps.Common.Enums.TravelMode.Driving
            };

            // Get the directions.
            var directionsResponse = await GetDirectionsAsync(directionsRequest);

            // Return the live location sharing link.
            //return directionsResponse.Routes[0].Legs[0].Steps[0].LiveLocationSharingLink;
            return null;
        }
        public static async Task<DirectionsResponse> GetDirectionsAsync(DirectionsRequest request)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {YOUR_API_KEY}");
            var requestUri = new Uri($"https://maps.googleapis.com/maps/api/directions/json?{request.ToQueryString()}");
            var response = await client.GetAsync(requestUri);
            var responseBody = await response.Content.ReadAsStringAsync();
            var directionsResponse = JsonConvert.DeserializeObject<DirectionsResponse>(responseBody);
            return directionsResponse;
        }
        public static string ToQueryString(this DirectionsRequest request)
        {
            try
            {
                var parameters = new Dictionary<string, string>();

                // Add the origin and destination parameters.
                parameters.Add("origin", request.Origin.ToString());
                parameters.Add("destination", request.Destination.ToString());

                // Add the travel mode parameter.
                parameters.Add("travel_mode", request.TravelMode.ToString().ToLowerInvariant());

                // Add any other parameters.
                // ...

                // Build the query string.
                var queryString = string.Join("&", parameters.Select(parameter => $"{parameter.Key}={parameter.Value}"));

                return queryString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
    }
}
