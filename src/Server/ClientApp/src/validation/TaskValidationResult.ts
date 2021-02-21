export interface TaskValidationErrors {
  nameErrors: string[];
  priorityErrors: string[];
  modelErrors: string[];
}

export interface TaskFormState {
  isNamePristine: boolean;
  isPriorityPristine: boolean;
  isModelPristine: boolean;
}

export type TaskValidationResult = TaskValidationErrors & TaskFormState

export const defaultTaskValidationResult: () => TaskValidationResult = () => {
  return {
    isNamePristine: true,
    nameErrors: [],
    isPriorityPristine: true,
    priorityErrors: [],
    isModelPristine: true,
    modelErrors: []
  };
}