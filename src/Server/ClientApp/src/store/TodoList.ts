import { Action, Reducer } from 'redux';
import { TaskChange } from '../components/ModalTask';
import { AppThunkAction } from './';
import * as NewTaskCreation from './NewTaskCreation';
import * as TaskEditing from './TaskEditing';
import * as TaskDeletion from './TaskDeletion';
import * as TaskRetrieval from './TaskRetrieval';
import * as ErrorHandling from './ErrorHandling';
import { defaultTodoTask, TodoTask } from '../model/TodoTask';
import { Field } from '../model/Field';
import { defaultTaskValidationResult, TaskValidationErrors, TaskValidationResult } from '../validation/TaskValidationResult';

// STATE

export interface TodoListState {
  isLoading: boolean;
  tasks: TodoTask[];
  isOrderByAsc: boolean;
  isNewTaskMenuOpen: boolean;
  newTask: TodoTask;
  newTaskValidation: TaskValidationResult;
  isEditTaskMenuOpen: boolean;
  taskInEdit: TodoTask;
  editTaskValidation: TaskValidationResult;
  isDeleteDialogOpen: boolean;
  taskToDelete: TodoTask;
  isErrorDisplayed: boolean;
  errorMessage: string;
}

// TOGGLE sorting

interface ToggleOrderByAction { type: 'TOGGLE_ORDER_BY' }

type KnownAction = 
    ToggleOrderByAction
  | NewTaskCreation.NewTaskCreationActions
  | TaskRetrieval.TaskRetrievalActions
  | TaskEditing.TaskEditingActions
  | TaskDeletion.TaskDeletionActions
  | ErrorHandling.ErrorHandlingActions;

// ACTION CREATORS

export const actionCreators = {
  requestAllTodoTasks: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList) {
      fetch(
        `tasks/all`
      ).then(response => {
        if (!response.ok) {
          (response.json() as Promise<{message: string}>)
            .then(error => {
              dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${error.message}` })
            })
            .catch(_ => {
              dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${response.body}` })
            })
        } else {
          (response.json() as Promise<TodoTask[]>)
            .then(data => {
              dispatch({ type: 'RECEIVE_TODO_TASKS', tasks: data })
            })
            .catch(error => {
              dispatch({ type: 'SHOW_ERROR', message: `${error}` })
            })
        }
      })
      dispatch({ type: 'REQUEST_ALL_TODO_TASKS' });
    }
  },
  toggleNewTaskMenu: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList) {
      if (appState.todoList.isNewTaskMenuOpen) {
        dispatch({ type: 'CANCEL_NEW_TASK_CREATION' });
      } else {
        dispatch({ type: 'START_NEW_TASK_CREATION' });
      }
    }
  },
  handleNewTaskChange: (change: TaskChange): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList) {
      switch (change.field) {
        case Field.Name:
          dispatch({ 
            type: 'CHANGE_NEW_TASK_NAME', 
            value: change.value
          });
          break;
        case Field.Priority:
          dispatch({
            type: 'CHANGE_NEW_TASK_PRIORITY', 
            value: change.value
          });
          break;
        case Field.Status:
          dispatch({
            type: 'CHANGE_NEW_TASK_STATUS',
            value: change.value
          })
      }
    }
  },
  handleSubmit: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList && appState.todoList.newTask) {
      const task = appState.todoList.newTask;
      fetch('tasks/new', {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(appState.todoList.newTask)
      })      
        .then(response => {
          if (!response.ok) {
            (response.json() as Promise<{message: string}>)
              .then(error => {
                dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${error.message}` })
              })
              .catch(_ => {
                dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${response.body}` })
              })
              .finally(() => {
                dispatch({ type: 'CANCEL_NEW_TASK_CREATION' });
              });
          } else {
            (response.json() as Promise<{id: string}>)
              .then(response => {
                dispatch({ 
                  type: 'NEW_TASK_CREATED',
                  newTask: {
                    id: response.id,
                    name: task.name,
                    priority: task.priority,
                    status: task.status
                  } 
                });
              })
              .catch(_ => {
                dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${response.body}` })
              })
              .finally(() => {
                dispatch({ type: 'CANCEL_NEW_TASK_CREATION' });
              });
          }
        });
    }
  },
  toggleTaskEditMenu: (taskId: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList) {
      if (appState.todoList.isEditTaskMenuOpen) {
        dispatch({
          type: 'CANCEL_TASK_EDITING'
        });
      } else {
        dispatch({
          type: 'START_TASK_EDITING',
          id: taskId
        });
      }
    }
  },
  handleTaskEdit: (change: TaskChange): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList) {
      switch (change.field) {
        case Field.Name:
          dispatch({ 
            type: 'EDIT_TASK_NAME', 
            value: change.value
          });
          break;
        case Field.Priority:
          dispatch({
            type: 'EDIT_TASK_PRIORITY', 
            value: change.value
          });
          break;
        case Field.Status:
          dispatch({
            type: 'EDIT_TASK_STATUS',
            value: change.value
          })
      }
    }
  },
  handleTaskEditSubmit: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList && appState.todoList.newTask) {
      const task = appState.todoList.taskInEdit;
      fetch(`tasks/update/${task.id}`, {
        method: 'PUT',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(appState.todoList.taskInEdit)
      }).then(response => {
        if (!response.ok) {
          (response.json() as Promise<{message: string}>)
            .then(error => {
              dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${error.message}` })
            })
            .catch(_ => {
              dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${response.body}` })
            })
            .finally(() => {
              dispatch({ type: 'CANCEL_TASK_EDITING' })
            });
        } else {
          dispatch({
            type: 'TASK_UPDATED_ACTION',
            model: task
          })
        }
      });
    }
  },
  toggleDeleteDialog: (taskId: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList) {
      if (appState.todoList.isDeleteDialogOpen) {
        dispatch({
          type: 'CANCEL_TASK_DELETE'
        });
      } else if (appState.todoList.tasks.find(t => t.id === taskId)) {
        dispatch({
          type: 'START_DELETING_TASK',
          id: taskId
        });
      } else {
        dispatch({
          type: 'TASK_DELETED',
          id: taskId
        });
      }
    }
  },
  handleTaskDeleteSubmit: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList && appState.todoList.newTask) {
      const task = appState.todoList.taskToDelete;
      fetch(`tasks/delete/${task.id}`, {
        method: 'DELETE',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      }).then(response => {
        if (!response.ok) {
          (response.json() as Promise<{message: string}>)
            .then(error => {
              dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${error.message}` })
            })
            .catch(_ => {
              dispatch({ type: 'SHOW_ERROR', message: `${response.status} ${response.body}` })
            })
            .finally(() => {
              dispatch({ type: 'CANCEL_TASK_DELETE' })
            });
        } else {
          dispatch({
            type: 'TASK_DELETED',
            id: task.id
          })
        }
      });
    }
  },
  toggleOrderBy: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState &&  appState.todoList) {
      dispatch({ type: 'TOGGLE_ORDER_BY' });
    }
  },
  closeErrorPanel: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
    const appState = getState();
    if (appState && appState.todoList) {
      dispatch({ type: 'HIDE_ERROR' });
    }
  }
};

// REDUCER

const unloadedState: TodoListState = { 
  tasks: [], 
  isOrderByAsc: true,
  isLoading: false,
  isNewTaskMenuOpen: false,
  newTask: defaultTodoTask(),
  newTaskValidation: defaultTaskValidationResult(),
  isEditTaskMenuOpen: false,
  taskInEdit: defaultTodoTask(),
  editTaskValidation: defaultTaskValidationResult(),
  isDeleteDialogOpen: false,
  taskToDelete: defaultTodoTask(),
  isErrorDisplayed: false,
  errorMessage: '',
};

export const reducer: Reducer<TodoListState> = (state: TodoListState | undefined, incomingAction: Action): TodoListState => {
  if (state === undefined) {
    return unloadedState;
  }

  const action = incomingAction as KnownAction;
  console.log(action);
  switch (action.type) {
    case 'TOGGLE_ORDER_BY':
      return {
        ...state,
        isOrderByAsc: !state.isOrderByAsc
      }

    case 'REQUEST_ALL_TODO_TASKS':
      return TaskRetrieval.reducers.requestAllTasks(action, state);
    case 'RECEIVE_TODO_TASKS':
      return TaskRetrieval.reducers.receiveTasks(action, state);

    // CREATE
    case 'START_NEW_TASK_CREATION':
      return NewTaskCreation.reducers.startNewTaskCreation(action, state);
    case 'CANCEL_NEW_TASK_CREATION':
      return NewTaskCreation.reducers.cancelNewTaskCreation(action, state);
    case 'CHANGE_NEW_TASK_NAME': 
      return NewTaskCreation.reducers.changeNewTaskName(action, state);
    case 'CHANGE_NEW_TASK_PRIORITY':
      return NewTaskCreation.reducers.changeNewTaskPriority(action, state);
    case 'CHANGE_NEW_TASK_STATUS':
      return NewTaskCreation.reducers.changeNewTaskStatus(action, state);
    case 'NEW_TASK_CREATED':
      return NewTaskCreation.reducers.newTaskCreated(action, state);

    // EDIT actions
    case 'START_TASK_EDITING':
      return TaskEditing.reducers.startTaskEditing(action, state);
    case 'TASK_UPDATED_ACTION':
      return TaskEditing.reducers.taskUpdated(action, state);
    case 'CANCEL_TASK_EDITING':
      return TaskEditing.reducers.cancelTaskEditing(action, state);
    case 'EDIT_TASK_NAME':
      return TaskEditing.reducers.editTaskName(action, state);
    case 'EDIT_TASK_PRIORITY':
      return TaskEditing.reducers.editTaskPriority(action, state);
    case 'EDIT_TASK_STATUS':
      return TaskEditing.reducers.editTaskStatus(action, state);
    
    // DELETE actions
    case 'START_DELETING_TASK':
      return TaskDeletion.reducers.startDeletingTask(action, state);
    case 'CANCEL_TASK_DELETE':
      return TaskDeletion.reducers.cancelTaskDelete(action, state);
    case 'SUBMIT_TASK_DELETE':
      return TaskDeletion.reducers.submitTaskDelete(action, state);
    case  'TASK_DELETED':
      return TaskDeletion.reducers.taskDeleted(action, state);

    // ERROR HANDLING
    case 'SHOW_ERROR':
      return ErrorHandling.reducers.showError(action, state);
    case 'HIDE_ERROR':
      return ErrorHandling.reducers.hideError(action, state);
    default:
      return state;
  }
};
