import { stat } from "fs";
import { defaultTodoTask, TodoTask } from "../model/TodoTask";
import { TodoTaskStatus } from "../model/TodoTaskStatus";
import { defaultTaskValidationResult } from "../validation/TaskValidationResult";
import { validators } from "../validation/TodoListValidator";
import { TodoListState } from "./TodoList";

// ACTIONS

export interface CancelTaskEditingAction { type: 'CANCEL_TASK_EDITING' }
export interface StartTaskEditingAction {
  type: 'START_TASK_EDITING',
  id: string;
}
export interface TaskUpdatedAction { 
  type: 'TASK_UPDATED_ACTION',
  model: TodoTask
}
export interface EditTaskNameAction {
  type: 'EDIT_TASK_NAME',
  value: string
}
export interface EditTaskPriorityAction {
  type: 'EDIT_TASK_PRIORITY',
  value: number
}
export interface EditTaskStatusAction {
  type: 'EDIT_TASK_STATUS',
  value: TodoTaskStatus
}

export type TaskEditingActions = 
    CancelTaskEditingAction
  | StartTaskEditingAction
  | TaskUpdatedAction
  | EditTaskNameAction
  | EditTaskPriorityAction
  | EditTaskStatusAction

// REDUCERS

export interface CancelTaskEditingReducer {
  (action: CancelTaskEditingAction, state: TodoListState): TodoListState;
}
export interface StartTaskEditingReducer {
  (action: StartTaskEditingAction, state: TodoListState): TodoListState;
}
export interface TaskUpdatedReducer {
  (action: TaskUpdatedAction, state: TodoListState): TodoListState;
}
export interface EditTaskNameReducer {
  (action: EditTaskNameAction, state: TodoListState): TodoListState;
}
export interface EditTaskPriorityReducer {
  (action: EditTaskPriorityAction, state: TodoListState): TodoListState;
}
export interface EditTaskStatusReducer {
  (action: EditTaskStatusAction, state: TodoListState): TodoListState;
}
export interface TaskEditingReducers {
  cancelTaskEditing: CancelTaskEditingReducer;
  startTaskEditing: StartTaskEditingReducer;
  taskUpdated: TaskUpdatedReducer;
  editTaskName: EditTaskNameReducer;
  editTaskPriority: EditTaskPriorityReducer;
  editTaskStatus: EditTaskStatusReducer;
}

export const reducers: TaskEditingReducers = {
  cancelTaskEditing: (_, state) => {
    return {
      ...state,
      isEditTaskMenuOpen: false,
      taskInEdit: defaultTodoTask(),
      editTaskValidation: defaultTaskValidationResult()
    };
  },
  startTaskEditing: (action, state) => {
    const taskToEdit = state.tasks.find(task => task.id === action.id);
    if (taskToEdit !== undefined) {
      return {
        ...state,
        isEditTaskMenuOpen: true,
        taskInEdit: taskToEdit,
        editTaskValidation: {
          ...defaultTaskValidationResult(),
          ...validators.onEdit(taskToEdit, state.tasks)
        }
      }
    } else {
      return {
        ...state,
        isEditTaskMenuOpen: false,
        taskInEdit: defaultTodoTask(),
        editTaskValidation: defaultTaskValidationResult()
      }
    }
  },
  taskUpdated: (action, state) => {
    return {
      ...state,
      tasks: [
        action.model,
        ...state.tasks.filter(task => task.id !== action.model.id)
      ],
      isEditTaskMenuOpen: false,
      taskInEdit: defaultTodoTask(),
      editTaskValidation: defaultTaskValidationResult()
    }
  },
  editTaskName: (action, state) => {
    const next: TodoTask = {
      ...state.taskInEdit,
      name: action.value
    };
    return {
      ...state,
      taskInEdit: next,
      editTaskValidation: {
        ...state.editTaskValidation,
        isNamePristine: false,
        isModelPristine: false,
        ...validators.onEdit(next, state.tasks)
      }
    };
  },
  editTaskPriority: (action, state) => {
    const next: TodoTask = {
      ...state.taskInEdit,
      priority: action.value
    };
    return {
      ...state,
      taskInEdit: next,
      editTaskValidation: {
        ...state.editTaskValidation,
        isPriorityPristine: false,
        isModelPristine: false,
        ...validators.onEdit(next, state.tasks)
      }
    };
  },
  editTaskStatus: (action, state) => {
    const next: TodoTask = {
      ...state.taskInEdit,
      status: action.value
    };
    return {
      ...state,
      taskInEdit: next,
      editTaskValidation: {
        ...state.editTaskValidation,
        isModelPristine: false,
        ...validators.onEdit(next, state.tasks)
      }
    };
  },
}