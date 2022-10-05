using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Domain.Shared.Entities.Bot.Dtos.BitbucketWebHook
{
    public class BitbucketWebHook
    {
        public string AuthorName { get; set; }
        public string PullRequestLink { get; set; }
        public string PullRequestTitle { get; set; }
        public DateTime PullRequestDate { get; set; }
        public string RepositoryName { get; set; }
        public string RepositoryLink { get; set; }
        public int PullRequestId { get; set; }

    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Activity
    {
        public string href { get; set; }
    }

    public class Actor
    {
        public string display_name { get; set; }
        public Links links { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
        public string account_id { get; set; }
        public string nickname { get; set; }
    }

    public class Approve
    {
        public string href { get; set; }
    }

    public class Author
    {
        public string display_name { get; set; }
        public Links links { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
        public string account_id { get; set; }
        public string nickname { get; set; }
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class Branch
    {
        public string name { get; set; }
    }

    public class Comments
    {
        public string href { get; set; }
    }

    public class Commit
    {
        public string type { get; set; }
        public string hash { get; set; }
        public Links links { get; set; }
    }

    public class Commits
    {
        public string href { get; set; }
    }

    public class Decline
    {
        public string href { get; set; }
    }

    public class Description
    {
        public string type { get; set; }
        public string raw { get; set; }
        public string markup { get; set; }
        public string html { get; set; }
    }

    public class Destination
    {
        public Branch branch { get; set; }
        public Commit commit { get; set; }
        public Repository repository { get; set; }
    }

    public class Diff
    {
        public string href { get; set; }
    }

    public class Diffstat
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Self self { get; set; }
        public Html html { get; set; }
        public Avatar avatar { get; set; }
        public Commits commits { get; set; }
        public Approve approve { get; set; }

        [JsonProperty("request-changes")]
        public RequestChanges RequestChanges { get; set; }
        public Diff diff { get; set; }
        public Diffstat diffstat { get; set; }
        public Comments comments { get; set; }
        public Activity activity { get; set; }
        public Merge merge { get; set; }
        public Decline decline { get; set; }
        public Statuses statuses { get; set; }
    }

    public class Merge
    {
        public string href { get; set; }
    }

    public class Owner
    {
        public string display_name { get; set; }
        public Links links { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
        public string account_id { get; set; }
        public string nickname { get; set; }
    }

    public class Participant
    {
        public string type { get; set; }
        public User user { get; set; }
        public string role { get; set; }
        public bool approved { get; set; }
        public object state { get; set; }
        public object participated_on { get; set; }
    }

    public class Project
    {
        public string type { get; set; }
        public string key { get; set; }
        public string uuid { get; set; }
        public string name { get; set; }
        public Links links { get; set; }
    }

    public class Pullrequest
    {
        public int comment_count { get; set; }
        public int task_count { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Rendered rendered { get; set; }
        public string state { get; set; }
        public object merge_commit { get; set; }
        public bool close_source_branch { get; set; }
        public object closed_by { get; set; }
        public Author author { get; set; }
        public string reason { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public Destination destination { get; set; }
        public Source source { get; set; }
        public List<Reviewer> reviewers { get; set; }
        public List<Participant> participants { get; set; }
        public Links links { get; set; }
        public Summary summary { get; set; }
    }

    public class Rendered
    {
        public Title title { get; set; }
        public Description description { get; set; }
    }

    public class Repository
    {
        public string type { get; set; }
        public string full_name { get; set; }
        public Links links { get; set; }
        public string name { get; set; }
        public string scm { get; set; }
        public object website { get; set; }
        public Owner owner { get; set; }
        public Workspace workspace { get; set; }
        public bool is_private { get; set; }
        public Project project { get; set; }
        public string uuid { get; set; }
    }

    public class RequestChanges
    {
        public string href { get; set; }
    }

    public class Reviewer
    {
        public string display_name { get; set; }
        public Links links { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
        public string account_id { get; set; }
        public string nickname { get; set; }
    }

    public class BitbucketWebHookRequest
    {
        public Repository repository { get; set; }
        public Actor actor { get; set; }
        public Pullrequest pullrequest { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Source
    {
        public Branch branch { get; set; }
        public Commit commit { get; set; }
        public Repository repository { get; set; }
    }

    public class Statuses
    {
        public string href { get; set; }
    }

    public class Summary
    {
        public string type { get; set; }
        public string raw { get; set; }
        public string markup { get; set; }
        public string html { get; set; }
    }

    public class Title
    {
        public string type { get; set; }
        public string raw { get; set; }
        public string markup { get; set; }
        public string html { get; set; }
    }

    public class User
    {
        public string display_name { get; set; }
        public Links links { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
        public string account_id { get; set; }
        public string nickname { get; set; }
    }

    public class Workspace
    {
        public string type { get; set; }
        public string uuid { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public Links links { get; set; }
    }


}
