import { defaultTodoTask } from "../model/TodoTask";
import { TodoListState } from "./TodoList";

// ACTIONS

export interface StartDeletingTaskAction {
  type: 'START_DELETING_TASK',
  id: string;
}
export interface CancelTaskDeleteAction { type: 'CANCEL_TASK_DELETE' }
export interface SubmitTaskDeleteAction { type: 'SUBMIT_TASK_DELETE' }
export interface TaskDeletedAction {
  type: 'TASK_DELETED',
  id: string
}

export type TaskDeletionActions =
    StartDeletingTaskAction
  | CancelTaskDeleteAction
  | SubmitTaskDeleteAction
  | TaskDeletedAction;

// REDUCERS

export interface StartDeletingTaskReducer {
  (action: StartDeletingTaskAction, state: TodoListState): TodoListState;
}
export interface CancelTaskDeleteReducer {
  (action: CancelTaskDeleteAction, state: TodoListState): TodoListState;
}
export interface SubmitTaskDeleteReducer {
  (action: SubmitTaskDeleteAction, state: TodoListState): TodoListState;
}
export interface TaskDeletedReducer {
  (action: TaskDeletedAction, state: TodoListState): TodoListState;
}
export interface TaskDeletionReducers {
  startDeletingTask: StartDeletingTaskReducer,
  cancelTaskDelete: CancelTaskDeleteReducer,
  submitTaskDelete: SubmitTaskDeleteReducer,
  taskDeleted: TaskDeletedReducer
}
export const reducers: TaskDeletionReducers = {
  startDeletingTask: (action, state) => {
    const task = state.tasks.find(t => t.id === action.id);
    if (task) {
      return {
        ...state,
        isDeleteDialogOpen: true,
        taskToDelete: task
      }
    } else {
      return state;
    }
  },
  cancelTaskDelete: (_, state) => {
    return {
      ...state,
      isDeleteDialogOpen: false,
      taskToDelete: defaultTodoTask()
    }
  },
  submitTaskDelete: (_, state) => {
    // TODO start spinner
    return {
      ...state
    }
  },
  taskDeleted: (action, state) => {
    return {
      ...state,
      tasks: [
        ...state.tasks.filter(t => t.id !== action.id)
      ],
      isDeleteDialogOpen: false,
      taskToDelete: defaultTodoTask()
    }
  }
}