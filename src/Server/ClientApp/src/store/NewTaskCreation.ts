import { AppThunkAction } from ".";
import { defaultTodoTask, TodoTask } from "../model/TodoTask";
import { TodoTaskStatus } from "../model/TodoTaskStatus";
import { defaultTaskValidationResult } from "../validation/TaskValidationResult";
import { validators } from "../validation/TodoListValidator";
import { TodoListState } from "./TodoList";

// ACTIONS

export interface StartNewTaskCreationAction { type: 'START_NEW_TASK_CREATION' }
export interface CreateNewTaskAction { type: 'CREATE_NEW_TASK' }
export interface CancelNewTaskCreationAction { type: 'CANCEL_NEW_TASK_CREATION' }
export interface NewTaskCreatedAction {
  type: 'NEW_TASK_CREATED',
  newTask: TodoTask
}
export interface ChangeNewTaskName {
  type: 'CHANGE_NEW_TASK_NAME',
  value: string
}
export interface ChangeNewTaskPriority {
  type: 'CHANGE_NEW_TASK_PRIORITY',
  value: number
}
export interface ChangeNewTaskStatus {
  type: 'CHANGE_NEW_TASK_STATUS',
  value: TodoTaskStatus
}

export type NewTaskCreationActions = 
    StartNewTaskCreationAction
  | CreateNewTaskAction
  | ChangeNewTaskPriority
  | NewTaskCreatedAction
  | CancelNewTaskCreationAction
  | ChangeNewTaskName
  | ChangeNewTaskPriority
  | ChangeNewTaskStatus

// REDUCERS

export interface StartNewTaskCreationReducer {
  (action: StartNewTaskCreationAction, state: TodoListState): TodoListState;
}
export interface CancelNewTaskCreationReducer {
  (action: CancelNewTaskCreationAction, state: TodoListState): TodoListState
}
export interface ChangeNewTaskNameReducer {
  (action: ChangeNewTaskName, state: TodoListState): TodoListState
}
export interface ChangeNewTaskPriorityReducer {
  (action: ChangeNewTaskPriority, state: TodoListState): TodoListState
}
export interface ChangeNewTaskStatusReducer {
  (action: ChangeNewTaskStatus, state: TodoListState): TodoListState
}
export interface NewTaskCreatedReducer {
  (action: NewTaskCreatedAction, state: TodoListState) : TodoListState
}

export interface NewTaskCreationReducers {
  startNewTaskCreation: StartNewTaskCreationReducer;
  cancelNewTaskCreation: CancelNewTaskCreationReducer;
  changeNewTaskName: ChangeNewTaskNameReducer;
  changeNewTaskPriority: ChangeNewTaskPriorityReducer;
  changeNewTaskStatus: ChangeNewTaskStatusReducer;
  newTaskCreated: NewTaskCreatedReducer;
}

export const reducers: NewTaskCreationReducers = {
  startNewTaskCreation: (_, state) => {
    return {
      ...state,
      isNewTaskMenuOpen: true,
      newTask: defaultTodoTask(),
      newTaskValidation: defaultTaskValidationResult()
    };
  },
  cancelNewTaskCreation: (_, state) => {
    return {
      ...state,
      isNewTaskMenuOpen: false,
      newTask: defaultTodoTask(),
      newTaskValidation: defaultTaskValidationResult()
    };
  },
  changeNewTaskName: (action, state) => {
    const next: TodoTask = {
      ...state.newTask,
      name: action.value
    };
    return {
      ...state,
      newTask: next,
      newTaskValidation: {
        ...state.newTaskValidation,
        isNamePristine: false,
        isModelPristine: false,
        ...validators.onCreate(next, state.tasks)
      }
    };
  },
  changeNewTaskPriority: (action, state) => {
    const next: TodoTask = {
      ...state.newTask,
      priority: action.value
    };
    return {
      ...state,
      newTask: next,
      newTaskValidation: {
        ...state.newTaskValidation,
        isPriorityPristine: false,
        isModelPristine: false,
        ...validators.onCreate(next, state.tasks)
      }
    };
  },
  changeNewTaskStatus: (action, state) => {
    const next: TodoTask = {
      ...state.newTask,
      status: action.value
    }
    return {
      ...state,
      newTask: next,
      newTaskValidation: {
        ...state.newTaskValidation,
        isModelPristine: false,
        ...validators.onCreate(next, state.tasks)
      }
    };
  },
  newTaskCreated: (action, state) => {
    return {
      ...state,
      tasks: [
        ...state.tasks,
        action.newTask
      ],
      isNewTaskMenuOpen: false,
      newTask: defaultTodoTask(),
      newTaskValidation: defaultTaskValidationResult()
    };
  }
}