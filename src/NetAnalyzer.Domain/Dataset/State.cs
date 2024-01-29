using System.ComponentModel;

namespace NetAnalyzer.Domain.Dataset;

public enum State
{
    [Description("New")]
    New,
    [Description("In process")]
    InProcess,
    [Description("Saved")]
    Saved,
    [Description("Deleted")]
    Deleted
}
