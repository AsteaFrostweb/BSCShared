using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Entity
{
    private readonly Dictionary<string, List<TaskCompletionSource<object[]>>> pendingNotifications = new Dictionary<string, List<TaskCompletionSource<object[]>>>();
    private static readonly Dictionary<string, Entity> Entities = new Dictionary<string, Entity>();

    public string UID { get; set; }

    public Entity()
    {
        UID = UIDGenerator.GenerateUID();
        Entities.Add(UID, this);
    }

    public async Task<object[]> AwaitNotification(string tag)
    {
        var tcs = new TaskCompletionSource<object[]>();

        lock (pendingNotifications)
        {
            if (!pendingNotifications.TryGetValue(tag, out var list))
            {
                list = new List<TaskCompletionSource<object[]>>();
                pendingNotifications[tag] = list;
            }
            list.Add(tcs);
        }

        return await tcs.Task;
    }

    public void Notify(string tag, object[] args)
    {
        List<TaskCompletionSource<object[]>> subscribers = null;

        lock (pendingNotifications)
        {
            if (pendingNotifications.TryGetValue(tag, out subscribers))
            {
                pendingNotifications.Remove(tag);
            }
        }

        if (subscribers != null)
        {
            foreach (var tcs in subscribers)
            {
                try
                {
                    tcs.TrySetResult(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error notifying subscriber: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine($"No pending notification for tag '{tag}' - Entity.cs");
        }
    }

    public static void Notify(Entity e, string tag, object[] args) => Notify(e.UID, tag, args);

    public static void Notify(string entityUID, string tag, object[] args)
    {
        if (Entities.TryGetValue(entityUID, out Entity ent))
        {
            ent?.Notify(tag, args);
        }
        else
        {
            Console.WriteLine($"Unable to locate entity with UID {entityUID} - Entity.cs");
        }
    }
}
