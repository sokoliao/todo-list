import { TodoTask } from "../model/TodoTask";
import { TodoListState } from "./TodoList";

// ACTIONS

export interface RequestAllTodoTasksAction { type: 'REQUEST_ALL_TODO_TASKS'; }
export interface ReceiveTodoTasksAction {
  type: 'RECEIVE_TODO_TASKS',
  tasks: TodoTask[];
}

export type TaskRetrievalActions = RequestAllTodoTasksAction | ReceiveTodoTasksAction;

// REDUCERS

export interface RequestAllTodoTasksReducer {
  (action: RequestAllTodoTasksAction, state: TodoListState): TodoListState;
}
export interface ReceiveTodoTasksReducer {
  (action: ReceiveTodoTasksAction, state: TodoListState): TodoListState;
}

export interface TaskRetrievalReducers {
  requestAllTasks: RequestAllTodoTasksReducer;
  receiveTasks: ReceiveTodoTasksReducer;
}
export const reducers: TaskRetrievalReducers = {
  requestAllTasks: (_, state) => {
    return {...state};
  },
  receiveTasks: (action, state) => {
    return {
      ...state,
      tasks: action.tasks
    };
  }
}