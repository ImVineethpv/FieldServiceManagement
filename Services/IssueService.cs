using FSM.Services;
using System.Net.Http.Json;

namespace FSM.Services;

public class IssueService
{
    HttpClient httpClient;
    public IssueService()
    {
        this.httpClient = new HttpClient();
    }

    List<IssueModel> issueList;
    public async Task<List<IssueModel>> GetIssues()
    {
        if (issueList?.Count > 0)
            return issueList;

        issueList=await Common.GetAllIssues();

        return issueList;
    }
}
