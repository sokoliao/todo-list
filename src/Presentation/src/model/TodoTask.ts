import { TodoTaskStatus } from "./TodoTaskStatus";

export interface TodoTask {
  id: string;
  name: string;
  priority: number;
  status: TodoTaskStatus;
}

export const defaultTodoTask: () => TodoTask = () => {
  return {
    id: '',
    name: '',
    priority: 1,
    status: TodoTaskStatus.NotStarted
  }
};