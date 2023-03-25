using System.Net;
using System.Linq;
using System.Text.Json;
using VideoLibrary;
using Newtonsoft.Json.Linq;
using dotnet_player_client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Web;
using System.Net.Http.Headers;
using System;

namespace dotnet_player_client.Services
{
    public interface IYouTubeClientService
    {
        IAsyncEnumerable<int> DownloadYoutubeAudioAsync(string url, string FileName);
        Task<List<YoutubeInfo>?> SearchVideoByName(string query);
        string GetSafeFileName(string name, char replace = ' ');
    }

    public class YouTubeClientService : IYouTubeClientService
    {
        public const string YouTubeSearchUrl = "https://www.youtube.com/results?search_query=";
        public const string YouTubeBase = "https://www.youtube.com";
        public async Task<List<YoutubeInfo>?> SearchVideoByName(string query)
        {
            List<YoutubeInfo> videos = new List<YoutubeInfo>();
            try
            {
                using (var client = new HttpClient())
                {
                    var queryEncoded = query.Contains("//:") ? query : HttpUtility.UrlEncode(query);
                    string ? response = await client.GetStringAsync(YouTubeSearchUrl + queryEncoded);
                    if (response != null)
                    {
                        string start = "var ytInitialData = ";
                        string end = "};";
                        var startIndex = response.IndexOf(start) + start.Length;
                        var endIndex = response.IndexOf(end, startIndex);

                        var InitialData = JObject.Parse(response.Substring(startIndex, endIndex + 1 - startIndex));

                        var results = InitialData?["contents"]?["twoColumnSearchResultsRenderer"]?["primaryContents"]?["sectionListRenderer"]?["contents"]?[0]?["itemSectionRenderer"]?["contents"];

                        if (results != null)
                        {
                            foreach (var item in results)
                            {
                                var video_info = item["videoRenderer"];
                                var title = video_info?["title"]?["runs"]?[0]?["text"];
                                var url = video_info?["navigationEndpoint"]?["commandMetadata"]?["webCommandMetadata"]?["url"];
                                var length = video_info?["lengthText"]?["simpleText"];
                                var views = video_info?["shortViewCountText"]?["simpleText"];
                                var channel = video_info?["ownerText"]?["runs"]?[0]?["text"];

                                if (title != null && url != null && length != null
                                    && channel != null && views != null)
                                {
                                    videos.Add(new YoutubeInfo
                                    {
                                        TitleInfo = GetSafeFileName(title.ToString()),
                                        URLInfo = YouTubeBase + url.ToString(),
                                        LengthInfo = length.ToString(),
                                        ChannelInfo = channel.ToString(),
                                        ViewsInfo = views.ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return default;
            }

            return videos;
        }

        public async IAsyncEnumerable<int> DownloadYoutubeAudioAsync(string url, string FileName)
        {
            var youTube = YouTube.Default;
            var videos = await youTube.GetAllVideosAsync(url);
            var video = videos.Where(x => x.AdaptiveKind == AdaptiveKind.Audio).MinBy(x => x.AudioBitrate) ?? videos.First(x => x.AdaptiveKind == AdaptiveKind.Audio);

            var videoUrl = HttpUtility.ParseQueryString(video.Uri);
            long contentLength = 0;

            using (var client = new HttpClient())
            {
                var res = await (await Task.Factory.StartNew(() => client.SendAsync(new HttpRequestMessage(HttpMethod.Head, video.Uri))));
                contentLength = res.Content.Headers.ContentLength ?? 0;
            }

            using (var file = File.Create(FileName))
            {

                long globalNumBytesRead = 0;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("range", $"bytes=0-{contentLength}");
                    using var videoStream = await client.GetStreamAsync(videoUrl.ToString());

                    byte[] buffer = new byte[8192];

                    int currentBytes = 0;

                    while (globalNumBytesRead < contentLength)
                    {
                        currentBytes = await videoStream.ReadAsync(buffer, 0, buffer.Length);
                        globalNumBytesRead += currentBytes;
                        await file.WriteAsync(buffer, 0, currentBytes);

                        double percent = globalNumBytesRead / (contentLength * 1.0);
                        yield return (int)(percent * 100);
                    }
                }
            }
        }

        public string GetSafeFileName(string name, char replace = ' ')
        {
            char[] invalids = Path.GetInvalidFileNameChars();
            return new string(name.Select(c => invalids.Contains(c) ? replace : c).ToArray());
        }
    }
}