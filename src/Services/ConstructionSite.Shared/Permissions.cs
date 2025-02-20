using System.Collections.Immutable;

namespace ConstructionSite.Shared;

public static class Permissions
{
    public static class Users
    {
        public const string View = "Users.View";
        public const string ViewWorkers = "Users.View.Workers";

        public static readonly ImmutableArray<string> Metrics =
           [View, ViewWorkers];
    }
    public static class ActivityTypes
    {
        public const string View = "ActivityTypes.View";

        public const string ViewForWorker = "ActivityTypes.ViewForWorker";

        public const string Create = "ActivityTypes.Create";

        public const string Edit = "ActivityTypes.Edit";

        public const string Delete = "ActivityTypes.Delete";

        public static readonly ImmutableArray<string> Metrics =
           [View, Create, Edit, Delete, ViewForWorker];
    }

    public static class Activities
    {
        public const string View = "Activities.View";

        public const string Create = "Activities.Create";

        public const string Edit = "Activities.Edit";

        public const string Delete = "Activities.Delete";

        public const string CreateForWorker = "Activities.CreateForWorker";

        public static readonly ImmutableArray<string> Metrics =
           [View, Create, Edit, Delete, CreateForWorker];
    }
}
