using Microsoft.AspNetCore.Mvc;
using plain.Entities;
using plain.Services;
using plain.Models;
using Microsoft.AspNetCore.Authorization;

namespace plain.Controllers;


/// <summary>
/// issueのコントローラー
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class IssueController : ControllerBase
{
    private readonly ILogger<IssueController> _logger;
    private readonly IIssueRepository _issueRepository;

    /// <summary>
    /// コンストラクター
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="issueRepository"></param>
    public IssueController(ILogger<IssueController> logger, IIssueRepository issueRepository)
    {
        _logger = logger;
        _issueRepository = issueRepository;
    }

    /// <summary>
    /// 全てのissueを取得する。
    /// GET: api/issue
    /// </summary>
    /// <returns>issueのリスト</returns>
    /// <response code="200">成功</response>
    /// <response code="500">失敗</response>
    [HttpGet]
    public ActionResult<IssueResponce> Get()
    {
        var issues = new IssueResponce(_issueRepository.GetAll());

        return Ok(issues);
    }

    /// <summary>
    /// 指定したIDのissueを取得する。
    /// GET: api/issue/{id}
    /// </summary>
    /// <param name="id">issueのID</param>
    /// <returns>issue</returns>
    /// <response code="200">成功</response>
    /// <response code="404">指定したIDのissueは存在しない</response>
    /// <response code="500">サーバーエラー</response>
    [HttpGet("{id}")]
    public ActionResult<IssueResponce> Get(int id)
    {
        var issue = _issueRepository.Get(id);
        if (issue == null)
        {
            return NotFound();
        }

        var responce = new IssueResponce(new List<Issue>() { issue });
        return Ok(responce);
    }

    /// <summary>
    /// issueを追加する。
    /// POST: api/issue
    /// </summary>
    /// <param name="issue">issue</param>
    /// <returns>追加したissue</returns>
    /// <response code="200">成功</response>
    /// <response code="400">リクエストボディがnull</response>
    /// <response code="401">認証エラー</response>
    /// <response code="409">IDが重複している</response>
    /// <response code="500">失敗</response>
    [HttpPost]
    [Authorize]
    public ActionResult<IssueResponce> Post(IssueRequest issue)
    {
        if (issue.Issue == null)
        {
            return BadRequest(new IssueResponce(new List<Issue>(), "Request body is null."));
        }

        var newIssue = _issueRepository.Add(issue.Issue);
        if (newIssue == null)
        {
            return Conflict(new IssueResponce(new List<Issue>(), "Id conflict."));
        }

        var ret = new IssueResponce(new List<Issue>() { newIssue });

        return Ok(ret);
    }

    /// <summary>
    /// issueを更新する。
    /// </summary>
    /// <param name="id"></param>
    /// <param name="issue"></param>
    /// <returns></returns>
    /// <response code="200">成功</response>
    /// <response code="400">リクエストボディがnull</response>
    /// <response code="401">認証エラー</response>
    /// <response code="404">指定したIDのissueは存在しない</response>
    /// <response code="500">失敗</response>
    [HttpPut("{id}")]
    [Authorize]
    public ActionResult<IssueResponce> Put(int id, [FromBody] IssueRequest issue)
    {
        if (issue.Issue == null)
        {
            return BadRequest(new IssueResponce(new List<Issue>(), "Request body is null."));
        }

        var ret = _issueRepository.Update(issue.Issue);
        if (!ret)
        {
            return NotFound(new IssueResponce(new List<Issue>(), "Not found."));
        }

        return Ok(new IssueResponce(new List<Issue>() { issue.Issue }));
    }
}