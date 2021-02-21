import { TodoListState } from "./TodoList";

// ACTIONS

export interface ShowErrorAction {
  type: 'SHOW_ERROR';
  message: string;
}

export interface HideErrorAction { type: 'HIDE_ERROR'; }

export type ErrorHandlingActions = ShowErrorAction | HideErrorAction;

// REDUCERS

export interface ShowErrorReducer {
  (action: ShowErrorAction, state: TodoListState): TodoListState;
}
export interface HideErrorReducer {
  (action: HideErrorAction, state: TodoListState): TodoListState;
}
export interface ErrorHandlingReducers {
  showError: ShowErrorReducer;
  hideError: HideErrorReducer;
}
export const reducers: ErrorHandlingReducers = {
  showError: (action, state) => {
    return {
      ...state,
      isErrorDisplayed: true,
      errorMessage: action.message
    };
  },
  hideError: (_, state) => {
    return {
      ...state,
      isErrorDisplayed: false
    }
  }
}