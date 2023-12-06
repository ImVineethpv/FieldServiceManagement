namespace FSM.Model;

public class IssueModel
{
    public string IssueKey { get; set; }
    public string IssueSummary { get; set; }
    public string Reporter { get; set; }
    public string Status { get; set; }
    public string CreatedDate { get; set; }
    public string RequestType { get; set; }
    public string Description { get; set; }
    public string Assignee { get; set; }
    public ImageSource AssigneeAvatar { get; set; }
    public string TimeToResolution { get; set; }
    public bool IsStage1ScopeOfworkCompleted { get; set; }
    public bool IsDropDuctCompleted { get; set; }
    public bool IsStage1WorkOrderCompleted { get; set; }
    public bool IsStage2WorkOrderCompleted { get; set; }
    public bool IsStage2ScopeOfworkCompleted { get; set; }
    public bool IsSplicingCompleted { get; set; }
    public bool IsLightLevelReadingCompleted { get; set; }
    public bool IsPostInstallFiberCompleted { get; set; }
    public bool IsONTSerialNumberCompleted { get; set; }
    public bool IsONTInstalled { get; set; }
    public bool IsSpeedTestCompleted { get; set; }
    public bool IsAdditionalEquipmentCompleted { get; set; }
    public bool IsFinalLayoutCompleted { get; set; }
    public bool IsInstallationCompleted { get; set; }
    public bool IsAdditionalNotesCompleted { get; set; }
    public string Title { get; set; }
    public string TitleValue { get; set; }

    public List<AttachmentModel> Attachments { get; set; }
    public List<CommentModel> Comments { get; set; }
}

