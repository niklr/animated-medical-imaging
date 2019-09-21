using AMI.Domain.Attributes;

namespace AMI.Domain.Enums.Auditing
{
    /// <summary>
    /// A type to describe a sub event.
    /// </summary>
    public enum SubEventType
    {
        /// <summary>
        /// The sub event not representing any operation.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.None)]
        None,

        /// <summary>
        /// The sub event representing an operation to create an object.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Create)]
        CreateObject,

        /// <summary>
        /// The sub event representing an operation to update an object.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Update)]
        UpdateObject,

        /// <summary>
        /// The sub event representing an operation to delete an object.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Delete)]
        DeleteObject,

        /// <summary>
        /// The sub event representing an operation to download an object.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Read)]
        DownloadObject,

        /// <summary>
        /// The sub event representing an operation to create a task.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Create)]
        CreateTask,

        /// <summary>
        /// The sub event representing an operation to update a task.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Update)]
        UpdateTask,

        /// <summary>
        /// The sub event representing an operation to delete a task.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Delete)]
        DeleteTask,

        /// <summary>
        /// The sub event representing an operation to delete a webhook endpoint.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Create)]
        CreateWebhook,

        /// <summary>
        /// The sub event representing an operation to update a webhook endpoint.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Update)]
        UpdateWebhook,

        /// <summary>
        /// The sub event representing an operation to delete a webhook endpoint.
        /// </summary>
        [EventType(BaseEventType = BaseEventType.Delete)]
        DeleteWebhook
    }
}
