export enum GatewayEvent {
  /// <summary>
  /// The gateway event is not known.
  /// </summary>
  Unknown = 0,

  /// <summary>
  /// The event representing a created task.
  /// </summary>
  CreateTask = 1,

  /// <summary>
  /// The event representing an updated a task.
  /// </summary>
  UpdateTask = 2,

  /// <summary>
  /// The event representing a deleted task.
  /// </summary>
  DeleteTask = 3,

  /// <summary>
  /// The event representing a created object.
  /// </summary>
  CreateObject = 4,

  /// <summary>
  /// The event representing an updated object.
  /// </summary>
  UpdateObject = 5,

  /// <summary>
  /// The event representing a deleted object.
  /// </summary>
  DeleteObject = 6
}
