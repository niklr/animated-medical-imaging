export enum GatewayEvent {
  /// <summary>
  /// The gateway event is not known.
  /// </summary>
  Unknown = 'Unknown',

  /// <summary>
  /// The event representing a created task.
  /// </summary>
  CreateTask = 'CreateTask',

  /// <summary>
  /// The event representing an updated a task.
  /// </summary>
  UpdateTask = 'UpdateTask',

  /// <summary>
  /// The event representing a deleted task.
  /// </summary>
  DeleteTask = 'DeleteTask',

  /// <summary>
  /// The event representing a created object.
  /// </summary>
  CreateObject = 'CreateObject',

  /// <summary>
  /// The event representing an updated object.
  /// </summary>
  UpdateObject = 'UpdateObject',

  /// <summary>
  /// The event representing a deleted object.
  /// </summary>
  DeleteObject = 'DeleteObject',

  /// <summary>
  /// The event representing a created worker.
  /// </summary>
  CreateWorker = 'CreateWorker',

  /// <summary>
  /// The event representing an updated worker.
  /// </summary>
  UpdateWorker = 'UpdateWorker',

  /// <summary>
  /// The event representing a deleted worker.
  /// </summary>
  DeleteWorker = 'DeleteWorker'
}
