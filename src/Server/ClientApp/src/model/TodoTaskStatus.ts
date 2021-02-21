export enum TodoTaskStatus {
  NotStarted = "NotStarted",
  InProgress = "InProgress",
  Completed = "Completed"
}

export function parseTodoStatus(value: string): TodoTaskStatus {
  return (<any>TodoTaskStatus)[value];
}

export const getTodoStatusNames: () => string[] = () => Object.keys(TodoTaskStatus);
