using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CodeforcesApi.Core.Models;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace CodeforcesApi.Core
{
    public class CodeforcesClient : HttpClient
    {
        public static CodeforcesClient Instance { get; }
        private static readonly JsonSerializerOptions _serializerOptions;
        static CodeforcesClient()
        {
            Instance = new CodeforcesClient();
            _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            _serializerOptions.Converters.Add(new JsonStringEnumConverter());
        }
        private CodeforcesClient() : base(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })
        {
            BaseAddress = new Uri("http://codeforces.com/api/");
            DefaultRequestHeaders.UserAgent.ParseAdd(@"Mozilla/5.0 (X11; Linux x86_64; rv:90.0) Gecko/20100101 Firefox/90.0");
            DefaultRequestHeaders.AcceptLanguage.ParseAdd(@"en-US,en;q=0.5");
            DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
            DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            DefaultRequestHeaders.Add("X-MicrosoftAjax", "Delta=true");
            DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };
            DefaultRequestHeaders.Connection.Add("keep-alive");
            DefaultRequestHeaders.Accept.ParseAdd("application/json");
        }
        public async ValueTask<CodeforcesResponse<SubmissionDto[]>> GetUserSubmissions(string userHandle, int offset, int count)
        {
            using var submissionStream = await GetStreamAsync($@"http://codeforces.com/api/user.status?handle={userHandle}&from={offset}&count={count}").ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<CodeforcesResponse<SubmissionDto[]>>(submissionStream, _serializerOptions).ConfigureAwait(false);
        }
        public async ValueTask<CodeforcesResponse<ContestDto[]>> GetContests(bool includeGYMContests)
        {
            using var contestsStream = await GetStreamAsync($@"http://codeforces.com/api/contest.list?gym={includeGYMContests}").ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<CodeforcesResponse<ContestDto[]>>(contestsStream, _serializerOptions).ConfigureAwait(false);
        }
        public async ValueTask<CodeforcesResponse<SubmissionDto[]>> GetUserSubmissionsInContest(int contestId, string userHandle, int offset, int count)
        {
            using var submissionsInContestStream = await GetStreamAsync($@"http://codeforces.com/api/contest.status?contestId={contestId}&handle={userHandle}&from={offset}&count={count}").ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<CodeforcesResponse<SubmissionDto[]>>(submissionsInContestStream, _serializerOptions).ConfigureAwait(false);
        }
        public async ValueTask<IHtmlDocument?> GetProblemPage(Uri questionUri, CancellationToken cancellationToken = default)
        {
            var parser = new HtmlParser();
            using var problemResponse = await GetAsync(questionUri, cancellationToken).ConfigureAwait(false);
            if (!problemResponse.IsSuccessStatusCode) { return null; }
            using var problemStream = await problemResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return await parser.ParseDocumentAsync(problemStream, cancellationToken).ConfigureAwait(false);
        }
    }
}
