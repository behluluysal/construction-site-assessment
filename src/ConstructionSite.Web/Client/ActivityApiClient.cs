using ConstructionSite.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace ConstructionSite.Web.Client;

public class ActivityApiClient(HttpClient httpClient,
                               AuthenticationStateProvider authStateProvider,
                               NavigationManager navigationManager,
                               NotificationService notificationService,
                               AuthService authService)
    : ApiClientBase(httpClient, authStateProvider, navigationManager, notificationService, authService)
{

    #region [ Activity Type ]

    public async Task<ActivityTypesResponseData?> GetActivityTypesAsync(string queryString, bool isWorker = false)
    {
        string route = isWorker ? "api/activity-types/worker" : "api/activity-types";
        var response = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, $"{route}?{queryString}"));
        var apiResponse = await HandleApiResponseAsync<ActivityTypesResponseData>(response);

        return apiResponse?.Data;
    }

    public async Task<string?> CreateActivityTypeAsync(ActivityTypeViewModel activityType)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/activity-types")
        {
            Content = JsonContent.Create(new { activityType.Name })
        };

        var response = await SendRequestAsync(request,
            "Activity Type Created",
            $"Activity Type '{activityType.Name}' was successfully created.");
        return (await HandleApiResponseAsync<string>(response))?.Data;
    }

    public async Task UpdateActivityTypeAsync(ActivityTypeViewModel activityType)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/activity-types/{activityType.Id}")
        {
            Content = JsonContent.Create(new { activityType.Id, activityType.Name })
        };

        _ = await SendRequestAsync(request,
            "Activity Type Updated",
            "Activity Type '{activityType.Name}' was successfully updated.");
    }

    public async Task DeleteActivityTypeAsync(string id)
    {
        _ = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Delete, $"api/activity-types/{id}"),
            "Activity Type Deleted",
            "The activity type has been deleted successfully.");
    }

    #endregion

    #region [ Activity ]

    public async Task<ActivitiesResponseData?> GetActivitiesAsync(string queryString)
    {
        var response = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Get, $"api/activities?{queryString}"));
        var apiResponse = await HandleApiResponseAsync<ActivitiesResponseData>(response);
        return apiResponse?.Data;
    }

    public async Task<string?> CreateActivityAsync(ActivityViewModel activity, bool isWorker = false)
    {
        string route = isWorker ? "api/activities/worker" : "api/activities";
        var request = new HttpRequestMessage(HttpMethod.Post, route)
        {
            Content = JsonContent.Create(activity)
        };

        var response = await SendRequestAsync(request,
            "Activity Created",
            $"Activity '{activity.Description}' was successfully created.");
        return (await HandleApiResponseAsync<string>(response))?.Data;
    }

    public async Task UpdateActivityAsync(ActivityViewModel activity)
    {
        var request = new HttpRequestMessage(HttpMethod.Put, $"api/activities/{activity.Id}")
        {
            Content = JsonContent.Create(activity)
        };

        await SendRequestAsync(request,
            "Activity Updated",
            $"Activity '{activity.Description}' was successfully updated.");
    }

    public async Task DeleteActivityAsync(string id)
    {
        _ = await SendRequestAsync(new HttpRequestMessage(HttpMethod.Delete, $"api/activities/{id}"),
            "Activity Deleted",
            "The activity has been deleted successfully.");
    }

    #endregion
}

public class ActivityTypeViewModel
{
    public string Id { get; set; } = Ulid.NewUlid().ToString();
    public string Name { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        if (obj is ActivityTypeViewModel other)
        {
            return Id == other.Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public class ActivityTypesResponseData
{
    public List<ActivityTypeViewModel> ActivityTypes { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int ElementsInCurrentPage { get; set; }
}

public class ActivityViewModel
{
    public string Id { get; set; } = string.Empty;
    public string ActivityTypeId { get; set; } = string.Empty;
    public ActivityTypeViewModel ActivityType { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public string Worker { get; set; } = string.Empty;
    public DateTime ActivityDate { get; set; } = DateTime.UtcNow;
}

public class ActivitiesResponseData
{
    public List<ActivityViewModel> Activities { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int ElementsInCurrentPage { get; set; }
}

