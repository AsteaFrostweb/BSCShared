using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCShared
{
    public class Entity
    {
        private readonly Dictionary<string, TaskCompletionSource<object[]>> pendingNotifications = new();
        private static readonly Dictionary<string, Entity> Entitys = new();

        public string UID {  get; set; }


        public Entity() 
        {
            UID = UIDGenerator.GenerateUID();
            Entitys.Add(UID, this);
        } 

        /// <summary>
        /// Wait for a notification with a specified tag
        /// </summary>
        /// <param name="tag">The tag of the notification</param>
        /// <returns></returns>
        public async Task<object[]> AwaitNotification(string tag) 
        {
            TaskCompletionSource<object[]> tcs = new TaskCompletionSource<object[]>();
            pendingNotifications[tag] = tcs;

            object[] _result = await tcs.Task;

            if (_result != null && _result.Length > 0)
                return _result;
            else 
                return null;
        }

        /// <summary>
        /// Notify this entity to allow it to continue the related awaiter if it has any
        /// </summary>
        /// <param name="tag">The tag of the notification</param>
        /// <param name="args">The arguments or parameters to pass to the awaiter</param>
        public void Notify(string tag, object[] args) 
        {
            if (pendingNotifications.TryGetValue(tag, out var tcs))
            {
                pendingNotifications.Remove(tag);
                tcs.SetResult(args);
            }
            else 
            {
                Console.WriteLine("Unable to find pending notification with specified tag - Entity.cs");
            }            
        }


        /// <summary>
        /// Notify a specified entity to allow it to continue related awaiters if it has any
        /// </summary>
        /// <param name="e">The entity to notify</param>
        /// <param name="tag">The tag of the notification</param>
        /// <param name="args">The arguments or parameters to pass to the awaiter</param>
        public static void Notify(Entity e, string tag, object[] args) 
        {
            Notify(e.UID, tag, args);
        }
        public static void Notify(string entityUID, string tag, object[] args) 
        {
            if (Entitys.TryGetValue(entityUID, out Entity ent))
            {
                if (ent != null)
                    ent.Notify(tag, args);
            }
            else 
            {
                Console.WriteLine("EUnable to locate specified antity by tag - ENtity.cs");
            }
        }
    }
}
